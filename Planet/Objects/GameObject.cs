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
    // texture
    protected Texture2D tex;
    public Vector2 origin;
    public Rectangle spriteRec;
    public Color color = Color.White;
    public float alpha = 1f;
    public float layerDepth = 0f;
    // hitbox
    public Rectangle hitbox;
    public Vector2 hitboxOffset;
    public Layer layer;
    public Layer layerMask;
    // game
    protected World world;
    public int frame;
    public int framesTilDispose;
    public bool isRewindable = true;
    public bool isRewinding;
    public bool disposed;               // if true, object will be deleted at the end of the frame
    public bool isActive = true;        // determines whether or not to draw/update/collision check the object
    public bool isDead;                 // will set to dispose after a number of frames

    public TimeMachine timeMachine;
    // debug
    public bool drawHitbox;

    public GameObject(Vector2 pos, World world)
        : base(pos)
    {
      this.world = world;
      timeMachine = new TimeMachine(this);
      Scale = 1.0f;
    }

    public void Update(GameTime gt)
    {
      if (isRewinding)
      {
        timeMachine.DoRewind();
        UpdateHitboxPos();
      }
      else
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
        if (isRewindable && frame % TimeMachine.framesToSkipSaving == 0)
          timeMachine.SaveCurrentState();
        ++frame;
      }
    }
    protected virtual void DoUpdate(GameTime gt)
    {
      UpdateHitboxPos();
    }
    /// <summary>
    /// Goes x frames back in time
    /// </summary>
    public void StartRewind(int x)
    {
      if (!isRewindable)
        return;
      // save state here so objects are synced up if framesToSkipSaving != 0
      //timeMachine.SaveCurrentState();
      isRewinding = true;
      timeMachine.remainingFramesToRewind = x;
    }
    public virtual State GetState()
    {
      return new State(this);
    }
    public virtual void SetState(State data)
    {
      this.Pos = data.Pos;
      this.Rotation = data.Rotation;
      this.Scale = data.Scale;
      this.Parent = data.Parent;
      this.frame = data.frame;
      this.isDead = data.isDead;
      this.isActive = data.isActive;
    }
    protected void SetTexture(Texture2D tex)
    {
      this.tex = tex;
      spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
      origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
      CreateHitbox();
    }
    protected void SetTexture(Texture2D tex, Rectangle spriteRec)
    {
      this.tex = tex;
      this.spriteRec = spriteRec;
      origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
      CreateHitbox();
    }
    public void Die()
    {
      isDead = true;
      isActive = false;
      framesTilDispose = TimeMachine.maxRewindableFrames;
    }
    public bool IsColliding(GameObject other)
    {
      if ((layerMask & other.layer) != Layer.ZERO)
      {
        ++Game1.collisionChecksPerFrame;
        return hitbox.Intersects(other.hitbox);
      }
      return false;
    }
    public virtual void DoCollision(GameObject other)
    {

    }
    public void UpdateHitboxPos()
    {
      hitbox.X = (int)(Pos.X - origin.X + 0.5f * (spriteRec.Width * (1.0f - Scale) + hitboxOffset.X));
      hitbox.Y = (int)(Pos.Y - origin.Y + 0.5f * (spriteRec.Height * (1.0f - Scale) + hitboxOffset.Y));
    }
    public void CreateHitbox()
    {
      if (tex == null)
        return;
      hitbox = new Rectangle(
          (int)(Pos.X - origin.X + 0.5f * (spriteRec.Width * (1.0f - Scale) + hitboxOffset.X)),
          (int)(Pos.Y - origin.Y + 0.5f * (spriteRec.Height * (1.0f - Scale) + hitboxOffset.Y)),
          (int)(spriteRec.Width * Scale - hitboxOffset.X),
          (int)(spriteRec.Height * Scale - hitboxOffset.Y)
          );
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (tex != null && isActive)
      {
        spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);

        // show last possible rewind position
        State old = null;
        if (timeMachine.stateBuffer.Count > 0)
          old = ((State)(timeMachine.stateBuffer.Last.Value));
        if (old != null)
          spriteBatch.Draw(tex, old.Pos, spriteRec, Color.Red * alpha * 0.2f, old.Rotation, origin, Scale, SpriteEffects.None, layerDepth);

        // show hitboxes, may be slow
        if (drawHitbox)
          spriteBatch.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Blue * 0.5f);
      }
    }

    /// <summary>
    /// Contains info required to load to a previous state.
    /// </summary>
    public class State
    {
      public Vector2 Pos;
      public float Rotation;
      public float Scale;
      public Transform Parent;
      public Color color;
      public float alpha;
      public Rectangle hitbox;
      public int frame;
      public bool isRewindable = true;
      public bool disposed;               // if true, object will be deleted at the end of the frame
      public bool isActive = true;        // determines whether or not to draw/update/collision check the object
      public bool isDead;                 // will set to dispose after a number of frames

      public State(GameObject go)
      {
        this.Pos = go.Pos;
        this.Rotation = go.Rotation;
        this.Scale = go.Scale;
        this.Parent = go.Parent;
        this.color = go.color;
        this.alpha = go.alpha;
        this.hitbox = go.hitbox;
        this.frame = go.frame;
        this.isRewindable = go.isRewindable;
        this.disposed = go.disposed;
        this.isActive = go.isActive;
        this.isDead = go.isDead;
      }
    }
  }
}
