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
    public TimeMachine timeMachine;
    public int frame;
    public int framesTilDispose;
    public bool isRewindable { get; protected set; }
    public bool disposed { get; set; }               // if true, object will be deleted at the end of the frame
    public bool isActive { get; protected set; }               // determines whether or not to draw/update/collision check the object
    public bool isDead { get; protected set; }                 // will set to dispose after a number of frames

    // debug
    public bool drawHitbox = true;

    public GameObject(Vector2 pos, World world)
        : base(pos)
    {
      this.world = world;
      timeMachine = new TimeMachine(this);
      Scale = 4.0f;
      isRewindable = true;
      isActive = true;
    }
    public void Update(GameTime gt)
    {
      if (IsRewinding())
      {
        timeMachine.DoRewind();
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
        if (isRewindable && frame % TimeMachine.framesBetweenStates == 0)
          timeMachine.AddState(GetState());
        ++frame;
      }
    }
    protected virtual void DoUpdate(GameTime gt)
    {
    }
    /// <summary>
    /// Goes x frames back in time
    /// </summary>
    public void StartRewind(int x)
    {
      if (!isRewindable)
        return;
      timeMachine.StartRewind(x);
    }
    protected void SetTexture(Texture2D tex)
    {
      this.tex = tex;
      spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
      hitbox = new Hitbox(this, Math.Min(spriteRec.Width/2.0f, spriteRec.Height/2.0f));
    }
    protected void SetTexture(Texture2D tex, Rectangle spriteRec)
    {
      this.tex = tex;
      this.spriteRec = spriteRec;
      origin = new Vector2((float)spriteRec.Width / 2.0f, (float)spriteRec.Height / 2.0f);
      hitbox = new Hitbox(this, Math.Min(spriteRec.Width/2.0f, spriteRec.Height/2.0f));
    }
    public void Die()
    {
      if (!isDead)
      {
        isDead = true;
        isActive = false;
        framesTilDispose = TimeMachine.maxRewindableFrames;
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
        return hitbox.Collides(other.hitbox);
      }
      return false;
    }
    public bool IsRewinding()
    {
      return timeMachine.isRewinding;
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
      if (tex != null && isActive)
      {
        spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);

        /* DEBUG */
        //// show last possible rewind position
        //GOState old = null;
        //if (timeMachine.stateBuffer.Count > 0)
        //  old = ((GOState)(timeMachine.stateBuffer.Last.Value));
        //if (old != null)
        //{
        //  spriteBatch.Draw(tex, old.Pos, spriteRec, Color.Red * alpha * 0.2f, old.Rotation, origin, Scale, SpriteEffects.None, layerDepth);
        //}
        // show hitboxes
        if (drawHitbox)
          hitbox.Draw(spriteBatch);
      }
    }
    public virtual GOState GetState()
    {
      return new GOState(this);
    }
    public virtual void SetState(GOState data)
    {
      this.Pos = data.Pos;
      this.Rotation = data.Rotation;
      this.Scale = data.Scale;
      this.Parent = data.Parent;
      this.frame = data.frame;
      this.isDead = data.isDead;
      this.isActive = data.isActive;
      this.color = data.color;
      this.alpha = data.alpha;
    }
    /// <summary>
    /// Contains info required to load to a previous state.
    /// </summary>
    public class GOState
    {
      public Vector2 Pos;
      public float Rotation;
      public float Scale;
      public Transform Parent;
      public Color color;
      public float alpha;
      public int frame;
      public bool isActive;        // determines whether or not to draw/update/collision check the object
      public bool isDead;                 // will set to dispose after a number of frames

      public GOState(GameObject go)
      {
        this.Pos = go.Pos;
        this.Rotation = go.Rotation;
        this.Scale = go.Scale;
        this.Parent = go.Parent;
        this.color = go.color;
        this.alpha = go.alpha;
        this.frame = go.frame;
        this.isActive = go.isActive;
        this.isDead = go.isDead;
      }
    }
  }
}
