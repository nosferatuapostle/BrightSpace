using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input;
using MonoGame.Extended.Timers;

namespace BrightSpace;

public class Player
{
    public Unit playerUnit;

    private Experience experience;

    private MoveMarker moveMarker;
    private CountdownTimer delay;

    public static Vector2 SpawnPosition = Vector2.Zero;

    public Player()
    {
        moveMarker = new MoveMarker();
        delay = new CountdownTimer(0.125f);

        _ = new Spawner();

        experience = new Experience();
    }

    public void SetPlayerUnit(Unit unit)
    {
        playerUnit = unit;

        playerUnit.name = "PLAYER";
        playerUnit.isPlayer = true;
        playerUnit.isCannotDie = false;

        playerUnit.RestoreFullHealth();

        playerUnit.position = SpawnPosition;

        playerUnit.AddValueModifier(UnitValue.HealRate, ModifierData.PLAYER_BUFF, new ValueModifier(4f, ModifierType.PercentMult));
        playerUnit.AddValueModifier(UnitValue.Magnitude, ModifierData.PLAYER_BUFF, new ValueModifier(1f, ModifierType.PercentMult));

        playerUnit.OnDamage += DamageEvent;
        playerUnit.OnKill += KillEvent;
        playerUnit.OnRemoved += RemoveEvent;

        experience.Reset();
        experience.OnLevelUp += LevelUpEvent;
    }

    private void DamageEvent(Unit source, Unit victim, ref float damage)
    {
        damage *= 0.75f;
    }

    private void KillEvent(Unit dying)
    {
        experience.AddKillReward(dying);
    }

    private void RemoveEvent()
    {
        Main.GameState = GameState.SelectState;
        
        SpawnPosition = playerUnit.position;

        playerUnit.OnDamage -= DamageEvent;
        playerUnit.OnKill -= KillEvent;
        playerUnit.OnRemoved -= RemoveEvent;
        experience.OnLevelUp -= LevelUpEvent;
    }
    
    private void LevelUpEvent()
    {
        playerUnit.level++;
        playerUnit.LevelUp();
    }

    public void Update(GameTime gameTime)
    {
        if (playerUnit != null && !playerUnit.isDead)
        {
            World.Camera.LookAt(playerUnit.position);

            var mousePos = World.MousePosition;

            delay.Update(gameTime);
            if (Input.Mouse.IsButtonDown(MouseButton.Right) && delay.State == TimerState.Completed)
            {
                delay.Restart();

                Unit targetUnit = null;
                for (int i = 0; i < World.UnitList.Count; i++)
                {
                    var u = World.UnitList[i];
                    if (u.IsHovered && !u.isPlayer)
                    {
                        targetUnit = u;
                        break;
                    }
                }

                if (targetUnit != null)
                {
                    MoveMarker.Add(Color.DarkRed);
                    playerUnit.SetUnitTarget(targetUnit);
                }
                else
                {
                    MoveMarker.Add(Color.LimeGreen);
                    playerUnit.SetUnitTarget(null);
                    playerUnit.MoveTo(mousePos);
                }
            }

            if (Input.Keyboard.IsKeyDown(Keys.W) && delay.State == TimerState.Completed)
            {
                delay.Restart();
                playerUnit.MoveTo(mousePos);
                MoveMarker.Add(Color.LimeGreen);
            }
            if (Input.Keyboard.IsKeyDown(Keys.S) && delay.State == TimerState.Completed)
            {
                playerUnit.MoveStop();
                playerUnit.SetUnitTarget(null);
            }

            if (Input.Keyboard.WasKeyReleased(Keys.F))
            {
                playerUnit.isCannotDie = !playerUnit.isCannotDie;
            }

            if (Input.Keyboard.WasKeyReleased(Keys.E))
            {
                playerUnit.RestoreFullHealth();
            }

            if (Input.Keyboard.WasKeyReleased(Keys.Q))
            {
                playerUnit.TakeDamage(playerUnit, Utils.Random.Next(1, 10));
            }

            if (Input.Keyboard.WasKeyReleased(Keys.R))
            {
                playerUnit.isCannotDie = !playerUnit.isCannotDie;
                playerUnit.Kill(playerUnit);
            }

            if (Input.Keyboard.WasKeyReleased(Keys.OemPlus))
            {
                experience.AddExp(experience.GetNextLevelXP());
            }
        }
        moveMarker.Update(gameTime);
    }

    public void UpdateInUnitLoop(Unit unit, GameTime gameTime)
    {
        if (unit.isPlayer || !unit.isAIControl || unit.isDead)
        {
            return;
        }

        unit.UpdateVision(gameTime);
        
        if (unit.HasLeader)
        {
            UpdateMinionAI(unit, gameTime);   
        }
        else if (unit.UnitTargetIsNull && !unit.IsMoving)
        {
            const float roamRadius = 1280f;
            var offset = new Vector2(Utils.Random.NextSingle(-roamRadius, roamRadius),
                Utils.Random.NextSingle(-roamRadius, roamRadius)
            );
            var target = World.Camera.Center + offset;
            unit.MoveTo(target);
        }
    }
    
    private void UpdateMinionAI(Unit unit, GameTime gameTime)
    {
        var leader = unit.GetLeader();

        if (leader == null || leader.isDead)
        {
            unit.ClearLeader();
            return;
        }

        if (!leader.UnitTargetIsNull)
        {
            unit.SetUnitTarget(leader.unitTarget);
            return;
        }

        var distance = Vector2.Distance(unit.position, unit.formationPosition);
        if (distance > 640f)
        {
            unit.SetUnitTarget(null);
        }
        else if (unit.UnitTargetIsNull && distance > 8f)
        {
            unit.MoveTo(unit.formationPosition);
        }
    }

    public void DrawMarker(SpriteBatch spriteBatch)
    {
        moveMarker.Draw(spriteBatch);
    }

    public void DrawUI(SpriteBatch spriteBatch, FramesPerSecondCounter fps)
    {
        if (playerUnit != null)
        {
            var centerX = Screen.Width / 2f;

            var barSize = new SizeF(Screen.Width / 4, 6);

            var barPosition = new Vector2(centerX - barSize.Width / 2, Screen.Height - Screen.Height * 0.95f);

            var xp = experience.GetXP();
            var nextLevelXp = experience.GetNextLevelXP();

            var xpProgress = nextLevelXp > 0 ? xp / nextLevelXp : 0f;

            var progressWidth = barSize.Width * xpProgress;
            var currentBarRect = new RectangleF(barPosition, new SizeF(progressWidth, barSize.Height));
            var totalBarRect = new RectangleF(barPosition, barSize);

            var text = $"{(int)xp} / {(int)nextLevelXp} XP";
            var textSize = Data.Font14.MeasureString(text);
            var textPos = new Vector2(Screen.Width / 2 - textSize.Width / 2, barPosition.Y + textSize.Height / 2 + 10);

            spriteBatch.FillRectangle(totalBarRect, Color.White * 0.1f);

            spriteBatch.FillRectangle(currentBarRect, Color.Goldenrod);

            spriteBatch.DrawRectangle(totalBarRect, Color.Black);

            spriteBatch.DrawString(Data.Font14, text, textPos, Color.White);

            var restartText = "PRESS R TO RESTART";
            var textRestartSize = Data.Font14.MeasureString(restartText);
            var restartTextPos = new Vector2(textPos.X - textRestartSize.Width / 4, textPos.Y + textRestartSize.Height / 2 + 40);
            spriteBatch.DrawString(Data.Font14, restartText, restartTextPos, Color.White);

            DrawPlayerInfo(spriteBatch, fps);
        }
    }
    
    private void DrawPlayerInfo(SpriteBatch spriteBatch, FramesPerSecondCounter fps)
    {
        var panelPadding = 25f;
        var lineHeight = Data.Font14.LineSpacing + 12f;
        
        var statsX = 25f;
        var statsY = 35f;

        var baseHealth = playerUnit.GetBaseValue(UnitValue.Health);
        
        var statLines = new List<string>
        {
            // $"FPS: {fps.FramesPerSecond}",
            // $"UnitCount: {World.UnitList.Count}",
            // $"ProjectileCount: {World.ProjectileList.Count}",
            // $"EntityCount: {World.UnitList.Count + World.ProjectileList.Count}",
            "",
            $"Name: {playerUnit.name}",
            $"Type, Faction: {playerUnit.type}, {playerUnit.faction}",
            $"Level: {playerUnit.level}",
            $"Health: {baseHealth}",
            $"HealRate: {playerUnit.GetHealAmount(baseHealth):F2}",
            $"Speed Mult: {playerUnit.GetBaseValue(UnitValue.MoveSpeed):F2}",
            $"Speed: {playerUnit.GetSpeedValue():F2}",
            $"Range: {playerUnit.GetBaseValue(UnitValue.Range):F2}",
            $"Magnitude: {playerUnit.GetBaseValue(UnitValue.Magnitude):F2}",
            $"Damage: {playerUnit.GetBaseDamageValue():F2}",
            $"AttackSpeed: {playerUnit.GetBaseValue(UnitValue.AttackSpeed):F2}",
            $"AttackDelay: {playerUnit.GetAttackSpeed():F2}",
            $"CritChance: {playerUnit.GetBaseValue(UnitValue.CritChance):F2}",
            $"CritRate: {playerUnit.GetBaseValue(UnitValue.CritRate):F2}"
        };
        
        var maxTextWidth = 0f;
        foreach (var line in statLines)
        {
            var size = Data.Font14.MeasureString(line);
            if (size.Width > maxTextWidth)
            {
                maxTextWidth = size.Width;
            }
        }
        
        var panelWidth = maxTextWidth + panelPadding * 2f;
        var panelHeight = (lineHeight * statLines.Count) + panelPadding * 1.5f;
        
        var panelRect = new RectangleF(5f + statsX - panelPadding, statsY - panelPadding, panelWidth, panelHeight);
        spriteBatch.FillRectangle(panelRect, new Color(0, 0, 0, 200));
        spriteBatch.DrawRectangle(panelRect, Color.Goldenrod);

        var currentY = statsY;
        foreach (var line in statLines)
        {
            Color textColor;
            
            if (line.StartsWith("Name:") || line.StartsWith("Level:"))
            {
                textColor = Color.Goldenrod;
            }
            else if (line.StartsWith("Health:"))
            {
                textColor = Color.LightGreen;
            }
            else if (line.Contains("Damage"))
            {
                textColor = Color.Orange;
            }
            else if (line.Contains("Speed"))
            {
                textColor = Color.LightBlue;
            }
            else
            {
                textColor = Color.White;
            }
            
            spriteBatch.DrawString(Data.Font14, line, new Vector2(statsX + 2, currentY + 2), Color.Black * 0.5f);
            spriteBatch.DrawString(Data.Font14, line, new Vector2(statsX, currentY), textColor);
                
            currentY += lineHeight;
        }
    }
}