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
    readonly int RewindableFrames = 300;

    FixedList<State> states;
    Stack<State> shadowStack;
    bool rewinding;

    public RewinderShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_002"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_002");
      SetLayer(Layer.PLAYER_SHIP);

      states = new FixedList<State>(RewindableFrames);
      LeadShots = true;
      Scale = .5f;

      Hitbox.LocalScale = 0.5f;
      origin += new Vector2(0, 2);

      Weapon wpn;
      //WpnDesc desc = new WpnDesc(1, 60, 700, 4, 0, 0, 0, 60*6, 90, 60*6/360, 0, 1, true); // spinny laser thing
      //WpnDesc desc = new WpnDesc(1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);                 // spinny projectile thing
      //WpnDesc desc = new WpnDesc(40, 60, 400, 1, 20, 50, 1, 30, 0, 0, 0, 10);              // burst shotgun
      //WpnDesc desc = new WpnDesc(1, 30, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 3);           //normal laser
      //WpnDesc desc = new WpnDesc(10, 20, 1000, 1, 5, 0, 0, 30, 0, 0, 0, 3);           //machine gun
      //WpnDesc desc = new WpnDesc(30, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      //WpnDesc desc = new WpnDesc(0, 4, 10, 100, 0, 0, 0, 1, 360/100f, 0, 0, 10);           //stress test
      WpnDesc desc = new WpnDesc(1000, 30, 1000, 10, 45, 500, 0, 1, 360/10f, 0, 360/10/2, 10, false, true);           //cleanup
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

      desc = new WpnDesc(1, 60, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 0.1f);           // laser
      LaserGun laser = new LaserGun(this, world, desc, 20, false);
      laser.SetMuzzle(new Vector2(0, -20));
      laser.Name = "Laser";
      weapons.Add(laser);

      //wpn3 = new ExplodeGun(this, world);
      //wpn3 = new TurretGun(this, world);
      Weapon wpn3 = new CycloneGun(this, world);
      wpn3.SetMuzzle(new Vector2(0, -30));
      wpn3.Name = "Cyclone";

      weapons.Add(wpn3);
      Weapon wpn4 = new ExplodeGun(this, world);
      wpn4.Name = "Grenade";
      weapons.Add(wpn4);

      desc = new WpnDesc(30, 1, 1000, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      Weapon wpn5 = new NewTestWeapon(this, world, desc, "Experimental");
      weapons.Add(wpn5);

      maxHealth = 1000;
      currentHealth = maxHealth;

      //drawHitbox = false;
    }
    public override void Fire1()
    {
      if (!rewinding)
      {
        base.Fire1();
        if (states.Count > 0)
          states.Peek().firing = true;
      }
    }
    public override void Fire2()
    {
      if (!rewinding)
      {
        StartRewind();
      }
      else if (rewinding)
      {
        StopRewind();
      }
    }
    public override void Switch()
    {
      base.Switch();
      if (states.Count > 0)
        states.Peek().switching = true;
    }
    public override void SwitchTarget()
    {
      base.SwitchTarget();
      if (states.Count > 0)
        states.Peek().switchingTarget = true;
    }
    private void StartRewind()
    {
      rewinding = true;
      color = Color.Turquoise * 0.4f;
      CollisionEnabled = false;
      shadowStack = new Stack<State>();
      flashParticle = null;
    }
    private void StopRewind()
    {
      rewinding = false;
      color = Color.White;
      CollisionEnabled = true;
      RewinderShipShadow rss = new RewinderShipShadow(world, this, weapons, shadowStack);
      world.PostGameObj(rss);
    }
    public override void Update(GameTime gt)
    {
      if (rewinding && states.Count > 0)
      {
        for (int i = 0; i < 2; i++)
        {
          shadowStack.Push(states.Peek());
          LoadState(states.Pop());
          Particle p = world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue16"), -150, 150, -150, 150, 0.5f, Color.White, 0.5f, 16.0f, 0.2f);
          if (states.Count == 0)
          {
            StopRewind();
            break;
          }
        }
      }
      else
      {
        SaveState();
        if (frame % 10 == 0)
          world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue08"), Vector2.Zero, RewindableFrames / 60.0f, Color.White, 0.4f, 1.0f, 0.3f);
        base.Update(gt);
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      if (states.Count > 0)
        spriteBatch.Draw(tex, states.Last.Value.pos, spriteRec, Color.Gray * 0.15f, states.Last.Value.rotation, origin, Scale, SpriteEffects.None, layerDepth + 0.1f);
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
    public class State
    {
      public float currentHealth;
      public Vector2 pos;
      public float rotation;

      // shadow variables
      public Vector2 movementDirection;
      public bool dashing;
      public bool firing;
      public bool switching;
      public bool switchingTarget;
      public int weaponIndex;
      public float rotationModifier;
      public float speedModifier;

      public State(RewinderShip rs)
      {
        currentHealth = rs.currentHealth;
        pos = rs.Pos;
        rotation = rs.Rotation;
        movementDirection = rs.movementDirection;
        weaponIndex = rs.weaponIndex;
        dashing = rs.Dashing;
        rotationModifier = rs.rotationModifier;
        speedModifier = rs.speedModifier;
        firing = false;
        switching = false;
        switchingTarget = false;
      }
    }
  }
}
