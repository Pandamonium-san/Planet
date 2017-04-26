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
    public Vector2 hit;
    public float length;
    public bool stickToInstigator;

    private bool fired;
    private bool canPierce;
    private int width;      // increases width on both sides so actual width is 2x bigger

    public PHitScan(World world, Texture2D tex, Vector2 start, Vector2 dir, float damage, bool canPierce, int width, float length, Ship instigator, float lifeTime = 0.1f)
      : base(world, tex, start, dir, 0, damage, instigator, lifeTime)
    {
      Rotation = Utility.Vector2ToAngle(dir);
      hit = Pos + dir * length;
      this.canPierce = canPierce;
      this.width = width;
      this.length = length;
      stickToInstigator = true;
    }
    void RayCast()
    {
      fired = true;
      List<GameObject> objects = world.GetGameObjects();
      List<GameObject> hits = new List<GameObject>();
      foreach (GameObject go in objects)
      {
        if (!go.IsActive || !go.CollisionEnabled || (LayerMask & go.Layer) == Layer.ZERO)
          continue;

        if (Utility.RayCast(Pos, Pos + dir * length, go.Pos, go.Hitbox.Radius + width / 2f, ref hit))
        {
          if (canPierce)
          {
            go.DoCollision(this);
            DoCollision(go);
          }
          hits.Add(go);
        }
      }

      if (hits.Count == 0)
        return;

      if (canPierce)
      {
        hit = Pos + dir * length;
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
        Utility.RayCast(Pos, Pos + dir * length, near.Pos, near.Hitbox.Radius + width / 2f, ref hit); // set the hit position at nearest
        near.DoCollision(this);
        DoCollision(near);
      }
    }
    public override void DoCollision(GameObject other)
    {
      if (onCollision != null)
        onCollision(this, other);
      else
      {
        for (int i = 0; i < 3; i++)
        {
          world.Particles.CreateHitEffect(Pos, 0.3f, -100, 100, color, 0.5f, 0.5f, 0.3f);
        }
      }
    }
    protected override void DoUpdate(GameTime gt)
    {
      if (frame == 0)
        RayCast();
      lifeTimer.Update(gt);
      alpha = 1 - (float)lifeTimer.Fraction;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!fired)
        return;
      Vector2 start = Pos;
      if (stickToInstigator)
        start = instigator.Pos;
      Vector2 end = hit;
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
        1);
    }
  }
}
