using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;

namespace BrightSpace;

public class World
{
    public static OrthographicCamera Camera;

    public static List<Unit> UnitList;

    public static List<Projectile> ProjectileList;
    private static List<Lightning> LightningList;

    private static List<ParticleEffect> ParticleList;

    private static Player player;

    private UnitScreenInfo unitInfo;

    private FramesPerSecondCounter fps;

    public World()
    {
        Camera = new OrthographicCamera(Core.GraphicsDevice);

        UnitList = [];
        ProjectileList = [];
        LightningList = [];
        ParticleList = [];

        player = new Player();

        unitInfo = new UnitScreenInfo();

        fps = new FramesPerSecondCounter();
    }

    public static Unit PlayerUnit { get { return player.playerUnit; } }

    public static void SetPlayerUnit(Unit unit)
    {
        player.SetPlayerUnit(unit);
    }

    public static Vector2 MousePosition
    {
        get { return Camera.ScreenToWorld(Input.MousePosition); }
    }

    public static void AddUnit(Unit unit)
    {
        UnitList.Add(unit);
    }

    public static void AddProjectile(Projectile projectile)
    {
        ProjectileList.Add(projectile);
    }

    public static void CreateLightning(float timeLeft, Vector2 start, Vector2 end)
    {
        var lightning = new Lightning()
        {
            timeLeft = timeLeft
        };
        lightning.Create(start, end);
        LightningList.Add(lightning);
    }

    public static void AddParticleEffect(ParticleEffect particleEffect)
    {
        ParticleList.Add(particleEffect);
    }

    public void Update(GameTime gameTime)
    {
        fps.Update(gameTime);
        if (player == null)
        {
            return;
        }

        player.Update(gameTime);

        for (int i = 0; i < UnitList.Count; i++)
        {
            var u = UnitList[i];

            player.UpdateInUnitLoop(u, gameTime);

            u.Update(gameTime);
            if (u.isDestroyed)
            {
                u.PostDeath();
                UnitList.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < ProjectileList.Count; i++)
        {
            var p = ProjectileList[i];
            p.Update(gameTime);
            if (p.isDone)
            {
                p.PostDeath();
                ProjectileList.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < LightningList.Count; i++)
        {
            LightningList[i].Update();
        }

        for (int i = 0; i < ParticleList.Count; i++)
        {
            var p = ParticleList[i];
            p.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        fps.Draw(gameTime);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: Camera.GetViewMatrix());
        foreach (var p in ProjectileList)
        {
            p.Draw(spriteBatch);
        }
        foreach (var l in LightningList)
        {
            l.Draw(spriteBatch);
        }

        foreach (var u in UnitList)
        {
            u.Draw(spriteBatch);
        }

        unitInfo.Draw(spriteBatch);        
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: Camera.GetViewMatrix());
        foreach (var p in ParticleList)
        {
            spriteBatch.Draw(p);
        }
        player.DrawMarker(spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        player.DrawUI(spriteBatch, fps);
        spriteBatch.End();
    }
}
