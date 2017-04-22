using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class RewinderShip : Ship
  {
    FixedList<State> states;
    bool rewinding;

    public RewinderShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture("enemyblue5"))
    {
      SetLayer(Layer.PLAYER_SHIP);

      states = new FixedList<State>(120);
      LeadShots = true;
      Scale = .5f;

      hitbox.localScale = 0.5f;
      origin += new Vector2(0, 2);
      layerDepth = 0.2f;

      Weapon wpn;
      //WpnDesc desc = new WpnDesc(1, 60, 700, 4, 0, 0, 0, 60*6, 90, 60*6/360, 0, 1, true); // spinny laser thing
      //WpnDesc desc = new WpnDesc(1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);                 // spinny projectile thing
      //WpnDesc desc = new WpnDesc(40, 60, 400, 1, 20, 50, 1, 30, 0, 0, 0, 10);              // burst shotgun
      //WpnDesc desc = new WpnDesc(1, 30, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 3);           //normal laser
      //WpnDesc desc = new WpnDesc(10, 20, 1000, 1, 5, 0, 0, 30, 0, 0, 0, 3);           //machine gun
      WpnDesc desc = new WpnDesc(30, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      //WpnDesc desc = new WpnDesc(0, 4, 10, 100, 0, 0, 0, 1, 360/100f, 0, 0, 10);           //stress test
      wpn = new Weapon(this, world, desc, "laserBlue16");
      wpn.SetMuzzle(new Vector2(0, -30));
      wpn.Name = "Weapon1";
      wpn.Scale = 1.5f;
      weapons.Add(wpn);

      desc = new WpnDesc(1, 60, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 3);           //normal laser
      Weapon wpn2 = new Weapon(this, world, desc);
      wpn2.SetMuzzle(new Vector2(0, -10));
      wpn2.Name = "Line";
      weapons.Add(wpn2);

      LaserGun laser = new LaserGun(this, world, desc, 20, false);
      laser.SetMuzzle(new Vector2(0, -20));
      laser.Name = "Laser";
      weapons.Add(laser);

      Weapon wpn3;
      wpn3 = new LightningGun(this, world, 2);
      //wpn2 = new ExplodeGun(this, world);
      //wpn2 = new TurretGun(this, world);
      //wpn2 = new LightningGun(this, world, 2);
      wpn3.SetMuzzle(new Vector2(0, -30));
      wpn3.Name = "Lightning";

      weapons.Add(wpn3);
      Weapon wpn4 = new ExplodeGun(this, world);
      wpn4.Name = "Grenade";
      weapons.Add(wpn4);

      desc = new WpnDesc(20, 1, 1000, 1, 0, 0, 0, 1, 0, 0, 0, 3);
      Weapon wpn5 = new NewTestWeapon(this, world, desc, "Experimental");
      weapons.Add(wpn5);

      maxHealth = 1000;
      currentHealth = maxHealth;

      //drawHitbox = false;
    }
    public override void Fire1()
    {
      if (!rewinding)
        base.Fire1();
    }
    public override void Fire2()
    {
      rewinding = !rewinding;
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
      if (rewinding && states.Count > 0)
      {
        LoadState(states.Pop());
        color = Color.Turquoise * 0.4f;
        CollisionEnabled = false;
        Particle p = world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue16"), -150, 150, -150, 150, 0.5f, Color.White, 0.5f, 16.0f, 0.2f);
      }
      else
      {
        color = Color.White;
        CollisionEnabled = true;
        SaveState();
        if(frame % 10 == 0)
          world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue08"), Vector2.Zero, 2.0f, Color.White, 0.4f, 1.0f, 0.3f);
        rewinding = false;
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      if (states.Count > 0)
        spriteBatch.Draw(tex, states.Last.Value.pos, spriteRec, Color.Gray * 0.3f, states.Last.Value.rotation, origin, Scale, SpriteEffects.None, layerDepth + 0.1f);
    }
    private void LoadState(State state)
    {
      Pos = state.pos;
      Rotation = state.rotation;
      currentHealth = state.currentHealth;
    }
    private void SaveState()
    {
      states.AddFirst(new State(this));
    }
    class State
    {
      public float currentHealth;
      public Vector2 pos;
      public float rotation;
      public State(RewinderShip rs)
      {
        currentHealth = rs.currentHealth;
        pos = rs.Pos;
        rotation = rs.Rotation;
      }
    }
  }
}
