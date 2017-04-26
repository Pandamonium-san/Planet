using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class BlinkerShip : Ship
  {
    Color blinkColor = Color.DeepSkyBlue;
    float blinkRange = 200;
    Timer blinkDelay;
    Vector2 blinkDirection;
    public BlinkerShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_001"))
    {
      SetLayer(Layer.PLAYER_SHIP);
      blinkDelay = new Timer(0.2, Blink, false);
      maxHealth = 100;
      currentHealth = maxHealth;

      WpnDesc desc = new WpnDesc(15, 1, 700, 12, 10, 100, 0, 1, 0, 0, 0, 1);              // burst shotgun
      Weapon wpn = new Weapon(this, world, desc, "laserBlue07");
      wpn.Name = "Shotgun2";
      weapons.Add(wpn);

      desc = new WpnDesc(5, 60, 500, 8, 10, 50, 1, 30, 360 / 8f, 360 / 30f, 0, 0.2f);                 // spinny projectile thing
      wpn = new Weapon(this, world, desc, "laserBlue10");
      wpn.Scale = 0.5f;
      wpn.Name = "Spin";
      weapons.Add(wpn);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      blinkDelay.Update(gt);
    }
    protected override void DoUpdate(GameTime gt)
    {
      base.DoUpdate(gt);
    }
    public override void Fire1()
    {
      CurrentWeapon.Fire();
    }
    public override void Fire2()
    {
      blinkDelay.Start();
      blinkDirection = movementDirection;
      CreateBlinkParticle(false);
      IsActive = false;
      Visible = false;
      CollisionEnabled = false;
    }
    private void Blink()
    {
      Vector2 dir = blinkDirection;
      if (dir != Vector2.Zero)
        dir.Normalize();
      Pos += dir * blinkRange;
      CreateBlinkParticle(true);
      IsActive = true;
      Visible = true;
      CollisionEnabled = true;
    }

    private void CreateBlinkParticle(bool implode)
    {
      int n = 6;
      Texture2D tex = AssetManager.GetTexture("star1");
      float speed = 500;
      float lifeTime = (float)blinkDelay.seconds;
      float alpha = 1.0f;
      float rotationSpeed = (float)Math.PI * 8;
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

        Projectile p = new Projectile(world, tex, pos, dir, speed, 40, this, lifeTime);
        p.Scale = scale;
        p.color = blinkColor;
        world.PostProjectile(p);

        Particle pr = new Particle(pos, tex, dir * speed, lifeTime, blinkColor, alpha, rotationSpeed, scale);
        world.Particles.AddParticle(pr);
      }
    }
  }
}
