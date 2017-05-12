using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Parasite : Projectile
  {
    public bool Latched { get { return latchedShip != null; } }

    private PossessorShip ship;
    private List<Sprite> parasiteLink;
    private double extensionTime;
    private float pullForce;
    private float leashRange;
    private Ship latchedShip;

    public Parasite(
     PossessorShip instigator,
     World world,
     Texture2D tex,
     Vector2 pos,
     Vector2 dir,
     float speed,
     double extensionTime,
     float pullForce,
     float leashRange,
     float damagePerSecond,
     float lifeTime)
      : base(world, tex, pos, dir, speed, damagePerSecond, instigator, lifeTime, null, null, false)
    {
      this.extensionTime = extensionTime;
      this.pullForce = pullForce;
      this.leashRange = leashRange;
      ship = instigator;
      parasiteLink = new List<Sprite>();
      for (int i = 0; i < 15; i++)
      {
        Sprite link = new Sprite(Pos, AssetManager.GetTexture("proj1"));
        link.layerDepth = layerDepth + 0.01f;
        parasiteLink.Add(link);
      }
    }
    protected override void DoUpdate(GameTime gt)
    {
      lifeTimer.Update(gt);
      if (latchedShip != null)
      {
        if (!(latchedShip.Controller is AIController))
        {
          Unlatch();
          return;
        }
        Pos = latchedShip.Pos;
        PullShip(latchedShip, ship, 0.2f);
        PullShip(ship, latchedShip, 0.1f);

        if (latchedShip.currentHealth < 20)
        {
          ((AIController)latchedShip.Controller).IsActive = false;
          latchedShip.IsActive = true;
          latchedShip.CollisionEnabled = false;
          ship.CollisionEnabled = false;
          if (latchedShip.Disposed)
          {
            latchedShip.Disposed = false;
            world.PostGameObj(latchedShip);
          }
          PullShip(ship, latchedShip);
          if (Vector2.Distance(ship.Pos, latchedShip.Pos) < 10)
          {
            latchedShip.CollisionEnabled = true;
            ship.TakeOver(latchedShip);
            Unlatch();
            Die();
          }
        }
        else if (frame % 20 == 0)
          latchedShip.TakeDamage(damage / 3);
        if (Vector2.Distance(ship.Pos, Pos) > leashRange)
          Unlatch();
      }
      else
      {
        if (lifeTimer.elapsedSeconds > extensionTime)
        {
          Vector2 d = Vector2.Normalize(ship.Pos - Pos);
          LocalPos += 2.0f * d * speed * (float)gt.ElapsedGameTime.TotalSeconds;
          if (Vector2.Distance(Pos, ship.Pos) < 10)
            Die();
        }
        else
          LocalPos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        LocalRotation = Utility.Vector2ToAngle(Pos - ship.Pos);
      }

      UpdateLink();
    }
    private void PullShip(Ship target, Ship to, float multiplier = 1.0f)
    {
      Vector2 pull = Vector2.Normalize(to.Pos - target.Pos) * pullForce * multiplier;
      target.SetAcceleration(pull);
    }
    private void UpdateLink()
    {
      Vector2 distance;
      if (latchedShip == null)
        distance = Pos - ship.Pos;
      else
        distance = latchedShip.Pos - ship.Pos;
      for (int i = 0; i < parasiteLink.Count(); i++)
        parasiteLink[i].Pos = ship.Pos + distance / parasiteLink.Count() * i;
    }
    public void Latch(Ship ship)
    {
      latchedShip = ship;
      CollisionEnabled = false;
      Visible = false;
      Pos = ship.Pos;
      this.ship.speedModifier *= 0.9f;
      AudioManager.PlaySound("parasite3", 0.40f);
    }
    public void Unlatch()
    {
      extensionTime = 0;
      latchedShip = null;
      Visible = true;
      this.ship.speedModifier /= 0.9f;
      AudioManager.PlaySound("parasite3", 0.40f);
    }
    public override void DoCollision(GameObject other)
    {
      if (other.Layer != Layer.ENEMY_SHIP || ship.PossessedShip != null)
        return;
      Latch((Ship)other);
    }
    public override void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      base.Draw(spriteBatch, a);
      foreach (Sprite sp in parasiteLink)
        sp.Draw(spriteBatch, a);
    }
  }
}
