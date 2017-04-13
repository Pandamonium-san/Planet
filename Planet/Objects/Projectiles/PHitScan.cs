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
    private Vector2 hit;
    private bool canPierce;
    private int width;      // increases width on both sides so actual width is 2x bigger
    public float length;

    public PHitScan(World world, Vector2 start, Vector2 dir, int damage, bool canPierce, int width, float length, GameObject instigator, float lifeTime = 0.1f)
      : base(world, AssetManager.GetTexture("pixel"), start, dir, 0, damage, instigator, lifeTime)
    {
      Rotation = Utility.Vector2ToAngle(dir);
      hit = Pos + dir * length;
      this.canPierce = canPierce;
      this.width = width;
      this.length = length;
    }

    protected override void DoUpdate(GameTime gt)
    {
      lifeTimer.Update(gt);
      alpha = 1-(float)lifeTimer.Fraction;

      if (frame != 0)
        return;

      List<GameObject> objects = world.GetGameObjects();
      List<GameObject> hits = new List<GameObject>();
      foreach (GameObject go in objects)
      {
        if (!go.isDead && (layerMask & go.layer) != Layer.ZERO)
        {
          if (Utility.RayCast(Pos, Pos + dir * length, go.Pos, go.hitbox.Radius + width, ref hit))
          {
            hits.Add(go);
          }
        }
      }

      if (hits.Count == 0)
        return;

      if (canPierce)
      {
        hit = Pos + dir * length;
        foreach (GameObject h in hits)
        {
          h.DoCollision(this);
        }
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
        Utility.RayCast(Pos, Pos + dir * length, near.Pos, near.hitbox.Radius + width, ref hit); // set the hit position at nearest
        near.DoCollision(this);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (isActive)
        Utility.DrawLine(spriteBatch, Pos, hit, color*alpha, width*2);
    }
  }
}
