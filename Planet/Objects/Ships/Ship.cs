using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public abstract class Ship : GameObject
  {
    public Weapon CurrentWeapon { get { return weapons[weaponIndex]; } }
    public float Speed { get { return baseSpeed * speedModifier; } }
    public float RotationSpeed { get { return rotationSpeed * rotationModifier; } }
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public bool Dashing { get; set; }
    public GameObject Target { get; set; }
    public ShipController Controller { get; set; }
    public bool ClampToScreen { get; set; }
    public bool LeadShots { get; set; }
    public bool Invulnerable { get; set; }

    public float baseSpeed = 300;
    public float rotationSpeed = 10;
    public float damageModifier = 1.0f;
    public float incomingDamageModifier = 1.0f;
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;
    public float currentHealth;
    public float maxHealth;

    public float currentShield;
    public float maxShield;
    public float shieldRechargeRate;
    public Timer shieldRechargeDelay;
    public bool recharging;
    public bool freeAim;

    protected Vector2 movementDirection;
    protected float currentRotationSpeed;
    protected Timer invulnerabilityTimer;
    protected Particle flashParticle;
    protected Texture2D flashTex;
    protected List<Weapon> weapons;
    protected int weaponIndex;

    public Ship(Vector2 pos, World world, Texture2D tex)
      : base(pos, world, tex)
    {
      SetTexture(tex);
      weapons = new List<Weapon>();
      weaponIndex = 0;
      maxHealth = 10;
      currentHealth = maxHealth;

      shieldRechargeRate = 7.5f;
      shieldRechargeDelay = new Timer(3, () => recharging = true, false, false);
      invulnerabilityTimer = new Timer(0.0, () => Invulnerable = false, false);
    }
    protected override void DoUpdate(GameTime gt)
    {
      if (Target == null || !Target.IsActive || Target.Layer == Layer)
        Target = NextTarget();

      if (movementDirection != Vector2.Zero)
        movementDirection.Normalize();
      if (Dashing)
      {
        Vector2 dashDir = movementDirection;
        if (dashDir == Vector2.Zero)
          dashDir = Forward;
        if (Acceleration.Length() < Speed * 1.5f) // Turn instantly when dash is initiated
          Rotation = Utility.Vector2ToAngle(dashDir);
        else
          TurnTowards(Pos + dashDir);
        Acceleration = Forward * Speed * 2;
        Velocity = Acceleration;
        //dash trail
        Particle pr = world.Particles.CreateParticle(Pos + Right * 10 - Forward * 25, AssetManager.GetTexture("fire15"), Vector2.Zero, 0.2f, Color.White, 1.0f);
        pr.Rotation = Rotation;
        pr = world.Particles.CreateParticle(Pos - Right * 10 - Forward * 25, AssetManager.GetTexture("fire15"), Vector2.Zero, 0.2f, Color.White, 1.0f);
        pr.Rotation = Rotation;
      }
      else
      {
        if (freeAim || (movementDirection != Vector2.Zero && Target == null))
          TurnTowards(Pos + movementDirection);
        else if (Target != null)
        {
          if (LeadShots)
            LeadShot((Ship)Target);
          else
            TurnTowards(Target.Pos);
        }
        Velocity = movementDirection * Speed;
        Velocity += Acceleration;
      }

      Pos += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation += currentRotationSpeed * rotationModifier * (float)gt.ElapsedGameTime.TotalSeconds;

      movementDirection = Vector2.Zero;
      currentRotationSpeed = 0;
      //speedModifier = 1.0f;
      //rotationModifier = 1.0f;
      Acceleration *= 0.90f;

      shieldRechargeDelay.Update(gt);
      if (recharging && currentShield < maxShield)
      {
        currentShield += shieldRechargeRate * (float)gt.ElapsedGameTime.TotalSeconds;
      }

      if (ClampToScreen)
        RestrictToScreen();

      foreach (Weapon wpn in weapons)
        wpn.Update(gt);

      if (flashParticle != null)
        flashParticle.Update(gt);
      invulnerabilityTimer.Update(gt);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!Visible)
        return;
      base.Draw(spriteBatch);
      if (flashParticle != null)
        flashParticle.Draw(spriteBatch);

      if (Layer != Layer.PLAYER_SHIP)
        return;
      if (Target != null)
      {
        Texture2D circle = AssetManager.GetTexture("crosshair_white_large");
        spriteBatch.Draw(circle, Target.Pos, null, Color.Green * 0.5f, (float)Math.PI / 4, new Vector2(circle.Width / 2, circle.Height / 2), 3.0f * Target.Scale, SpriteEffects.None, 0.0f);
      }
    }
    public virtual void Fire1()
    {
      if (IsActive && (!Dashing || CurrentWeapon.DashUsable))
        CurrentWeapon.Fire();
    }
    public virtual void Fire2()
    {
    }
    public virtual void Switch()
    {
      if (++weaponIndex >= weapons.Count())
        weaponIndex = 0;
      CurrentWeapon.ResetShootTimer();
    }
    public virtual void SwitchTarget()
    {
      Target = NextTarget();
    }
    public virtual void ToggleDash()
    {
      Dashing = !Dashing;
      if (Dashing)
        rotationModifier *= 0.7f;
      else if (!Dashing)
        rotationModifier /= 0.7f;
    }
    public virtual void SetDash(bool dashing)
    {
      if (Dashing == dashing)
        return;
      else
        ToggleDash();
    }
    public int GetWeaponIndex()
    {
      return weaponIndex;
    }
    public Weapon GetWeapon(int index)
    {
      index = MathHelper.Clamp(index, 0, weapons.Count());
      return weapons[index];
    }
    public void SetWeapon(int index)
    {
      weaponIndex = MathHelper.Clamp(index, 0, weapons.Count());
      CurrentWeapon.ResetShootTimer();
    }
    public GameObject NextTarget()
    {
      List<GameObject> go = world.GetGameObjects();
      GameObject nearest = null;
      float nearestDistance = float.MaxValue;
      foreach (GameObject g in go)
      {
        if (!(g is Ship) || g.Layer == Layer || !g.IsActive || g == Target || g.Untargetable)
          continue;
        float distance = Vector2.DistanceSquared(Pos, g.Pos);
        if (distance < nearestDistance)
        {
          nearest = g;
          nearestDistance = distance;
        }
      }
      return nearest;
    }
    public override void DoCollision(GameObject other)
    {
      if (other is Ship)
      {
        Separate(other);
      }
    }
    public override void Die()
    {
      base.Die();
      world.Particles.CreateExplosion(Pos, 0.3f, 0.8f, 0.3f * Scale);
      AudioManager.PlayExplosion(0.2f);
    }
    public void MakeInvulnerable(float invulnerabilityTime)
    {
      Invulnerable = true;
      invulnerabilityTimer.Start(invulnerabilityTime);
    }
    public void Flash(float flashTime, Color color, bool fadeIn, float initialAlpha = 1.0f, bool separate = false, bool defaultTex = false)
    {
      Texture2D tex = flashTex;
      if (defaultTex)
        tex = this.tex;
      flashParticle = new Particle(Pos, tex, Vector2.Zero, flashTime, color, initialAlpha, 0, Scale, fadeIn);
      flashParticle.Rotation = Rotation;
      if (separate)
      {
        world.Particles.AddParticle(flashParticle);
        flashParticle = null;
        return;
      }
      flashParticle.Parent = this;
      flashParticle.layerDepth = layerDepth - 0.01f;
    }
    public void AddAcceleration(Vector2 v)
    {
      Acceleration += v;
    }
    public void SetAcceleration(Vector2 v)
    {
      Acceleration = v;
    }
    public void Move(Vector2 direction)
    {
      if (IsActive)
        movementDirection += direction;
    }
    public void TurnTowards(Vector2 point)
    {
      float rotation;
      Vector2 direction = point - Pos;
      Rotation = MathHelper.WrapAngle(Rotation);

      float desiredAngle = Utility.Vector2ToAngle(direction);
      desiredAngle = MathHelper.WrapAngle(desiredAngle);

      float angleToTarget = desiredAngle - Rotation;
      angleToTarget = MathHelper.WrapAngle(angleToTarget);
      if (Math.Abs(angleToTarget * 60) < RotationSpeed) //turn fully if rotationspeed higher than difference
        rotation = angleToTarget * 60;
      else
        rotation = angleToTarget > 0 ? RotationSpeed : -RotationSpeed;

      currentRotationSpeed += rotation;
    }
    public void TakeDamage(float amount)
    {
      if (Invulnerable)
        return;
      amount *= incomingDamageModifier;
      shieldRechargeDelay.Start();
      recharging = false;
      if (currentShield > 0)
        currentShield -= amount;
      else
        currentHealth -= amount;
      if (currentHealth <= 0)
        Die();
      if (Layer == Layer.PLAYER_SHIP)
      {
        Flash(0.75f, Color.Red, false, 0.8f);
        MakeInvulnerable(0.75f);
      }
      else
      {
        Flash(0.25f, Color.White, false, 0.8f);
      }
    }
    public void TakeDamage(Projectile p)
    {
      if (Invulnerable)
        return;
      shieldRechargeDelay.Start();
      recharging = false;
      if (currentShield > 0)
      {
        currentShield -= p.damage * incomingDamageModifier;
        if (Layer == Layer.PLAYER_SHIP)
        {
          CreateShieldParticle(Utility.Vector2ToAngle(p.Pos - Pos), 0.5f);
          MakeInvulnerable(0.5f);
        }
        else
        {
          CreateShieldParticle(Utility.Vector2ToAngle(p.Pos - Pos), 0.25f);
        }
      }
      else
      {
        TakeDamage(p.damage);
      }
    }
    protected void LeadShot(Ship target)
    {
      Vector2 AB = target.Pos - Pos;
      AB.Normalize();
      Vector2 u = target.Velocity;
      Vector2 uj = Vector2.Dot(AB, u) * AB;
      Vector2 ui = u - uj;
      float vLenSq = (float)Math.Pow(weapons[weaponIndex].Desc.projSpeed, 2);
      Vector2 vi = ui;
      float viLenSq = vi.LengthSquared();
      float vjLenSq = vLenSq - viLenSq;
      float vjLen = (float)Math.Sqrt(vLenSq - viLenSq); //NaN if vjLenSq is negative
      Vector2 vj = AB * vjLen;
      Vector2 v = vi + vj;
      if (vjLenSq <= 0)
        TurnTowards(target.Pos);
      else
        TurnTowards(Pos + v);
    }
    private void CreateShieldParticle(float rotation, float duration)
    {
      string shieldTex;
      float f = currentShield / maxShield;
      if (f < 0)
        f = 0;
      if (f > 0.7f)
        shieldTex = "shield3";
      else if (f > 0.4f)
        shieldTex = "shield2";
      else
        shieldTex = "shield1";
      Particle shield = world.Particles.CreateParticle(
        Pos,
        AssetManager.GetTexture(shieldTex),
        Vector2.Zero,
        duration,
        Color.Turquoise,
        1.2f * (f + 0.5f),
        0,
        Scale,
        rotation);
      shield.Parent = this;
      Flash(duration, Color.Turquoise, false, 0.5f * (f + 0.5f));
    }
    private void Separate(GameObject other)
    {
      Vector2 dir = Pos - other.Pos;
      if (dir == Vector2.Zero)
        dir = new Vector2(1, 0);
      else
        dir.Normalize();
      AddAcceleration(dir * 2);
    }
    private void RestrictToScreen()
    {
      Pos = new Vector2(
        MathHelper.Clamp(Pos.X, 0, Game1.ScreenWidth),
        MathHelper.Clamp(Pos.Y, 0, Game1.ScreenHeight)
        );
    }
  }
}
