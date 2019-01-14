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
    public Vector2 Right { get { return Utility.AngleToVector2(Rotation + (float)Math.PI / 2); } }
    // collision
    public Hitbox Hitbox { get; set; }
    public Layer Layer { get; private set; }
    public Layer LayerMask { get; set; }
    // game
    public int frame;
    public bool Disposed { get; set; }               // if true, object will be deleted at the end of the frame
    public bool IsActive { get; set; }               // determines whether or not to draw/update/collision check the object
    public bool CollisionEnabled { get; set; }
    public bool Untargetable { get; set; }

    protected World world;
    // debug
    protected bool drawHitbox = false;

    public GameObject(Vector2 pos, World world, Texture2D tex)
      : base(pos, tex)
    {
      this.world = world;
      Scale = .75f;
      IsActive = true;
      CollisionEnabled = true;
    }
    public virtual void Update(GameTime gt)
    {
      if (IsActive)
      {
        DoUpdate(gt);
      }
      ++frame;
    }
    protected virtual void DoUpdate(GameTime gt)
    {
    }
    protected override void SetTexture(Texture2D tex, Rectangle? sourceRec = null)
    {
      base.SetTexture(tex, sourceRec);
      Hitbox = new Hitbox(this, Math.Min(spriteRec.Width / 2.0f, spriteRec.Height / 2.0f));
    }
    public virtual void Die()
    {
      Disposed = true;
      IsActive = false;
      if (Parent != null)
        Parent.RemoveChild(this);
    }
    public virtual void DoCollision(GameObject other)
    {

    }
    public bool IsColliding(GameObject other)
    {
      if ((LayerMask & other.Layer) != Layer.ZERO)
      {
        ++Game1.collisionChecksPerFrame;
        return Hitbox.Colliding(other.Hitbox);
      }
      return false;
    }
    public void SetLayer(Layer layer)
    {
      this.Layer = layer;

      switch (layer)
      {
        case Layer.ZERO:
          layerDepth = 0.5f;
          break;
        case Layer.PLAYER_SHIP:
          layerDepth = 0.1f;
          LayerMask = Layer.PLAYER_SHIP | Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE;
          break;
        case Layer.PLAYER_PROJECTILE:
          layerDepth = 0.4f;
          LayerMask = Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE;
          break;
        case Layer.ENEMY_SHIP:
          layerDepth = 0.2f;
          LayerMask = Layer.PLAYER_SHIP | Layer.ENEMY_SHIP | Layer.PLAYER_PROJECTILE;
          break;
        case Layer.ENEMY_PROJECTILE:
          layerDepth = 0.3f;
          LayerMask = Layer.PLAYER_SHIP | Layer.PLAYER_PROJECTILE;
          break;
        default:
          break;
      }
    }
    public override void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      if (tex != null && IsActive)
      {
        base.Draw(spriteBatch, a);

        if (drawHitbox)
          Hitbox.Draw(spriteBatch);
      }
    }
  }
}
