using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;
using MonoGame.Extended.BitmapFonts;
using Myra.Graphics2D.UI;

namespace BrightSpace;

public class UnitSelectState
{
    private bool isReady;
    private Desktop desktop;
    private CountdownTimer timer;

    public UnitSelectState()
    {
        desktop = new Desktop();
        timer = new CountdownTimer(5f);
        Build();
    }

    private void Reset()
    {
        isReady = false;
        timer.Restart();
    }

    private void Build()
    {
        isReady = true;
        desktop.Root = BuildUI();
    }

    private Widget BuildUI()
    {
        var panel = new VerticalStackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 16
        };

        var title = new Label
        {
            Text = "Select Your Faction",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        panel.Widgets.Add(title);

        UnitType unitType = UnitType.None;
        Faction faction = Faction.None;

        var buttonList = new List<TextButton>();

        foreach (Faction type in Enum.GetValues(typeof(Faction)))
        {
            if (type == Faction.None)
            {
                continue;
            }

            var button = new TextButton
            {
                Text = type.ToString(),
                Width = 250
            };

            button.Click += (s, a) =>
            {
                title.Text = "Select Your Unit Type";

                foreach (var b in buttonList)
                {
                    panel.Widgets.Remove(b);
                }
                buttonList.Clear();

                faction = type;
                foreach (UnitType type in Enum.GetValues(typeof(UnitType)))
                {
                    if (type == UnitType.None || type == UnitType.Asteroid)
                    {
                        continue;
                    }

                    var button = new TextButton
                    {
                        Text = type.ToString(),
                        Width = 250
                    };

                    button.Click += (s, a) =>
                    {
                        Reset();
                        Main.GameState = GameState.GamePlayState;
                        unitType = type;
                        World.SetPlayerUnit(Factory.CreateUnit(unitType, faction));

                        foreach (var b in buttonList)
                        {
                            panel.Widgets.Remove(b);
                        }
                        buttonList.Clear();
                        panel.Widgets.Clear();
                    };

                    buttonList.Add(button);
                    panel.Widgets.Add(button);
                }
            };

            buttonList.Add(button);
            panel.Widgets.Add(button);
        }

        return panel;
    }

    public void Update(GameTime gameTime)
    {
        timer.Update(gameTime);
        if (timer.State == TimerState.Completed && !isReady)
        {
            Build();
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!isReady)
        {
            var font = Data.Font32;
            var text = $"{timer.Interval.Seconds - timer.CurrentTime.Seconds}";
            var textSize = Data.Font14.MeasureString(text);
            var textPos = Screen.Center - (textSize / 2f);
            spriteBatch.DrawString(font, text, textPos, Color.White);
        }
        desktop.Render();
    }
}