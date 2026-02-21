using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public delegate void OnKillEvent(Unit dying);
public delegate void OnDeathEvent(Unit dying, Unit killer);
public delegate void OnAttackEvent(Unit target);
public delegate void OnHitEvent(Unit victim, ref float damage);
public delegate void OnDamageEvent(Unit source, Unit victim, ref float damage);
public delegate void OnValueChangedEvent(UnitValue unitValue);
public delegate void OnLevelIncreaseEvent();

public abstract class Unit : BasicObject
{
    public const int MaxCommandedCount = 8;

    public OnKillEvent OnKill;
    public OnDeathEvent OnDeath;
    public OnAttackEvent OnAttack;
    public OnHitEvent OnHit;
    public OnDamageEvent OnDamage;
    public OnValueChangedEvent OnValueChanged;
    public OnLevelIncreaseEvent OnLevelIncrease;

    public bool isPlayer;
    public bool isAIControl;
    public bool isPlayerTeammate;
    public bool isCannotDie;
    public bool isDead;
    public bool isDestroyed;

    private HashSet<Tag> tagList;
    private Dictionary<UnitValue, ValueInfo> values;
    private Dictionary<UnitValue, ResourceValue> resources;

    private EffectTarget effectTarget;

    public int level;

    protected float baseSpeed;
    protected float baseHealRate;

    public float radius;
    public float extraHeight;

    public string name;
    public Faction faction;
    public UnitType type;

    private Vector2 moveTarget;

    private Unit leader;
    public int formationIndex;

    private Dictionary<Unit, float> unitTargetList;

    private PseudoRandom critChance;

    private bool canAttack;
    private WeaponType weaponType;
    protected float baseDamage;
    protected float baseAttackSpeed;
    private CountdownTimer attackDelay;

    private CountdownTimer visionDelay;

    public Unit(float health)
    {
        radius = 16f;
        extraHeight = 0f;

        isPlayer = false;
        isAIControl = false;
        isPlayerTeammate = false;
        isCannotDie = false;
        isDead = false;
        isDestroyed = false;

        level = 1;

        baseSpeed = 100f;
        baseHealRate = 0.01f;

        tagList = [];

        values = [];
        resources = [];


        var unitValue = UnitValue.Health;
        var healthValue = AddValue(unitValue, health, 0f);

        AddValue(UnitValue.HealRate, 1f, 0f);
        AddValue(UnitValue.DamageResist, 0f, 0f);
        AddValue(UnitValue.MoveSpeed, 1f, 0f);
        AddValue(UnitValue.Range, 0f, 0f);
        AddValue(UnitValue.Magnitude, 1f, 0f);
        AddValue(UnitValue.AttackSpeed, 1f, 0.1f, 10f);
        AddValue(UnitValue.CritChance, 0.01f, 0.01f, 1f);
        AddValue(UnitValue.CritRate, 1.2f, 0.1f);

        AddResource(unitValue, healthValue);

        unitTargetList = [];

        canAttack = true;
        critChance = new PseudoRandom(GetBaseValue(UnitValue.CritChance), 0.5f);
        attackDelay = new CountdownTimer(0f);

        effectTarget = new EffectTarget();

        visionDelay = new CountdownTimer(0.25f);
    }

    public bool IsHovered
    {
        get
        {
            var mousePos = World.MousePosition;
            var distance = (mousePos - position).Length();
            return distance < radius * 1.2f;
        }
    }

    public virtual bool IsMoving
    {
        get { return moveTarget != Vector2.Zero; }
    }

    public bool UnitTargetIsNull
    {
        get { return unitTarget == null; }
    }

    public bool HasLeader
    {
        get { return leader != null; }
    }

    public Vector2 formationPosition
    {
        get
        {
            var radius = leader.radius + 64f;

            var angleStep = MathHelper.TwoPi / MaxCommandedCount;
            var angle = formationIndex * angleStep;

            var offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius;
            return leader.position + offset;
        }
    }

    public override void PostCreate()
    {
        base.PostCreate();

        SetWeapon(Data.LightningBolt());

        OnValueChanged += UpdateValueEvent;
        OnDeath += DeathEvent;
    }

    public override void PostDeath()
    {
        base.PostDeath();
        OnValueChanged -= UpdateValueEvent;
        OnDeath -= DeathEvent;
    }
    
    private void UpdateValueEvent(UnitValue unitValue)
    {
        if (unitValue == UnitValue.AttackSpeed)
        {
            UpdateAttackDelay();
        }
        else if (unitValue == UnitValue.CritChance)
        {
            UpdateCritChance();
        }
    }

    public virtual void DeathEvent(Unit dying, Unit killer)
    {
        isDead = true;
        killer.OnKill?.Invoke(this);
    }

    public virtual void MoveTo(Vector2 target)
    {
        moveTarget = target;
    }

    public virtual void MoveStop()
    {
        moveTarget = Vector2.Zero;
    }

    public virtual void SetUnitTarget(Unit target, float agressionLevel = 1f)
    {
        if (target == null)
        {
            unitTargetList.Clear();
            return;
        }
        else if (target.isDead || target == this || target == leader)
        {
            return;
        }

        if (!unitTargetList.ContainsKey(target))
        {
            unitTargetList[target] = agressionLevel;
        }

        unitTargetList[target] += agressionLevel;
    }

    public virtual Unit unitTarget
    {
        get
        {
            Unit best = null;
            var maxThreat = 0f;
            foreach (var kvp in unitTargetList)
            {
                if (kvp.Key.isDead)
                {
                    unitTargetList.Remove(kvp.Key);
                    continue;
                }

                if (kvp.Value > maxThreat)
                {
                    maxThreat = kvp.Value;
                    best = kvp.Key;
                }
            }

            return best;
        }
    }
    
    public void UpdateVision(GameTime gameTime)
    {
        visionDelay.Update(gameTime);
        if (visionDelay.State == TimerState.Completed)
        {
            visionDelay.Restart();
            foreach (var u in World.UnitList)
            {
                if (u.isDead || !u.HostileTo(this))
                {
                    continue;
                }

                const float maxDist = 540f;
                var distance = (u.position - position).Length();
                if (distance > maxDist)
                {
                    continue;
                }

                var bonus = (maxDist - distance) * 0.01f;
                SetUnitTarget(u, 1f + bonus);
                break;
            }
        }
    }

    public virtual Unit GetLeader()
    {
        return leader;
    }

    public virtual void SetLeader(Unit unit, int index)
    {
        leader = unit;
        formationIndex = index;

        leader.OnDamage += LeaderHitEvent;
    }

    private void LeaderHitEvent(Unit source, Unit victim, ref float damage)
    {
        SetUnitTarget(source, damage);
    }

    public virtual void ClearLeader()
    {
        leader = null;
    }

    public bool HasTag(Tag tag)
    {
        return tagList.Contains(tag);
    }

    public void AddTag(Tag tag)
    {
        tagList.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        tagList.Remove(tag);
    }

    public virtual float GetHealRate()
    {
        return baseHealRate * GetBaseValue(UnitValue.HealRate);
    }

    public virtual float GetHealAmount(float baseHealth)
    {
        return baseHealth * GetHealRate();
    }

    public virtual float GetSpeedValue()
    {
        var speed = baseSpeed * GetBaseValue(UnitValue.MoveSpeed);
        return MathHelper.Clamp(speed, 0f, 222f);
    }

    public virtual float GetAttackSpeed()
    {
        return baseAttackSpeed / GetBaseValue(UnitValue.AttackSpeed);
    }

    public virtual float Magnitude(float newValue)
    {
        return newValue * GetBaseValue(UnitValue.Magnitude);
    }

    public virtual float GetBaseDamageValue()
    {
        return Magnitude(baseDamage);
    }

    public float GetValue(UnitValue unitValue)
    {
        if (resources.TryGetValue(unitValue, out var resourceValue))
        {
            return resourceValue.value;
        }
        return 0f;
    }

    public float GetBaseValue(UnitValue unitValue)
    {
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            return valueInfo.value;
        }
        return 0f;
    }

    public ValueInfo AddValue(UnitValue unitValue, float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        var value = new ValueInfo(baseValue, minValue, maxValue);
        values[unitValue] = value;

        return value;
    }

    public ResourceValue AddResource(UnitValue unitValue, ValueInfo valueInfo)
    {
        var value = new ResourceValue(valueInfo);
        resources[unitValue] = value;

        return value;
    }

    public void SetBaseValue(UnitValue unitValue, float value)
    {
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            valueInfo.baseValue = value;
        }
    }

    public void SetValue(UnitValue unitValue, float value)
    {
        if (resources.TryGetValue(unitValue, out var resourceValue))
        {
            resourceValue.value = value;
        }
    }

    public void RestoreValue(UnitValue unitValue, float amount)
    {
        if (resources.TryGetValue(unitValue, out var resourceValue))
        {
            resourceValue.value += amount;
        }
    }

    public void RestoreFullHealth()
    {
        var unitValue = UnitValue.Health;
        RestoreValue(unitValue, GetBaseValue(unitValue));
    }

    public bool HasValueModifer(UnitValue unitValue, ModifierData data)
    {
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            return valueInfo.HasModifer(data);
        }
        return false;
    }

    public ValueModifier GetValueModifier(UnitValue unitValue, ModifierData data)
    {
        ValueModifier modifier = null;
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            modifier = valueInfo.GetModifier(data);
        }
        return modifier;
    }

    public void AddValueModifier(UnitValue unitValue, ModifierData data, ValueModifier valueModifier)
    {
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            valueInfo.AddModifier(data, valueModifier);
            OnValueChanged?.Invoke(unitValue);
        }
    }

    public void RemoveValueModifier(UnitValue unitValue, ModifierData data)
    {
        if (values.TryGetValue(unitValue, out var valueInfo))
        {
            valueInfo.RemoveModifier(data);
            OnValueChanged?.Invoke(unitValue);
        }
    }

    public UnitEffect AddEffect(UnitEffect unitEffect)
    {
        return effectTarget.Add(unitEffect);
    }

    public override void Update(GameTime gameTime)
    {
        var baseHealth = GetBaseValue(UnitValue.Health);
        if (GetValue(UnitValue.Health) < baseHealth)
        {
            var heal = GetHealAmount(baseHealth) * Time.Delta;
            RestoreValue(UnitValue.Health, heal);
        }

        if (IsMoving)
        {
            var dir = moveTarget - position;
            if (dir.Length() > 8f)
            {
                dir.Normalize();
                position += dir * GetSpeedValue() * Time.Delta;
            }
            else
            {
                MoveStop();
            }
            RotateTo(moveTarget);
        }

        effectTarget.Update(gameTime);

        attackDelay.Update(gameTime);
        if (attackDelay.State == TimerState.Completed)
        {
            canAttack = true;
        }

        if (!UnitTargetIsNull && !unitTarget.isDead)
        {
            var len = (position - unitTarget.position).Length();
            if (len > GetBaseValue(UnitValue.Range) + radius)
            {
                MoveTo(unitTarget.position);
            }
            else
            {
                if (IsMoving)
                {
                    MoveStop();
                }
                float angle = MathF.Abs(MathHelper.WrapAngle(rotation - position.ToAngle(unitTarget.position)));
                if (angle < 0.1f)
                {
                    Shoot();
                }
                else
                {
                    RotateTo(unitTarget.position);
                }
            }
        }
    }

    public void RotateTo(Vector2 target)
    {
        var rotationSpeed = baseSpeed * 0.1f;
        var targetRotation = MathF.Atan2(target.Y - position.Y, target.X - position.X) + MathHelper.PiOver2;
        var rotationDifference = MathHelper.WrapAngle(targetRotation - rotation);
        var maxRotation = rotationSpeed * Time.Delta;
        if (MathF.Abs(rotationDifference) < maxRotation)
        {
            rotation = targetRotation;
            return;
        }

        rotation += MathHelper.Clamp(rotationDifference, -maxRotation, maxRotation);
    }

    public virtual void DebugLog()
    {
        System.Diagnostics.Debug.WriteLine("");
        System.Diagnostics.Debug.WriteLine($"name: {name}");
        System.Diagnostics.Debug.WriteLine($"level: {level}");
        System.Diagnostics.Debug.WriteLine($"health: {GetBaseValue(UnitValue.Health)}");
        System.Diagnostics.Debug.WriteLine($"magnitude: {GetBaseValue(UnitValue.Magnitude)}");
        System.Diagnostics.Debug.WriteLine($"speed mult: {GetBaseValue(UnitValue.MoveSpeed)}");
        System.Diagnostics.Debug.WriteLine($"damage: {GetBaseDamageValue()}");
        System.Diagnostics.Debug.WriteLine($"speed: {GetSpeedValue()}");
        System.Diagnostics.Debug.WriteLine($"range: {GetBaseValue(UnitValue.Range)}");
        System.Diagnostics.Debug.WriteLine($"attack speed: {GetAttackSpeed()}");
        System.Diagnostics.Debug.WriteLine("");
    }

    public virtual void LevelUp(float mult = 1f)
    {
        // DebugLog();
        UpdateAttackDelay();
        UpdateCritChance();
        OnLevelIncrease?.Invoke();
    }

    public virtual bool HostileTo(Unit unit)
    {
        if (unitTargetList.ContainsKey(unit) || unit.faction != faction)
        {
            return true;
        }

        return false;
    }

    public float CalculateDamage(float damage, bool isPure = false)
    {
        // if (damage <= 0f)
        // {
        //     return 0f;
        // }
        // if (isPure)
        // {
        //     // ignorearmor
        // }
        
        var endDamage = damage;

        var critRate = GetBaseValue(UnitValue.CritRate);

        if (critChance.Check())
        {
            endDamage *= critRate;
        }
        
        return endDamage;
    }

    public virtual void TakeDamage(Unit source, float damage)
    {
        if (!isDead)
        {
            source.OnHit?.Invoke(this, ref damage);
            OnDamage?.Invoke(source, this, ref damage);
            RestoreValue(UnitValue.Health, -CalculateDamage(damage));
            if (GetValue(UnitValue.Health) <= 0 && !isCannotDie)
            {
                Kill(source);
            }
        }
    }

    public virtual void Kill(Unit source)
    {
        OnDeath?.Invoke(this, source);
    }

    public void SetWeapon(WeaponData weapon)
    {
        weaponType = weapon.type;
        baseDamage = weapon.damage;
        baseAttackSpeed = weapon.attackSpeed;
        UpdateAttackDelay();
        SetBaseValue(UnitValue.Range, weapon.range);
    }

    public void UpdateAttackDelay()
    {
        attackDelay.Interval = TimeSpan.FromSeconds(GetAttackSpeed());
    }

    public void UpdateCritChance()
    {
        critChance.baseChance = GetBaseValue(UnitValue.CritChance);
    }

    private void Shoot()
    {
        if (canAttack)
        {
            OnAttack?.Invoke(unitTarget);
            CreateProjectile(new ProjectileContext
            {
                owner = this,
                targetUnit = unitTarget,
                targetPosition = unitTarget.position
            });
        }
    }

    public virtual Projectile CreateProjectile(ProjectileContext context)
    {
        canAttack = false;
        attackDelay.Restart();
        return Factory.CreateProjectile(weaponType, context);
    }
}