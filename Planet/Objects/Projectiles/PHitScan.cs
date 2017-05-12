using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class PHitScan : Projectile
  {
    public Vector2 hitPos;
    public float length;
    public bool stick;

    private Weapon wpn;
    private int width;      // increases width on both sides so actual width is 2x bigger

    public PHitScan(World world, Texture2D tex, Vector2 start, Vector2 dir, float damage, bool canPierce, int width, float length, Ship instigator, Weapon wpn, float lifeTime = 0.1f)
      : base(world, tex, start, dir, 0, damage, instigator, lifeTime, null, null, canPierce)
    {
      this.wpn = wpn;
      Rotation = Utility.Vector2ToAngle(dir);
      hitPos = Pos + dir * length;
      this.width = width;
      this.length = length;
      stick = true;
      CollisionEnabled = false;
    }
    void RayCast()
    {
      List<GameObject> objects = world.GetGameObjects();
      List<GameObject> hits = new List<GameObject>();
      foreach (GameObject go in objects)
      {
        if (!go.IsActive || !go.CollisionEnabled || (LayerMask & go.Layer) == Layer.ZERO)
          continue;

        if (Utility.RayCast(Pos, Pos + dir * length, go.Pos, go.Hitbox.Radius + width / 2f, ref hitPos))
        {
          if (Piercing)
          {
            go.DoCollision(this);
            DoCollision(go);
          }
          hits.Add(go);
        }
      }

      if (hits.Count == 0)
        return;

      if (Piercing)
      {
        hitPos = Pos + dir * length;
      }
      // selects the closest target that was hit, may be inaccurate if hitboxes have different sizes
      else
      {
        GameObject near = null;
        float nDistance = 999999;
        foreach (GameObject go in hits)
        {
          if (near == null)
            near = go;
          float distance = Utility.DistanceSquared(Pos, go.Pos);
          if (distance < nDistance)
          {
            near = go;
            nDistance = distance;
          }
        }
        Utility.RayCast(Pos, Pos + dir * length, near.Pos, near.Hitbox.Radius + width / 2f, ref hitPos); // set the hit position at nearest
        near.DoCollision(this);
        DoCollision(near);
      }
    }
    protected override void DoUpdate(GameTime gt)
    {
      if (frame == 0)
        RayCast();
      lifeTimer.Update(gt);
      alpha = 1 - (float)lifeTimer.Fraction;
    }
    public override void DoCollision(GameObject other)
    {
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
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      Vector2 start = Pos;
      if (stick)
        start = wpn.Pos;
      Vector2 end = hitPos;
      Vector2 edge = end - start;
      float angle = (float)-Math.Atan2(edge.X, edge.Y);
      spriteBatch.Draw(
        tex,
        new Rectangle((int)start.X, (int)start.Y, width, (int)edge.Length()),
        null,
        color * alpha,
        angle,
        new Vector2(tex.Width / 2f, 0),
        SpriteEffects.None,
        layerDepth);
    }
  }
}
