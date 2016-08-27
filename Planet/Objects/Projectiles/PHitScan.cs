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
    public PHitScan(World world, Vector2 start, Vector2 direction, int damage, bool canPierce, int width, GameObject instigator)
      : base(world, null, start, direction, 0, damage, instigator)
    {
      SetTexture(AssetManager.GetTexture("Fill"));
      hit = Pos + dir * 10000;
      this.canPierce = canPierce;
      this.width = width;
    }

    protected override void DoUpdate(GameTime gt)
    {
      if (frame != 0)
        Die();
      List<GameObject> objects = world.GetGameObjects();
      List<GameObject> hits = new List<GameObject>();
      foreach (GameObject go in objects)
      {
        if (!go.isDead && (layerMask & go.layer) != Layer.ZERO)
        {
          if (Utility.RayCast(Pos, Pos + dir * 10000, go.Pos, go.hitbox.Radius + width, ref hit))
          {
            hits.Add(go);
          }
        }
      }

      if (hits.Count == 0)
        return;

      if (canPierce)
      {
        hit = Pos + dir * 10000;
        foreach (GameObject hit in hits)
        {
          hit.DoCollision(this);
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
        Utility.RayCast(Pos, Pos + dir * 10000, near.Pos, near.hitbox.Radius + width, ref hit);
        near.DoCollision(this);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (isActive)
        Utility.DrawLine(spriteBatch, Pos, hit, Color.White, width*2);
    }

  }
}
