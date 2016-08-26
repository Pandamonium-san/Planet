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
    public PHitScan(World world, Vector2 start, Vector2 direction, int damage, GameObject instigator)
      : base(world, null, start, direction, 0, damage, instigator)
    {
      SetTexture(AssetManager.GetTexture("Fill"));
      hit = Pos + dir * 10000;
    }

    protected override void DoUpdate(GameTime gt)
    {
      if (frame != 0)
        Die();
      List<GameObject> objects = world.GetGameObjects();
      foreach (GameObject go in objects)
      {
        if (!go.isDead && (layerMask & go.layer) != Layer.ZERO)
        {
          if (Utility.RayCast(Pos, Pos + dir * 10000, go.Pos, go.hitbox.Radius, ref hit))
            go.DoCollision(this);
        }
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (isActive)
        Utility.DrawLine(spriteBatch, Pos, hit, Color.White, 2);
    }

  }
}
