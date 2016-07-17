using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class RewinderShip : Ship
  {
    Weapon wpn;

    //test
    Laser laser;
    public RewinderShip(Vector2 pos, World world)
        : base(pos, world)
    {
      hitboxOffset = new Vector2(50, 50);
      SetTexture(AssetManager.GetTexture("Ship1"));
      layer = Layer.PLAYER_SHIP;
      rotationSpeed = 15;
      //wpn = new Weapon(this, WpnDesc.Spread());
      //wpn = new Weapon(this, WpnDesc.Circle(150));
      //wpn = new Weapon(this, new WpnDesc());
      wpn = new CycloneGun(this, world);
      weapons.Add(wpn);
      wpn.inaccuracy = 0;
      wpn.speedVariance = 0;
      drawHitbox = true;

      //test
      laser = new Laser(Pos + Vector2.UnitY * -25, Vector2.Zero, world);
      laser.Parent = this;
    }

    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
      wpn.Update(gt);
    }

    public override void Fire1()
    {
      //test
      //Utility.RayTrace(Pos, GetDirection(), Game1.s.hitbox, ref Game1.intersectPoint);
      wpn.Fire();
    }

    public override void Fire2()
    {
      foreach (GameObject g in world.gameObjects)
      {
        if (!g.isRewinding)
          g.StartRewind(TimeMachine.framesBetweenStates);
        //g.StartRewind(TimeMachine.maxRewindableFrames);
      }
      foreach (GameObject g in world.projectiles)
      {
        if (!g.isRewinding)
          g.StartRewind(TimeMachine.framesBetweenStates);
        //g.StartRewind(TimeMachine.maxRewindableFrames);
      }
    }

    public override void Fire3()
    {
      world.PostGameObj(laser);
    }
  }
}
