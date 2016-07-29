using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class BlinkerShip : Ship
  {
    Weapon wpn;
    public BlinkerShip(Vector2 pos, World world)
        : base(pos, world)
    {
      SetTexture(AssetManager.GetTexture("Parasite"));
      layer = Layer.PLAYER_SHIP;
      rotationSpeed = 15;

      //wpn = new Weapon(this, WpnDesc.Spread());
      wpn = new Weapon(this, world, WpnDesc.Circle(200));
      wpn = new CycloneGun(this, world);
      wpn.projSpeed = 200;
      wpn.projLifeTime = 5;
      wpn.shotsPerSecond = 10;
      //wpn.magSize = 5;
      wpn.magReloadTime = 0;
      wpn.inaccuracy = 0;
      wpn.speedVariance = 0;
    }
    protected override void DoUpdate(GameTime gt)
    {
      wpn.Update(gt);
      base.DoUpdate(gt);
    }
    public override void Fire1()
    {
      wpn.Fire();
    }

    public override void Fire2()
    {
      Vector2 dir = currentVelocity;
      if (dir != Vector2.Zero)
        dir.Normalize();
      else
        dir = GetDirection();
      Pos += dir * 100.0f;
    }

    public override void Fire3()
    {

    }


  }
}
