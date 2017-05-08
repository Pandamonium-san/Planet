using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  class RewinderShip : Ship, IPlayerShip
  {
    public Timer AbilityCooldown { get; set; }

    private readonly int RewindableFrames = 300;
    private FixedList<State> states;
    private Stack<State> shadowStack;
    private bool rewinding;
    private Color rewindColor = Color.Turquoise;// new Color(93, 50, 234);

    public RewinderShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_002"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_002");
      SetLayer(Layer.PLAYER_SHIP);

      maxHealth = 100;
      maxShield = 20;
      currentHealth = maxHealth;
      currentShield = maxShield;
      LeadShots = true;
      Hitbox.LocalScale = 0.9f;

      AbilityCooldown = new Timer(10, null, false);
      states = new FixedList<State>(RewindableFrames);

      Weapon wpn;
      //WpnDesc desc = new WpnDesc(1, 60, 700, 4, 0, 0, 0, 60*6, 90, 60*6/360, 0, 1, true); // spinny laser thing
      //WpnDesc desc = new WpnDesc(1, 5, 200, 4, 2, 0, 1, 30, 90, 18, 0, 10);                 // spinny projectile thing
      //WpnDesc desc = new WpnDesc(40, 60, 400, 1, 20, 50, 1, 30, 0, 0, 0, 10);              // burst shotgun
      //WpnDesc desc = new WpnDesc(1, 30, 1500, 1, 0.1f, 0, 0, 30, 0, 0, 0, 3);           //normal laser
      WpnDesc desc = new WpnDesc(8, 20, 1000, 1, 5, 0, 0, 30, 0, 0, 0, 3);           //machine gun
      //WpnDesc desc = new WpnDesc(30, 1, 2500, 5, 0, 0, 0, 1, 0, 0, 0, 3);           //sniper
      //WpnDesc desc = new WpnDesc(0, 4, 10, 100, 0, 0, 0, 1, 360/100f, 0, 0, 10);           //stress test
      //WpnDesc desc = new WpnDesc(1000, 30, 1000, 10, 45, 500, 0, 1, 360/10f, 0, 360/10/2, 10, false, true);           //cleanup
      wpn = new Weapon(this, world, desc, "laserBlue16");
      wpn.SetMuzzle(new Vector2(0, -20));
      wpn.Name = "Weapon1";
      wpn.Scale = .7f;
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
      if (!rewinding && !AbilityCooldown.Counting)
      {
        StartRewind();
        AbilityCooldown.Start();
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
      Untargetable = true;
      CollisionEnabled = false;
      color = rewindColor * 0.6f;
      shadowStack = new Stack<State>();
      flashParticle = null;
      Flash(0.5f, rewindColor, false, 0.8f, true, false);
      for (int i = 0; i < 10; i++)
        world.Particles.CreateStar(Pos, 0.5f, -70, 70, rewindColor, 0.5f, 0.7f, 0.4f);
    }
    private void StopRewind()
    {
      rewinding = false;
      Untargetable = false;
      CollisionEnabled = true;
      color = Color.White;
      Acceleration = Vector2.Zero;
      Velocity = Vector2.Zero;
      RewinderShipShadow rss = new RewinderShipShadow(world, this, weapons, shadowStack);
      rss.color = rewindColor * 0.4f;
      world.PostGameObj(rss);
      foreach (Weapon wpn in weapons)
        wpn.Reload();
      MakeInvulnerable(0.5f);
      Flash(0.5f, rewindColor, false);
      for (int i = 0; i < 20; i++)
        world.Particles.CreateStar(Pos, 0.5f, -100, 100, rewindColor, 0.5f, 0.7f, 0.4f);
    }
    public override void Update(GameTime gt)
    {
      if (rewinding && states.Count > 0)
      {
        if (states.Count / 2 % 2 == 0)
          Flash(0.3f, rewindColor, false, 0.3f, true, true);
        for (int i = 0; i < 2; i++)
        {
          shadowStack.Push(states.Peek());
          LoadState(states.Pop());
          if (states.Count == 0)
          {
            StopRewind();
            break;
          }
        }
      }
      else
      {
        AbilityCooldown.Update(gt);
        SaveState();
        if (frame % 10 == 0)
          world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue08"), Vector2.Zero, 0.3f + RewindableFrames / 60.0f, Color.White, 0.8f, 1.0f, 0.3f);
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
      currentShield = state.currentShield;
    }
    private void SaveState()
    {
      states.AddFirst(new State(this));
    }
    public class State
    {
      public float currentHealth;
      public float currentShield;
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
        currentShield = rs.currentShield;
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
