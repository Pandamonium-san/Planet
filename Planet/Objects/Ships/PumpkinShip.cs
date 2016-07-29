using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class PumpkinShip : Ship
  {
    public PumpkinShip(Vector2 pos, World world)
        : base(pos, world)
    {
      SetTexture(AssetManager.GetTexture("pumpkin"));
      //SetTexture(AssetManager.GetTexture("Sprites"), new Rectangle(10, 0, 15, 13));
      layer = Layer.ENEMY_SHIP;
      layerDepth = 1f;
      //wpn = new CycloneGun(this, world);
      Weapon wpn = new Weapon(this, world, 1, 1, 400, 1, 50, 50, 0, 30, 1, 0, 0, 10);
      weapons.Add(wpn);
      currentHealth = 100;
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
    }
    protected override void CalculateCurrentRotation()
    {
      //currentRotationSpeed = 1;
      //Rotation = MathHelper.WrapAngle(Rotation);
    }
    public override void Fire1()
    {
       weapons[0].Fire();
    }

    public override void Fire2()
    {

    }
    public override void Fire3()
    {

    }

  }
}
