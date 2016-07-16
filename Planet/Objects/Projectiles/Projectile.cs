using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Projectile : GameObject
  {
    public float damage;
    public GameObject instigator;

    public Vector2 dir;
    public Vector2 velocity;
    public float speed;
    public float maxLifeTime;
    public float currentLifeTime;

    public delegate void Pattern(Projectile p, GameTime gt);
    Pattern pattern;

    public Projectile(
        World world,
        Texture2D tex,
        Vector2 pos,
        Vector2 dir,
        float speed,
        float damage = 1,
        GameObject instigator = null,
        float lifeTime = 3,
        Pattern pattern = null)
        : base(pos, world)
    {
      this.pattern = pattern;

      if (tex != null)
        this.SetTexture(tex);
      this.speed = speed;
      this.instigator = instigator;
      this.damage = damage;

      if (dir != Vector2.Zero)
        dir.Normalize();
      this.dir = dir;

      maxLifeTime = lifeTime;
      currentLifeTime = lifeTime;
      velocity = dir * speed;

      if (instigator == null)
      {
        layer = Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE;
        layerMask = ~Layer.ZERO;
      }
      else if (instigator.layer == Layer.PLAYER_SHIP)
      {
        layer = Layer.PLAYER_PROJECTILE;
        layerMask = (Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE);
      }
      else if (instigator.layer == Layer.ENEMY_SHIP)
      {
        layer = Layer.ENEMY_PROJECTILE;
        layerMask = (Layer.PLAYER_SHIP | Layer.PLAYER_PROJECTILE);
      }
    }

    protected override void DoUpdate(GameTime gt)
    {
      currentLifeTime -= (float)gt.ElapsedGameTime.TotalSeconds;
      if (currentLifeTime <= 0 && !isDead)
      {
        Die();
      }

      if (IsOutsideScreen())
        Die();

      //perform bullet pattern operation
      if (pattern != null)
        pattern(this, gt);
      else
        Pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;

      base.DoUpdate(gt);
    }

    private bool IsOutsideScreen()
    {
      int xMax = Game1.ScreenWidth + 100;
      int xMin = -100;
      int yMax = Game1.ScreenHeight + 100;
      int yMin = -100;

      if (Pos.X > xMax ||
          Pos.X < xMin ||
          Pos.Y > yMax ||
          Pos.Y < yMin)
        return true;
      else
        return false;
    }

    public override State GetState()
    {
      return new ProjState(this);
    }

    public override void SetState(State data)
    {
      base.SetState(data);
      ProjState p = (ProjState)data;
      this.dir = p.dir;
      this.velocity = p.velocity;
      this.speed = p.speed;
      this.currentLifeTime = p.currentLifeTime;
    }

    protected class ProjState : State
    {
      public Vector2 dir;
      public Vector2 velocity;
      public float speed;
      public float currentLifeTime;

      public ProjState(Projectile p)
          : base(p)
      {
        this.dir = p.dir;
        this.velocity = p.velocity;
        this.speed = p.speed;
        this.currentLifeTime = p.currentLifeTime;
      }
    }
  }
}
