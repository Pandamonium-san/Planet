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
    public bool Piercing { get; set; }
    public float InvulnOnHit { get; set; }
    private List<GameObject> hitObjects;

    public float damage;
    public Ship instigator;

    public Vector2 velocity;
    public Vector2 dir;
    public float speed;
    public float initialLifeTime;
    public Timer lifeTimer;

    public delegate void Pattern(Projectile p, GameTime gt);
    public Pattern pattern;
    public delegate void OnCollision(Projectile p, GameObject other);
    public OnCollision onCollision;

    public Projectile(
        World world,
        Texture2D tex,
        Vector2 pos,
        Vector2 dir,
        float speed,
        float damage = 1,
        Ship instigator = null,
        float lifeTime = 3,
        Pattern pattern = null,
        OnCollision collisionEffect = null,
        bool piercing = false)
      : base(pos, world, tex)
    {
      this.pattern = pattern;
      this.onCollision = collisionEffect;
      Scale = 1f;
      this.speed = speed;
      this.instigator = instigator;
      this.damage = damage;
      this.initialLifeTime = lifeTime;
      this.Piercing = piercing;

      if (dir != Vector2.Zero)
        dir.Normalize();
      this.dir = dir;

      lifeTimer = new Timer(lifeTime, Die);
      velocity = dir * speed;

      if (Piercing)
        hitObjects = new List<GameObject>();

      if (instigator == null)
      {
        SetLayer(Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE);
        LayerMask = Layer.PLAYER_SHIP | Layer.ENEMY_SHIP;
      }
      else if (instigator.Layer == Layer.PLAYER_SHIP)
      {
        SetLayer(Layer.PLAYER_PROJECTILE);
        LayerMask = Layer.ENEMY_SHIP;
        color = new Color(210, 230, 255);
      }
      else if (instigator.Layer == Layer.ENEMY_SHIP)
      {
        SetLayer(Layer.ENEMY_PROJECTILE);
        LayerMask = (Layer.PLAYER_SHIP);
        color = new Color(255, 210, 210);
      }
    }
    protected override void DoUpdate(GameTime gt)
    {
      lifeTimer.Update(gt);

      if (IsOutsideScreen())
        Die();

      //perform bullet pattern operation
      if (pattern != null)
        pattern(this, gt);
      else
      {
        LocalPos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        LocalRotation = Utility.Vector2ToAngle(velocity);
      }
      base.DoUpdate(gt);
    }
    private bool IsOutsideScreen()
    {
      int xMax = Game1.ScreenWidth + 300;
      int xMin = -300;
      int yMax = Game1.ScreenHeight + 300;
      int yMin = -300;

      if (Pos.X > xMax ||
          Pos.X < xMin ||
          Pos.Y > yMax ||
          Pos.Y < yMin)
        return true;
      else
        return false;
    }
    public override void DoCollision(GameObject other)
    {
      if (Piercing && hitObjects.Contains(other))
        return;

      if (other is Ship)
      {
        Ship s = (Ship)other;
        s.TakeDamage(this, InvulnOnHit);
        if (instigator.Controller is PlayerShipController)
        {
          PlayerShipController ps = (PlayerShipController)instigator.Controller;
          ps.Player.Score += (damage * 10);
          if (other.Disposed)
            ps.Player.Score += (s.maxHealth * 10);
        }
      }

      if (onCollision != null)
        onCollision(this, other);
      else
      {
        for (int i = 0; i < 3; i++)
        {
          world.Particles.CreateStar(Pos, 0.3f, -100, 100, color, 0.5f, 0.5f, 0.3f);
        }
      }

      if (!Piercing)
        Die();
      else
      {
        hitObjects.Add(other);
      }
    }
  }
}
