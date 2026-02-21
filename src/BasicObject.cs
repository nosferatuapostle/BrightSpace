using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace BrightSpace;

public delegate void OnAddedEvent();
public delegate void OnRemovedEvent();

public abstract class BasicObject
{
    public OnAddedEvent OnAdded;
    public OnRemovedEvent OnRemoved;

    protected float width;
    protected float height;

    public Sprite sprite;

    public Transform2 transform;

    public BasicObject()
    {
        width = 32f;
        height = 32f;
        transform = new Transform2();
    }

    public Vector2 size
    {
        get { return new Vector2(width, height) * scale; }
        set
        {
            width = value.X / 2f;
            height = value.Y / 2f;
        }
    }

    public Vector2 position
    {
        get { return transform.Position; }
        set { transform.Position = value; }
    }

    public float rotation
    {
        get { return transform.Rotation; }
        set { transform.Rotation = value; }
    }

    public Vector2 scale
    {
        get { return transform.Scale; }
        set { transform.Scale = value; }
    }

    public virtual void PostCreate()
    {
        OnAdded?.Invoke();
    }
    
    public virtual void PostDeath()
    {
        OnRemoved?.Invoke();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch) { }
}