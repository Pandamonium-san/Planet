using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  public enum Layer
  {
    ZERO = 0x00000000,
    PLAYER_SHIP = 0x00000001,
    PLAYER_PROJECTILE = 0x00000002,
    ENEMY_SHIP = 0x00000004,
    ENEMY_PROJECTILE = 0x00000008
  }

  public class GameObject : Transform
  {
    public Vector2 Forward { get { return Utility.AngleToVector2(Rotation); } }

    // drawing
    protected Texture2D tex;
    public Vector2 origin;
    public Rectangle spriteRec;
    public Color color = Color.White;
    public float alpha = 1f;
    public float layerDepth = 0f;
    // collision
    public Hitbox hitbox;
    public Layer layer { get; private set; }
    public Layer layerMask;
    // game
    protected World world;
    public int frame;
    public int framesTilDispose;
    public bool disposed { get; set; }               // if true, object will be deleted at the end of the frame
    public bool isActive { get; protected set; }               // determines whether or not to draw/update/collision check the object
    public bool isDead { get; protected set; }                 // will set to dispose after a number of frames
    public bool Visible { get; set; }
    // debug
    protected bool drawHitbox = false;

    public GameObject(Vector2 pos, World world)
      : base(pos)
    {
      this.world = world;
      Scale = 4.0f;
      isActive = true;
      Visible = true;
    }
    public void Update(GameTime gt)
    {
      if (isDead)
      {
        framesTilDispose--;
        if (framesTilDispose <= 0)
          disposed = true;
      }
      if (isActive)
      {
        DoUpdate(gt);
      }
      ++frame;
    }
    protected virtual void DoUpdate(GameTime gt)
    {
    }
    protected void SetTexture(Texture2D tex)
    {
      this.tex = tex;
      spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
      hitbox = new Hitbox(this, Math.Min(spriteRec.Width / 2.0f, spriteRec.Height / 2.0f));
    }
    protected void SetTexture(Texture2D tex, Rectangle spriteRec)
    {
      this.tex = tex;
      this.spriteRec = spriteRec;
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
      hitbox = new Hitbox(this, Math.Min(spriteRec.Width / 2.0f, spriteRec.Height / 2.0f));
    }
    public virtual void Die()
    {
      if (!isDead)
      {
        isDead = true;
        isActive = false;
      }
    }
    public virtual void DoCollision(GameObject other)
    {

    }
    public bool IsColliding(GameObject other)
    {
      if ((layerMask & other.layer) != Layer.ZERO)
      {
        ++Game1.collisionChecksPerFrame;
        return hitbox.Colliding(other.hitbox);
      }
      return false;
    }
    public void SetLayer(Layer layer)
    {
      this.layer = layer;

      switch (layer)
      {
        case Layer.ZERO:
          break;
        case Layer.PLAYER_SHIP:
          layerDepth = 0.1f;
          break;
        case Layer.PLAYER_PROJECTILE:
          layerDepth = 0.4f;
          break;
        case Layer.ENEMY_SHIP:
          layerDepth = 0.3f;
          break;
        case Layer.ENEMY_PROJECTILE:
          layerDepth = 0.2f;
          break;
        default:
          break;
      }
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (!Visible)
        return;
      if (tex != null && isActive)
      {
        spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);

        // show hitboxes
        if (drawHitbox)
          hitbox.Draw(spriteBatch);
      }
    }
  }
}
