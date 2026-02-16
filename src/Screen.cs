using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BrightSpace
{
	public static class Screen
	{
        public static GraphicsDeviceManager GraphicsDeviceManager;
        
		internal static void Initialize(GraphicsDeviceManager graphicsManager)
        {
            GraphicsDeviceManager = graphicsManager;
        }

		public static int Width
		{
            get { return GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth; }
			set { GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = value; }
		}

		public static int Height
		{
            get { return GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight; }
			set { GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = value; }
		}

		public static Vector2 Size { get { return new Vector2(Width, Height); } }

		public static Vector2 Center { get { return new Vector2(Width / 2, Height / 2); } }

		public static int PreferredBackBufferWidth
		{
			get { return GraphicsDeviceManager.PreferredBackBufferWidth; }
			set { GraphicsDeviceManager.PreferredBackBufferWidth = value; }
		}

		public static int PreferredBackBufferHeight
		{
			get { return GraphicsDeviceManager.PreferredBackBufferHeight; }
			set { GraphicsDeviceManager.PreferredBackBufferHeight = value; }
		}

		public static int MonitorWidth { get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; } }

		public static int MonitorHeight { get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; } }

		public static SurfaceFormat BackBufferFormat { get { return GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferFormat; } }		

		public static SurfaceFormat PreferredBackBufferFormat
		{
            get { return GraphicsDeviceManager.PreferredBackBufferFormat; }
			set { GraphicsDeviceManager.PreferredBackBufferFormat = value; }
		}

		public static bool SynchronizeWithVerticalRetrace
		{
			get { return GraphicsDeviceManager.SynchronizeWithVerticalRetrace; }
			set { GraphicsDeviceManager.SynchronizeWithVerticalRetrace = value; }
		}

		public static DepthFormat PreferredDepthStencilFormat
		{
			get { return GraphicsDeviceManager.PreferredDepthStencilFormat; }
			set { GraphicsDeviceManager.PreferredDepthStencilFormat = value; }
		}

		public static bool IsFullscreen
		{
			get { return GraphicsDeviceManager.IsFullScreen; }
			set { GraphicsDeviceManager.IsFullScreen = value; }
		}

		public static DisplayOrientation SupportedOrientations
		{
			get { return GraphicsDeviceManager.SupportedOrientations; }
			set { GraphicsDeviceManager.SupportedOrientations = value; }
		}

		public static void ApplyChanges() { GraphicsDeviceManager.ApplyChanges(); }
		public static void SetSize(int width, int height)
		{
			PreferredBackBufferWidth = width;
			PreferredBackBufferHeight = height;
			ApplyChanges();
		}
	}
}