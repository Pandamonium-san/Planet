using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  abstract class EnemyShip : Ship
  {
    public static float GlobalEnemyDamageModifier = 1.0f;
    public static float GlobalEnemyHealthModifier = 1.0f;
    public EnemyShip(Vector2 pos, World world, Texture2D tex)
        : base(pos, world, tex)
    {
      SetLayer(Layer.ENEMY_SHIP);
      incomingDamageModifier = 1/GlobalEnemyHealthModifier;
      damageModifier = GlobalEnemyDamageModifier;
    }
  }
  class Enemy1 : EnemyShip
  {
    public Enemy1(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy1"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy1");
      WpnDesc desc = new WpnDesc(5, 3f, 300, 1, 10, 10, 3, 9, 0, 0, 0, 5);
      Weapon wpn = new Weapon(this, world, desc, "laserRed10", "Pew");
      wpn.Scale = 0.4f;
      //wpn.SetMuzzle(new Vector2(0, -10));
      weapons.Add(wpn);

      desc = new WpnDesc(1.5f, 10, 1000, 4, 1, 10, 1, 10, 3, 0, -4.5f, 3);
      wpn = new Weapon(this, world, desc, "laserBlue10", "Torrent");
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

      desc = new WpnDesc(6, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      //desc = new WpnDesc(6, 2.0f, 800, 1, 0, 0, 1, 3, 0, 0, 0, 3, false, true);   //piercing rifle
      wpn = new Weapon(this, world, desc, "laserBlue16", "Rifle+");
      wpn.Scale = 1.2f;
      wpn.LocalPos = new Vector2(0, -20);
      weapons.Add(wpn);

      rotationSpeed = 10;
      baseSpeed = 100;
      maxHealth = 300;
      currentHealth = maxHealth;
    }
  }
  class Enemy3 : EnemyShip
  {
    float rotation = 0.0f;

    public Enemy3(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\ufoRed"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\ufoRed");

      WpnDesc desc = new WpnDesc(8, 2f, 250, 4, 2, 0, 5, 6, 360 / 4, 360 / 4 / 6, 0, 10, true);
      Weapon wpn = new Weapon(this, world, desc, "laserRed08", "4way");
      wpn.Scale = 0.2f;
      weapons.Add(wpn);

      desc = new WpnDesc(2, 40f, 500, 4, 0, 0, 1.0f, 40, 360 / 4, 360 / 4 / 7, 0, 0.4f, true);
      wpn = new Weapon(this, world, desc, "laserBlue08", "Spinny");
      wpn.Scale = 0.2f;
      weapons.Add(wpn);

      rotationSpeed = 4.0f;
      baseSpeed = 100;
      maxHealth = 150;
      currentHealth = maxHealth;
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
      rotation += rotationSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation = rotation;
    }
  }
  class Enemy4 : EnemyShip
  {
    public Enemy4(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy4"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\ufoRed");

      WpnDesc desc = new WpnDesc(25, 0.3f, 200, 1, 0, 0, 4, 1, 0, 0, 0, 15, false, true);
      Weapon wpn = new Weapon(this, world, desc, "laserRed08", "BFG");
      wpn.Scale = 2.0f;
      weapons.Add(wpn);

      desc = new WpnDesc(0.2f, 60f, 500, 3, 0, 0, 0, 1, 5, 0, -5, 0.05f, false, false);
      wpn = new LaserGun(this, world, desc, 8, true);
      wpn.Name = "Scatter Laser";
      weapons.Add(wpn);

      rotationSpeed = 5;
      baseSpeed = 70;
      maxHealth = 500;
      currentHealth = maxHealth;
      maxShield = 50;
      currentShield = maxShield;
    }
  }
  class EnemyBoss : EnemyShip
  {
    public EnemyBoss(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\spaceShips_007"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_007");

      WpnDesc desc = new WpnDesc(10, 8f, 500, 10, 5, 50, 2, 8, 180 / 9, 0, -180 / 9 * 10 / 2, 10, false, false);
      Weapon wpn = new Weapon(this, world, desc, "laserRed08", "Spread");
      wpn.Scale = 0.6f;
      weapons.Add(wpn);

      desc = new WpnDesc(5, 10f, 500, 4, 2, 10, 2.0f, 20, 360 / 4, 360 / 4 / 10, 0, 5f, true);
      wpn = new Weapon(this, world, desc, "laserRed08", "Spinny");
      wpn.Scale = 0.4f;
      CompoundWeapon cw1 = new CompoundWeapon(wpn);

      desc = new WpnDesc(10, 5f, 500, 4, 5, 20, 2.0f, 10, 360 / 4, 360 / 4 / 5, -45, 5f, true);
      wpn = new Weapon(this, world, desc, "laserRed10", "Spinny");
      wpn.Scale = 0.6f;
      cw1.AddWeapon(wpn);
      weapons.Add(cw1);

      desc = new WpnDesc(20, 2.0f, 400, 3, 5, 50, 4, 12, 45 / 3, 0, -45 / 3, 15, false, true);
      wpn = new Weapon(this, world, desc, "laserRed10", "BFG");
      wpn.Scale = 1.5f;
      CompoundWeapon cw = new CompoundWeapon(wpn);

      desc = new WpnDesc(30, 60.0f, 500, 2, 0, 0, 4, 360, 45, 0, -22.5f, 0.1f, false, true);
      wpn = new LaserGun(this, world, desc, 30, true, 10000, true);
      wpn.Name = "X Laser front";
      cw.AddWeapon(wpn);
      desc = new WpnDesc(30, 60.0f, 500, 2, 0, 0, 4, 360, 45, 0, -22.5f + 180, 0.1f, false, true);
      wpn = new LaserGun(this, world, desc, 30, true, 10000, true);
      wpn.Name = "X Laser back";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(5, 6.0f, 400, 1, 5, 10, 4, 36, 0, 0, -10, 5);
      wpn = new Weapon(this, world, desc, "laserRed08", "Pew");
      wpn.Scale = 0.4f;
      wpn.LocalPos = new Vector2(-15, 0);
      wpn.Name = "Left pew";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(5, 6.0f, 400, 1, 5, 10, 4, 36, 0, 0, 10, 5);
      wpn = new Weapon(this, world, desc, "laserRed08", "Pew");
      wpn.Scale = 0.4f;
      wpn.LocalPos = new Vector2(15, 0);
      wpn.Name = "Right pew";
      cw.AddWeapon(wpn);

      desc = new WpnDesc(20, 16f, 750, 15, 10, 0, 4, 6 * 16, 315 / 15, 0, 22.5f + 21/2, 10, false, false);
      wpn = new Weapon(this, world, desc, "laserRed08", "DeathZone");
      wpn.Scale = 0.7f;
      cw.AddWeapon(wpn);

      weapons.Add(cw);
      cw.SetShip(this);

      rotationSpeed = 10;
      baseSpeed = 350;
      maxHealth = 5000;
      currentHealth = maxHealth;
      maxShield = 250;
      currentShield = maxShield;
    }
  }
}
