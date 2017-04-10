using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class BlinkerShip : Ship
  {
    public BlinkerShip(Vector2 pos, World world)
        : base(pos, world)
    {
      SetTexture("enemyBlue4");
      SetLayer(Layer.PLAYER_SHIP);
      rotationSpeed = 15;

      maxHealth = 1000;
      currentHealth = maxHealth;

      WpnDesc desc = new WpnDesc(20, 1, 700, 12, 10, 100, 0, 1, 0, 0, 0, 1);              // burst shotgun
      Weapon wpn = new Weapon(this, world, desc);
      wpn.Name = "Shotgun2";
      weapons.Add(wpn);

      desc = new WpnDesc(5, 60, 500, 8, 10, 50, 1, 30, 360/8f, 360/30f, 0, 0.2f);                 // spinny projectile thing
      wpn = new Weapon(this, world, desc);
      wpn.Name = "Spin";
      weapons.Add(wpn);
    }
    public override void Fire2()
    {
      Vector2 dir = movementDirection;
      if (dir != Vector2.Zero)
        dir.Normalize();
      else
        dir = Forward;
      Pos += dir * 100.0f;
    }
  }
}
