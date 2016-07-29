using Microsoft.Xna.Framework;
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
      SetTexture(AssetManager.GetTexture("Ship1"));
      //SetTexture(AssetManager.GetTexture("Sprites"), new Rectangle(0, 0, 9, 10));
      SetTexture(AssetManager.GetTexture("Sprites"), new Rectangle(1, 11, 7, 11));
      //SetTexture(AssetManager.GetTexture("Sprites"), new Rectangle(10, 0, 15, 13));

      layer = Layer.PLAYER_SHIP;
      layerDepth = 0f;
      rotationSpeed = 15;
      Weapon wpn;
      //wpn = new Weapon(this, world, WpnDesc.Spread());
      //wpn = new Weapon(this, world, WpnDesc.Circle(150));
      //wpn = new Weapon(this, world, new WpnDesc());
      wpn = new Weapon(this, world, 1, 30, 700, 1, 10, 50, 2, 30, 0, 0, 0, 1);
      //wpn = new Planet.Weapon(this, world, 1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);
      //wpn = new Weapon(this, world, 1, 60, 400, 1, 50, 50, 1, 30, 0, 0, 0, 10);
      //wpn = new Planet.Weapon(this, world, 1, 30, 700, 1, 10, 50, 2, 30, 0, 0, 0, 1);
      wpn.SetMuzzle(new Vector2(0, -30));
      weapons.Add(wpn);
      Weapon wpn2;
      wpn2 = new CycloneGun(this, world); 
      weapons.Add(wpn2);

      maxHealth = 10;
      currentHealth = maxHealth;

      drawHitbox = true;
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
      //if(Utility.RayCastCircle(Pos, GetDirection() * 10000, Game1.s.Pos, Game1.s.hitbox.Radius, ref Game1.intersectPoint))
        weapons[0].Fire();
    }
    public override void Fire2()
    {
      foreach (GameObject g in world.gameObjects)
      {
        if (!g.IsRewinding())
          g.StartRewind(TimeMachine.framesBetweenStates);
        //g.StartRewind(TimeMachine.maxRewindableFrames);
      }
      foreach (GameObject g in world.projectiles)
      {
        if (!g.IsRewinding())
          g.StartRewind(TimeMachine.framesBetweenStates);
        //g.StartRewind(TimeMachine.maxRewindableFrames);
      }
    }
    public override void Fire3()
    {
      weapons[1].Fire();
      //world.PostGameObj(laser);
    }
  }
}
