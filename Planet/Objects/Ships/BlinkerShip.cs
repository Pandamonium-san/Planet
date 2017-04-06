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
      SetLayer(Layer.PLAYER_SHIP);
      rotationSpeed = 15;

      wpn = new CycloneGun(this, world);
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
      Vector2 dir = movementDirection;
      if (dir != Vector2.Zero)
        dir.Normalize();
      else
        dir = Forward;
      Pos += dir * 100.0f;
    }

    public override void Switch()
    {

    }


  }
}
