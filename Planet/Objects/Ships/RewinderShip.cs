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
      //SetTexture(AssetManager.GetTexture("Ship1"));
      SetTexture(AssetManager.GetTexture("Sprites"), SpriteRegions.Get("Ship2"));
      hitbox.localScale = 0.5f;
      origin += new Vector2(0, 2);
      SetLayer(Layer.PLAYER_SHIP);
      layerDepth = 0.2f;
      rotationSpeed = 15;
      Weapon wpn;
      //WpnDesc desc = new WpnDesc(1, 60, 700, 4, 0, 0, 0, 60*6, 90, 60*6/360, 0, 1, true); // spinny laser thing
      //WpnDesc desc = new WpnDesc(1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);                 // spinny projectile thing
      //WpnDesc desc = new WpnDesc(40, 60, 400, 1, 20, 50, 1, 30, 0, 0, 0, 10);              // burst shotgun
      WpnDesc desc = new WpnDesc(1, 30, 700, 1, 0.1f, 0, 0, 30, 0, 0, 0, 1);           //normal laser
      //WpnDesc desc = new WpnDesc(10, 20, 1000, 1, 5, 0, 0, 30, 0, 0, 0, 3);           //machine gun
      //WpnDesc desc = new WpnDesc(0, 4, 10, 100, 0, 0, 0, 1, 360/100f, 0, 0, 10);           //stress test
      wpn = new Weapon(this, world, desc);
      wpn.SetMuzzle(new Vector2(0, -30));
      weapons.Add(wpn);

      WHitScan hitscan = new WHitScan(this, world, desc, 10, true);
      hitscan.SetMuzzle(new Vector2(0, -20));
      weapons.Add(hitscan);

      Weapon wpn2;
      wpn2 = new CycloneGun(this, world); 
      //wpn2 = new ExplodeGun(this, world);
      //wpn2 = new TurretGun(this, world);
      //wpn2 = new LightningGun(this, world, 2);
      wpn2.SetMuzzle(new Vector2(0, -30));
      weapons.Add(wpn2);
      weapons.Add(new TurretGun(this, world));

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
      //world.Rewind(TimeMachine.framesBetweenStates);
      //world.Rewind(TimeMachine.maxRewindableFrames);
    }
    public override void Fire3()
    {
      weapons[1].Fire();
      //world.PostGameObj(laser);
    }
    public override void Fire4()
    {
      weapons[2].Fire();
    }
  }
}
