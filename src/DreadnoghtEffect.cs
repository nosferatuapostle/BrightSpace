using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class DreadnoughtEffect : UnitEffect
{
    private bool spawnerIsReady;
    private int maxCount;
    private CountdownTimer cd;
    private List<Unit> commandedUnitList;

    private List<int> freeIndexList;

    public DreadnoughtEffect(Unit target) : base(target)
    {
        name = "Dreadnought Commander";

        commandedUnitList = [];

        spawnerIsReady = true;

        UpdateMaxCount();

        freeIndexList = [];

        cd = new CountdownTimer(8f);

        target.OnLevelIncrease += LevelIncreaseEvent;
        target.OnDeath += DeathEvent;
    }

    private void DeathEvent(Unit dying, Unit killer)
    {
        OnEnd();
    }

    private void CommandedDeathEvent(Unit dying, Unit killer)
    {
        cd.Restart();

        freeIndexList.Add(dying.formationIndex);

        dying.OnDeath -= CommandedDeathEvent;

        commandedUnitList.Remove(dying);
    }

    private void LevelIncreaseEvent()
    {
        UpdateMaxCount();
        KillAll();
        CommandedSpawner();
    }

    private void UpdateMaxCount()
    {
        maxCount = MathHelper.Clamp(target.level / 15 + 1, 1, Unit.MaxCommandedCount);
    }

    private int ApplyFreeIndex()
    {
        var index = freeIndexList.FirstOrDefault();
        freeIndexList.Remove(index);
        return index;
    }

    private void KillAll()
    {
        foreach (var u in commandedUnitList)
        {
            u.OnDeath -= CommandedDeathEvent;
            u.Kill(u);
        }

        commandedUnitList.Clear();
    }

    private void CommandedSpawner()
    {
        Coroutine.Start(DelayedCommandedSpawn());
    }

    private IEnumerator DelayedCommandedSpawn()
    {
        var missing = maxCount - commandedUnitList.Count;
        if (missing <= 0 || !spawnerIsReady)
        {
            yield break;
        }
        spawnerIsReady = false;

        var radius = 40f + target.radius;
        for (int i = 0; i < missing; i++)
        {
            var unit = Factory.CreateUnit(UnitType.Scout, target.faction);

            unit.scale *= 0.8f;

            unit.position = target.position;

            var index = commandedUnitList.Count;
            if (freeIndexList.Count > 0)
            {
                index = ApplyFreeIndex();
            }
            unit.SetLeader(target, index);

            unit.isPlayerTeammate = target.isPlayer;
            unit.isAIControl = true;

            var unitLevel = MathHelper.Max(1, target.level / 2);

            unit.level = unitLevel;
            unit.LevelUp(unitLevel - 1);

            unit.RestoreFullHealth();

            unit.AddValueModifier(UnitValue.Magnitude, ModifierData.COMMANDED_DEBUFF, new ValueModifier(-0.8f, ModifierType.PercentMult));

            unit.OnDeath += CommandedDeathEvent;

            commandedUnitList.Add(unit);

            yield return Coroutine.WaitForSeconds(0.2f);
        }

        spawnerIsReady = true;
    }

    protected override void OnTick(GameTime gameTime)
    {
        cd.Update(gameTime);
        if (!target.isDead && cd.State == TimerState.Completed)
        {
            CommandedSpawner();
        }
    }

    protected override void OnEnd()
    {
        KillAll();
        target.OnLevelIncrease -= LevelIncreaseEvent;

        base.OnEnd();
    }
}