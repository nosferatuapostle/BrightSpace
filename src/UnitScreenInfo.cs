using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace BrightSpace;

public class UnitScreenInfo
{
    private Dictionary<Unit, HealthBarData> healthBarList = [];

    private class HealthBarData
    {
        public float green;
        public float red;
        public float faded;
    }

    public Color textColor;

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var unit in World.UnitList)
        {
            if (unit.isDead)
            {
                continue;
            }

            var unitValue = UnitValue.Health;
            var baseHealth = unit.GetBaseValue(unitValue);
            var currentHealth = unit.GetValue(unitValue);

            if (!healthBarList.TryGetValue(unit, out var hb))
            {
                healthBarList[unit] = new HealthBarData
                {
                    green = currentHealth,
                    red = currentHealth,
                    faded = currentHealth
                };
                continue;
            }
            
            hb.green = currentHealth;

            var speed = baseHealth * 0.125f;

            if (hb.red < hb.green)
            {
                hb.red += speed * Time.Delta;
                if (hb.red > hb.green) hb.red = hb.green;
            }
            else if (hb.red > hb.green)
            {
                hb.red = hb.green;
            }

            if (hb.faded < hb.green)
            {
                hb.faded += speed * Time.Delta;
                if (hb.faded > hb.green) hb.faded = hb.green;
            }
            else if (hb.faded > hb.green)
            {
                hb.faded -= speed * Time.Delta;
                if (hb.faded < hb.green) hb.faded = hb.green;
            }

            hb.faded = MathHelper.Clamp(hb.faded, 0f, baseHealth);

            var barSize = new Vector2(36, 4);
            var barPosition = unit.position + new Vector2(-barSize.X / 2f, unit.radius + 4f + unit.extraHeight);

            var greenPercent = hb.green / baseHealth;
            var redPercent = hb.red / baseHealth;
            var fadedPercent = hb.faded / baseHealth;

            var allAligned = MathF.Abs(hb.green - hb.red) < 0.01f && Math.Abs(hb.green - hb.faded) < 0.01f;
            var fullHp = Math.Abs(hb.green - baseHealth) < 0.01f;

            if (!(allAligned && fullHp))
            {
                spriteBatch.FillRectangle(new RectangleF(barPosition.X, barPosition.Y, barSize.X, barSize.Y), Color.White * 0.2f);
                spriteBatch.FillRectangle(new RectangleF(barPosition.X, barPosition.Y, barSize.X * fadedPercent, barSize.Y), Color.DarkRed * 0.5f);
                spriteBatch.FillRectangle(new RectangleF(barPosition.X, barPosition.Y, barSize.X * greenPercent, barSize.Y), Color.LimeGreen * 0.8f);
                spriteBatch.FillRectangle(new RectangleF(barPosition.X, barPosition.Y, barSize.X * redPercent, barSize.Y), Color.DarkRed);
                spriteBatch.DrawRectangle(new RectangleF(barPosition.X, barPosition.Y, barSize.X, barSize.Y), Color.Black, 1);
            }

            var player = World.PlayerUnit;
            if (player != null && !unit.isPlayer)
            {
                if (!unit.isPlayerTeammate && unit.HostileTo(player))
                {
                    int level_difference = unit.level - player.level;

                    if (level_difference >= 18)
                    {
                        textColor = Color.Purple;
                    }
                    else if (level_difference >= 10)
                    {
                        textColor = Color.Red;
                    }
                    else if (level_difference >= 2)
                    {
                        textColor = Color.Orange;
                    }
                    else if (level_difference >= -6)
                    {
                        textColor = Color.Gray;
                    }
                    else
                    {
                        textColor = Color.LightSteelBlue;
                    }
                }
                else if (!unit.isPlayerTeammate && !unit.HostileTo(player))
                {
                    textColor = Color.CornflowerBlue;
                }
                else
                {
                    textColor = Color.DarkSeaGreen;
                }
            }
            else
            {
                textColor = Color.AliceBlue;
            }            

            var font = Data.Font14;
            var text = "";
            if (unit.level > 0)
            {
                text = $"{unit.name} {unit.level} lvl";                
            }
            var textSize = font.MeasureString(text);

            var textPosition = unit.position + new Vector2(-textSize.Width / 2f, -unit.radius * unit.scale.Y - textSize.Height - 4f - unit.extraHeight);

            spriteBatch.DrawString(font, text, textPosition, textColor * 0.75f);
        }
    }
}