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
      //SetTexture(AssetManager.GetTexture("pumpkin"));
      SetTexture(AssetManager.GetTexture("Sprites"), SpriteRegions.Get("Drone1"));
      origin += new Vector2(0, 2);
      //hitbox.localPos = new Vector2(0, 10);
      SetLayer(Layer.ENEMY_SHIP);
      layerDepth = 0.8f;
      //wpn = new CycloneGun(this, world);
      WpnDesc desc = new WpnDesc(1, 10, 400, 1, 50, 50, 0, 30, 1, 0, 0, 10);
      Weapon wpn = new Weapon(this, world, desc);
      wpn.SetMuzzle(new Vector2(0, -10));
      weapons.Add(wpn);
      currentHealth = 100;
      baseSpeed = 100;
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
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
