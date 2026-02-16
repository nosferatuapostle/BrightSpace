using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace BrightSpace;

internal static class Input
{
    public static KeyboardStateExtended Keyboard { get { return KeyboardExtended.GetState(); } }
    public static MouseStateExtended Mouse { get { return MouseExtended.GetState(); } }
    public static GamePadState GamePad { get { return Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One); } }

    public static Vector2 MousePosition { get { return Mouse.Position.ToVector2(); } }

    public static readonly KeyboardListener KeyboardListener;
    public static readonly MouseListener MouseListener;
    public static readonly GamePadListener GamePadListener;
    public static readonly TouchListener TouchListener;

    public static readonly bool IsMac;
    public static readonly bool IsWindows;
    public static readonly bool IsLinux;

    static Input()
    {
        KeyboardListener = new KeyboardListener();
        MouseListener = new MouseListener();
        GamePadListener = new GamePadListener();
        TouchListener = new TouchListener();

        IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;
        IsMac = Environment.OSVersion.Platform == PlatformID.MacOSX;
    }

    public static void Update(GameTime gameTime)
    {
        KeyboardExtended.Update();
        MouseExtended.Update();

        KeyboardListener.Update(gameTime);
        MouseListener.Update(gameTime);
        GamePadListener.Update(gameTime);
        TouchListener.Update(gameTime);
    }


    public static bool IsShiftDown()
    {
        return Keyboard.IsKeyDown(Keys.LeftShift) || Keyboard.IsKeyDown(Keys.RightShift);
    }


    public static bool IsAltDown()
    {
        return Keyboard.IsKeyDown(Keys.LeftAlt) || Keyboard.IsKeyDown(Keys.RightAlt);
    }


    public static bool IsCtrlDown()
    {
        if (IsMac)
        {
            return Keyboard.IsKeyDown(Keys.LeftWindows) || Keyboard.IsKeyDown(Keys.RightWindows);
        }

        return Keyboard.IsKeyDown(Keys.LeftControl) || Keyboard.IsKeyDown(Keys.RightControl);
    }
}