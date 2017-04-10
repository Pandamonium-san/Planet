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

  public class GameObject : Sprite
  {
    public Vector2 Forward { get { return Utility.AngleToVector2(Rotation); } }

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
    // debug
    protected bool drawHitbox = false;

    public GameObject(Vector2 pos, World world)
      : base(pos)
    {
      this.world = world;
      Scale = .5f;
      isActive = true;
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
    protected override void SetTexture(string tName, Rectangle? sourceRec = null)
    {
      base.SetTexture(tName, sourceRec);
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
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (tex != null && isActive)
      {
        base.Draw(spriteBatch);

        if (drawHitbox)
          hitbox.Draw(spriteBatch);
      }
    }
  }
}
