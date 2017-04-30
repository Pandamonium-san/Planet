using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class EnemyShip : Ship
  {
    public EnemyShip(Vector2 pos, World world, Texture2D tex)
        : base(pos, world, tex)
    {
      SetLayer(Layer.ENEMY_SHIP);
    }
  }
  class Enemy1 : EnemyShip
  {
    public Enemy1(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy1"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy1");
      WpnDesc desc = new WpnDesc(1, 3f, 300, 1, 10, 10, 3, 9, 0, 0, 0, 5);
      Weapon wpn = new Weapon(this, world, desc, "laserRed10", "BasicGun");
      wpn.Scale = 0.4f;
      //wpn.SetMuzzle(new Vector2(0, -10));
      weapons.Add(wpn);

      desc = new WpnDesc(5, 10, 1000, 4, 1, 10, 1, 10, 3, 0, -4.5f, 3);
      wpn = new Weapon(this, world, desc, "laserBlue10", "Spinny");
      wpn.Scale = 0.4f;
      weapons.Add(wpn);

      rotationSpeed = 5;
      baseSpeed = 100;
      maxHealth = 100;
      currentHealth = maxHealth;
    }
  }
  class Enemy2 : EnemyShip
  {
    public Enemy2(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy2"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy2");
      WpnDesc desc = new WpnDesc(10, 3f, 500, 1, 2, 0, 5, 3, 0, 0, 0, 3);
      Weapon wpn = new Weapon(this, world, desc, "laserRed07", "Rifle");
      wpn.Scale = 1.0f;
      wpn.LocalPos = new Vector2(0, -20);
      weapons.Add(wpn);

      desc = new WpnDesc(30, 2.0f, 800, 1, 0, 0, 1, 3, 0, 0, 0, 3, false, true);
      wpn = new Weapon(this, world, desc, "laserBlue01", "Rifle+");
      wpn.Scale = 1.2f;
      wpn.LocalPos = new Vector2(0, -20);
      weapons.Add(wpn);

      rotationSpeed = 10;
      baseSpeed = 100;
      maxHealth = 300;
      currentHealth = maxHealth;
    }
  }
}
