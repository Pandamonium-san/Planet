using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class Missile : Projectile
  {
    public Missile(World world, Texture2D tex, Vector2 pos, Vector2 dir, float speed, int damage = 1, Ship instigator = null, float inaccuracy = 0, float lifeTime = 3)
      : base(world, tex, pos, dir, speed, damage, instigator, lifeTime)
    {
      if (instigator == null)
      {
        SetLayer(Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE);
        LayerMask = ~Layer.ZERO;
      }
      else if (instigator.Layer == Layer.PLAYER_SHIP)
      {
        SetLayer(Layer.PLAYER_PROJECTILE);
        LayerMask = (Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE);
      }
      else if (instigator.Layer == Layer.ENEMY_SHIP)
      {
        SetLayer(Layer.ENEMY_PROJECTILE);
        LayerMask = (Layer.PLAYER_SHIP | Layer.PLAYER_PROJECTILE);
      }
    }


  }
}
