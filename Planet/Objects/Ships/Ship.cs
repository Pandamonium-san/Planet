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
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;
    public float currentHealth;
    public float maxHealth;

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
      weapons = new List<Weapon>();
      weaponIndex = 0;
      maxHealth = 10;
      currentHealth = maxHealth;
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
        if (movementDirection != Vector2.Zero && Target == null)
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
      Text weaponText = new Text(AssetManager.GetFont("font1"), CurrentWeapon.Name, Pos + Vector2.UnitY * 30, Color.Green);
      weaponText.Draw(spriteBatch);
    }
    public virtual void Fire1()
    {
      if (IsActive && !Dashing)
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
    public void SetDash(bool dashing)
    {
      if (Dashing == dashing)
        return;
      else
        ToggleDash();
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
        if (!(g is Ship) || g.Layer == Layer || !g.IsActive || g == Target)
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
    }
    public void SetInvulnerable(float invulnerabilityTime)
    {
      Invulnerable = true;
      invulnerabilityTimer.Start(invulnerabilityTime);
    }
    public void Flash(float flashTime, Color color, bool fadeIn, float initialAlpha = 1.0f, bool separate = false)
    {
      flashParticle = new Particle(Pos, flashTex, Vector2.Zero, flashTime, color, initialAlpha, 0, Scale, fadeIn);
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
      currentHealth -= amount;
      if (currentHealth <= 0)
        Die();
      Flash(0.25f, Color.White, false, 0.8f);
      if (Layer == Layer.PLAYER_SHIP)
        SetInvulnerable(0.25f);
    }
    protected void LeadShot(Ship target)
    {
      Ship t = (Ship)target;
      Vector2 AB = t.Pos - Pos;
      AB.Normalize();
      Vector2 u = t.Velocity;
      Vector2 uj = Vector2.Dot(AB, u) * AB;
      Vector2 ui = u - uj;
      float vLenSq = (float)Math.Pow(weapons[weaponIndex].Desc.projSpeed, 2);
      Vector2 vi = ui;
      float viLenSq = vi.LengthSquared();
      float vjLenSq = vLenSq - viLenSq;
      float vjLen = (float)Math.Sqrt(vLenSq - viLenSq); //NaN if vjLenSq is negative
      Vector2 vj = AB * vjLen;
      Vector2 v = vi + vj;
      if (vjLenSq < 0)
        v = t.Pos + u - Pos;
      TurnTowards(Pos + v);
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
