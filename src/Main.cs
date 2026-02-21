using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Myra;

namespace BrightSpace;

public class Main : Core
{
    private SpriteBatch spriteBatch;

    private World world;

    private UnitSelectState unitSelectState;

    public static GameState GameState;

    protected override void Initialize()
    {
        base.Initialize();

        MyraEnvironment.Game = this;

        spriteBatch = new SpriteBatch(GraphicsDevice);

        GameState = GameState.SelectState;

        world = new World();

        unitSelectState = new UnitSelectState();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        if (GameState == GameState.SelectState)
        {
            unitSelectState.Update(gameTime);
        }
        world.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        world.Draw(spriteBatch, gameTime);
        if (GameState == GameState.SelectState)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            unitSelectState.Draw(spriteBatch);
            spriteBatch.End();
        }
        
    }
}
