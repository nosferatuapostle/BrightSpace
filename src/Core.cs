using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BrightSpace;

public class Core : Game
{
    public new static ContentManager Content;
    public new static GraphicsDevice GraphicsDevice;

    public Core(int width = 800, int height = 600, string windowTitle = "Bright Space", bool isFullScreen = false, string contentDirectory = "res", bool hardwareModeSwitch = true)
    {
        Window.Title = windowTitle;
        var graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = isFullScreen,
            SynchronizeWithVerticalRetrace = false,
            HardwareModeSwitch = hardwareModeSwitch,
            PreferHalfPixelOffset = true
        };

        Screen.Initialize(graphics);

        Window.AllowUserResizing = true;
        base.Content.RootDirectory = contentDirectory;

        IsMouseVisible = true;
        IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        Content = base.Content;
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Time.Update(gameTime.GetElapsedSeconds());
        Input.Update(gameTime);
        Coroutine.Manager.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        GraphicsDevice.Clear(Color.Black);

    }
    
    internal class WaitForSeconds
    {
        internal static WaitForSeconds waiter = new();
        internal float waitTime;

        internal WaitForSeconds Wait(float seconds)
        {
            waiter.waitTime = seconds;
            return waiter;
        }
    }
}
