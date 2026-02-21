using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace BrightSpace;

public class Spawner
{
    private int counter;

    public Spawner()
    {
        counter = 0;

        // for (int i = 0; i < 10; i++)
        // {
        //     var a = new Asteroid();
        //     a.position = new Vector2(64 * i + 1, 64);
        //     World.UnitList.Add(a);
        // }

        Coroutine.Start(SpawnSystem());
    }

    private IEnumerator SpawnSystem()
    {
        while (true)
        {
            yield return Coroutine.WaitForSeconds(Utils.Random.Next(2, 8)); 

            var player = World.PlayerUnit;

            if (counter >= 100 || player == null)
            {
                continue;
            }
            counter++;

            var bounds = World.Camera.BoundingRectangle;
            var offset = 50f;

            var side = Utils.Random.Next(0, 4);
            var randFaction = Utils.Random.Next(0, 2);
            var randType = Utils.Random.Next(0, 7);

            var position = side switch
            {
                0 => new Vector2(bounds.Left - offset, bounds.Top + Utils.Random.Next(0, (int)bounds.Height)),
                1 => new Vector2(bounds.Right + offset, bounds.Top + Utils.Random.Next(0, (int)bounds.Height)),
                2 => new Vector2(bounds.Left + Utils.Random.Next(0, (int)bounds.Width), bounds.Top - offset),
                _ => new Vector2(bounds.Left + Utils.Random.Next(0, (int)bounds.Width), bounds.Bottom + offset),
            };

            var faction = randFaction switch
            {
                0 => Faction.IronCorps,
                1 => Faction.Biomantes,
                _ => Faction.DuskFleet
            };

            var type = randType switch
            {
                0 => UnitType.Battlecruiser,
                1 => UnitType.Bomber,
                2 => UnitType.Dreadnought,
                3 => UnitType.Fighter,
                4 => UnitType.Frigate,
                5 => UnitType.Scout,
                6 => UnitType.Support,
                _ => UnitType.Torpedo
            };

            int playerLevel = player.level;
            int minLevel = MathHelper.Max(1, playerLevel - 12);
            int maxLevel = playerLevel + 24;

            // int unitLevel = Utils.Random.Next(minLevel, maxLevel + 1);
            var stdDev = 2f + (playerLevel / 10f);
            int unitLevel = GenerateUnitLevel(playerLevel, minLevel, maxLevel, stdDev);

            var unit = Factory.CreateUnit(type, faction);

            unit.position = position;

            unit.level = unitLevel;
            unit.LevelUp(unitLevel - 1);

            unit.RestoreFullHealth();

            unit.isAIControl = true;

            unit.OnDamage += DamageEvent;
            unit.OnDeath += DeathEvent;
        }
    }

    private void DamageEvent(Unit source, Unit victim, ref float damage)
    {
        victim.SetUnitTarget(source, damage);
    }

    private void DeathEvent(Unit dying, Unit killer)
    {
        counter--;
        dying.OnDamage -= DamageEvent;
        dying.OnDeath -= DeathEvent;
    }
    
    private int GenerateUnitLevel(int mean, int min, int max, float stdDev)
    {
        int level;
        int attempts = 0;
        const int maxAttempts = 100;
        
        do
        {
            var u1 = 1f - Utils.Random.NextSingle();
            var u2 = 1f - Utils.Random.NextSingle();
            
            var randStdNormal = MathF.Sqrt(-2f * MathF.Log(u1)) * MathF.Sin(2f * MathF.PI * u2);
            
            var randNormal = mean + stdDev * randStdNormal;
            
            level = (int)Math.Round(randNormal);
            level = MathHelper.Max(1, level);
            
            attempts++;
            
            if (attempts >= maxAttempts)
            {
                level = mean;
                break;
            }
        }
        while (level < min || level > max);
        
        return level;
    }
}