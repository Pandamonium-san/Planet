﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class RewinderShip : Ship
  {

    //test
    Laser laser;
    public RewinderShip(Vector2 pos, World world)
        : base(pos, world)
    {
      //SetTexture(AssetManager.GetTexture("Ship1"));
      SetTexture(AssetManager.GetTexture("Sprites"), SpriteRegions.Get("Ship1"));
      origin += new Vector2(0, 2);
      SetLayer(Layer.PLAYER_SHIP);
      layerDepth = 0.2f;
      rotationSpeed = 15;
      Weapon wpn;
      //wpn = new Weapon(this, world, WpnDesc.Spread());
      //wpn = new Weapon(this, world, WpnDesc.Circle(150));
      //wpn = new Weapon(this, world, new WpnDesc());
      //WpnDesc desc = new WpnDesc(1, 60, 700, 4, 0, 0, 0, 60*6, 90, 60*6/360, 0, 1, true); // spinny laser thing
      //WpnDesc desc = new WpnDesc(1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);                 // spinny projectile thing
      //WpnDesc desc = new WpnDesc(1, 60, 400, 1, 50, 50, 1, 30, 0, 0, 0, 10);              // burst shotgun
      WpnDesc desc = new WpnDesc(1, 60, 700, 1, 0, 0, 0, 30, 0, 0, 0, 1);
      wpn = new Weapon(this, world, desc);
      wpn.SetMuzzle(new Vector2(0, -30));
      //weapons.Add(wpn);

      WHitScan hitscan = new WHitScan(this, world, desc, 10, true);
      //hitscan.SetMuzzle(new Vector2(0, -30));
      weapons.Add(hitscan);

      Weapon wpn2;
      wpn2 = new CycloneGun(this, world); 
      weapons.Add(wpn2);

      maxHealth = 1000;
      currentHealth = maxHealth;

      //drawHitbox = false;
      //test
      laser = new Laser(Pos + Vector2.UnitY * -25, Vector2.Zero, world);
      laser.Parent = this;
    }

    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
    }
    public override void Fire1()
    {
      //test
        //if (Utility.RayCastCircle(Pos, Pos + GetDirection() * 10000, Game1.s.Pos, Game1.s.hitbox.Radius, ref Game1.intersectPoint)) ;
        weapons[0].Fire();
    }
    public override void Fire2()
    {
      world.Rewind(TimeMachine.framesBetweenStates);
      //world.Rewind(TimeMachine.maxRewindableFrames);
    }
    public override void Fire3()
    {
      weapons[1].Fire();
      //world.PostGameObj(laser);
    }
  }
}
