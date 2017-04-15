using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class PumpkinShip : Ship
  {
    public PumpkinShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture("enemyBlack1"))
    {
      origin += new Vector2(0, 2);
      //hitbox.localPos = new Vector2(0, 10);
      SetLayer(Layer.ENEMY_SHIP);
      layerDepth = 0.8f;
      //wpn = new CycloneGun(this, world);
      //WpnDesc desc = new WpnDesc(1, 3, 200, 1, 50, 50, 0, 30, 1, 0, 0, 10); //inaccurate shots
      WpnDesc desc = new WpnDesc(1, 10f, 700, 1, 0, 5, 0, 1, 0, 0, 0, 5);
      //WpnDesc desc = new WpnDesc(0, 0, 250);
      //WpnDesc desc = new WpnDesc(10, 1, 200);
      Weapon wpn = new Weapon(this, world, desc);
      //WHitScan wpn = new WHitScan(this, world, desc, 10, false);
      wpn.SetMuzzle(new Vector2(0, -10));
      weapons.Add(wpn);
      currentHealth = 100;
      baseSpeed = 100;
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
    }
  }
}
