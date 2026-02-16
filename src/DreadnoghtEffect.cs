using System;
using System.Collections.Generic;
using BrightSpace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;

public class DreadnoughtEffect : UnitEffect
{
    private int maxCount;
    private CountdownTimer cd;
    private List<Unit> commandedUnitList;

    public DreadnoughtEffect(Unit target) : base(target)
    {
        commandedUnitList = [];

        UpdateMaxCount();

        cd = new CountdownTimer(8f);

        target.OnLevelIncrease += LevelIncreaseEvent;
    }

    private void DeathEvent(Unit dying, Unit killer)
    {
        dying.OnDeath -= DeathEvent;
        commandedUnitList.Remove(dying);
        cd.Restart();
    }

    private void LevelIncreaseEvent(Unit unit)
    {
        UpdateMaxCount();
        KillAll();
        CommandedSpawner();
    }

    private void UpdateMaxCount()
    {
        maxCount = MathHelper.Clamp(target.level / 15 + 1, 1, 7);
    }

    private void KillAll()
    {
        foreach (var u in commandedUnitList)
        {
            u.OnDeath -= DeathEvent;
            u.Kill();
        }

        commandedUnitList.Clear();
    }

    private void CommandedSpawner()
    {
        var missing = maxCount - commandedUnitList.Count;
        if (missing <= 0)
        {
            return;
        }

        var radius = 40f + target.radius;
        for (int i = 0; i < missing; i++)
        {
            var index = commandedUnitList.Count;

            var angle = MathHelper.TwoPi * index / maxCount;

            var offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius;

            var unit = Factory.CreateUnit(UnitType.Scout, target.faction);

            unit.position = target.position + offset;
            unit.isPlayerTeammate = target.isPlayer;

            unit.level = target.level;
            unit.LevelUp(target.level - 1);

            unit.RestoreFullHealth();

            unit.OnDeath += DeathEvent;

            commandedUnitList.Add(unit);
        }
    }

    protected override void OnTick(GameTime gameTime)
    {
        if (Input.Keyboard.WasKeyReleased(Keys.B))
        {
            if (commandedUnitList.Count > 0)
            {
                commandedUnitList[Utils.Random.Next(0, commandedUnitList.Count)].Kill();                
            }
        }

        cd.Update(gameTime);
        if (cd.State == TimerState.Completed)
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