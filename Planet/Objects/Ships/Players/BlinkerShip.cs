using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class BlinkerShip : Ship, IPlayerShip
  {
    public Timer AbilityCooldown { get; set; }
    public int AbilityCharges { get; private set; }
    public Player Player { get; set; }

    private Color blinkColor = Color.DeepSkyBlue;
    private float blinkRange = 200;
    private float blinkDelay = 0.3f;
    private Timer blinkTimer1;
    private Timer blinkTimer2;
    private Timer blinkTimer3;
    private Vector2 blinkDirection;
    private int maxCharges = 3;

    public BlinkerShip(Vector2 pos, World world, Player player)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_001"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_001");
      SetLayer(Layer.PLAYER_SHIP);
      Player = player;
      Flash(2.0f, Color.White, false);
      MakeInvulnerable(2.0f);

      maxHealth = 60;
      currentHealth = maxHealth;
      maxShield = 40;
      currentShield = maxShield;
      LeadShots = true;
      Scale = 0.65f;
      Hitbox.LocalScale = 0.7f;

      AbilityCharges = maxCharges;
      AbilityCooldown = new Timer(4.0f, () =>
      {
        AbilityCharges = Math.Min(AbilityCharges + 1, maxCharges);
        if (AbilityCharges < maxCharges)
          AbilityCooldown.Start();
      }, false);

      blinkTimer1 = new Timer(blinkDelay, Blink1, false);
      blinkTimer2 = new Timer(blinkDelay, Blink2, false);
      blinkTimer3 = new Timer(blinkDelay, Blink3, false);

      weapons.Add(WeaponList.Burst(this, world));
      weapons.Add(WeaponList.Wing(this, world));
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      AbilityCooldown.Update(gt);
      blinkTimer1.Update(gt);
      blinkTimer2.Update(gt);
      blinkTimer3.Update(gt);
    }
    // ship disappears, particles spawn
    public override void Fire2()
    {
      if (blinkTimer1.Counting || blinkTimer2.Counting || AbilityCharges <= 0)//AbilityCooldown.Counting)
        return;
      AbilityCharges--;
      if (!AbilityCooldown.Counting)
        AbilityCooldown.Start();
      Flash(0.15f, blinkColor, false, 1.0f, true);
      blinkDirection = movementDirection;
      IsActive = false;
      Visible = false;
      CollisionEnabled = false;
      CreateBlinkParticle(false);
      CreateBlinkParticle2();
      blinkTimer1.Start();
      AudioManager.PlaySound("blink", 0.95f);
    }
    // particles reappear
    private void Blink1()
    {
      Vector2 dir = blinkDirection;
      if (dir != Vector2.Zero)
        dir.Normalize();
      Pos += dir * blinkRange;

      CreateBlinkParticle(true);
      blinkTimer2.Start();
      AudioManager.PlaySound("blink2", 0.95f);
    }
    // ship reappears
    private void Blink2()
    {
      Flash(0.5f, blinkColor, false, 0.9f);
      IsActive = true;
      Visible = true;
      CreateBlinkParticle2();
      blinkTimer3.Start();
    }
    // collision is enabled
    private void Blink3()
    {
      CollisionEnabled = true;
    }
    private void CreateBlinkParticle(bool implode)
    {
      int n = 6;
      Texture2D tex = AssetManager.GetTexture("star1");
      float speed = 400;
      float lifeTime = (float)blinkTimer1.seconds;
      float alpha = 1f;
      float rotationSpeed = (float)Math.PI * 6;
      float scale = 1.3f;

      for (int i = 0; i < n; i++)
      {
        float angle = (float)(Math.PI * 2 / n * i);
        Vector2 pos = Pos;
        Vector2 dir = Utility.AngleToVector2(angle);
        if (implode)
        {
          pos += dir * speed * lifeTime;
          dir = -dir;
        }

        Projectile p = new Projectile(world, tex, pos, dir, speed, 20, this, lifeTime);
        p.Scale = scale;
        p.color = blinkColor;//Color.Transparent;
        p.Visible = false;
        world.PostProjectile(p);

        Particle pr = new Particle(pos, tex, dir * speed, lifeTime, blinkColor, alpha, rotationSpeed, scale);
        pr.FadeIn = implode;
        world.Particles.AddParticle(pr);
      }
    }
    private void CreateBlinkParticle2()
    {
      for (int i = 0; i < 15; i++)
        world.Particles.CreateStar(Pos, .7f, -50, 50, blinkColor, 0.7f, .5f, 0.4f);
    }
  }
}
