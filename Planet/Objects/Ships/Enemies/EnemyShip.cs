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

    public float Cost { get { return baseCost; } }
    protected float baseCost;
    public EnemyShip(Vector2 pos, World world, Texture2D tex)
        : base(pos, world, tex)
    {
      SetLayer(Layer.ENEMY_SHIP);
      incomingDamageModifier = 1 / GlobalEnemyHealthModifier;
      damageModifier = GlobalEnemyDamageModifier;
    }
  }
  class Enemy1 : EnemyShip
  {
    public Enemy1(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy1"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy1");

      baseCost = 100;
      rotationSpeed = 5;
      baseSpeed = 200;
      maxHealth = 150;
      currentHealth = maxHealth;

      weapons.Add(WeaponList.Pew(this, world));
      weapons.Add(WeaponList.Torrent(this, world));
    }
  }
  class Enemy2 : EnemyShip
  {
    public Enemy2(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy2"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy2");

      baseCost = 300;
      rotationSpeed = 10;
      baseSpeed = 200;
      maxHealth = 350;
      currentHealth = maxHealth;

      weapons.Add(WeaponList.Split(this, world));
      weapons.Add(WeaponList.Sniper(this, world));
    }
  }
  class Enemy3 : EnemyShip
  {
    public bool AutoRotate { get; set; }
    float rotation = 0.0f;

    public Enemy3(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\ufoRed"))
    {
      baseCost = 200;
      flashTex = AssetManager.GetTexture(@"ships\flash\ufoRed");

      AutoRotate = true;
      rotationSpeed = 4.0f;
      baseSpeed = 200;
      maxHealth = 150;
      maxShield = 25;
      currentShield = maxShield;
      currentHealth = maxHealth;

      weapons.Add(WeaponList.FourWay(this, world));
      weapons.Add(WeaponList.Spinny(this, world));
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
      if (!AutoRotate)
        return;
      rotation += rotationSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation = rotation;
    }
  }
  class Enemy4 : EnemyShip
  {
    public Enemy4(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\enemy4"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\enemy4");

      baseCost = 350;
      rotationSpeed = 5;
      baseSpeed = 150;
      maxHealth = 700;
      currentHealth = maxHealth;
      maxShield = 50;
      currentShield = maxShield;

      weapons.Add(WeaponList.BFG(this, world));
      weapons.Add(WeaponList.Scatter(this, world));
    }
  }
  class EnemyBoss : EnemyShip
  {
    public EnemyBoss(Vector2 pos, World world)
      : base(pos, world, AssetManager.GetTexture(@"ships\red\spaceShips_007"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_007");

      baseCost = 5000;
      rotationSpeed = 10;
      baseSpeed = 350;
      maxHealth = 4000;
      currentHealth = maxHealth;
      maxShield = 250;
      currentShield = maxShield;

      weapons.Add(WeaponList.Spread(this, world));
      weapons.Add(WeaponList.Spinny2(this, world));
      weapons.Add(WeaponList.XLaser(this, world));
      weapons.Add(WeaponList.Nova(this, world));
    }
  }
}
