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
    public Weapon CurrentWeapon { get { return weapons[currentWeapon]; } }
    public bool Dashing { get; set; }
    public GameObject Target { get; set; }

    protected Vector2 acceleration;
    protected Vector2 velocity;
    protected Vector2 movementDirection;
    protected float currentRotationSpeed;
    protected float baseSpeed = 400;
    protected float rotationSpeed = 5;
    protected bool leadShots;

    protected List<Weapon> weapons;
    protected int currentWeapon;
    protected Timer damageTimer;

    public bool restrictToScreen;

    public float fireRateModifier = 1.0f;
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;

    public float currentHealth;
    public float maxHealth;

    public Ship(Vector2 pos, World world, Texture2D tex)
      : base(pos, world, tex)
    {
      weapons = new List<Weapon>();
      currentWeapon = 0;
      maxHealth = 10;
      currentHealth = maxHealth;

      damageTimer = new Timer(0.25f);
    }
    protected override void DoUpdate(GameTime gt)
    {
      if (Target == null || !Target.isActive)
        Target = NextTarget();

      if (Dashing)
      {
        Vector2 dashDir = movementDirection;
        if (dashDir == Vector2.Zero)
          dashDir = Forward;
        if (acceleration.Length() < baseSpeed * 1.5f) // Turn instantly when dash is initiated
          Rotation = Utility.Vector2ToAngle(dashDir);
        else
          TurnTowards(Pos + dashDir);
        rotationModifier = 0.3f;
        acceleration = Forward * baseSpeed * 2;
        velocity = acceleration * speedModifier;
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
          if (leadShots)
            LeadShot((Ship)Target);
          else
            TurnTowards(Target.Pos);
        }
        velocity = movementDirection * baseSpeed * speedModifier;
        velocity += acceleration * speedModifier;
      }

      Pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation += currentRotationSpeed * rotationModifier * (float)gt.ElapsedGameTime.TotalSeconds;

      movementDirection = Vector2.Zero;
      currentRotationSpeed = 0;
      speedModifier = 1.0f;
      rotationModifier = 1.0f;
      acceleration *= 0.90f;

      if (restrictToScreen)
        RestrictToScreen();

      foreach (Weapon wpn in weapons)
        wpn.Update(gt);

      // flash when damaged
      if (damageTimer.counting)
      {
        alpha = 0.55f + (float)damageTimer.Fraction * 0.45f;
        damageTimer.Update(gt);
      }
      base.DoUpdate(gt);
    }
    public virtual void Fire1()
    {
      if (!Dashing)
        CurrentWeapon.Fire();
    }
    public virtual void Fire2() { }
    public virtual void Switch()
    {
      if (++currentWeapon >= weapons.Count())
        currentWeapon = 0;
      CurrentWeapon.ResetShootTimer();
    }
    public virtual void SwitchTarget()
    {
      Target = NextTarget();
    }
    protected virtual GameObject NextTarget()
    {
      List<GameObject> go = world.GetGameObjects();
      GameObject nearest = null;
      float nearestDistance = float.MaxValue;
      foreach (GameObject g in go)
      {
        if (!(g is Ship) || g.layer == layer || !g.isActive || g == Target)
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
      if (other is Projectile)
      {
        Projectile p = (Projectile)other;
        TakeDamage(p.damage);
      }
    }
    public void AddAcceleration(Vector2 v)
    {
      acceleration += v;
    }
    public void SetAcceleration(Vector2 v)
    {
      acceleration = v;
    }
    public void Move(Vector2 direction)
    {
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
      if (Math.Abs(angleToTarget * 60) < rotationSpeed) //turn fully if rotationspeed higher than difference
        rotation = angleToTarget * 60;
      else
        rotation = MathHelper.Lerp(0, angleToTarget, rotationSpeed);

      currentRotationSpeed += rotation;
    }
    public void TakeDamage(int amount)
    {
      currentHealth -= amount;
      if (currentHealth < 0)
        Die();
      damageTimer.Start();
    }
    protected void LeadShot(Ship target)
    {
      Ship t = (Ship)target;
      Vector2 AB = t.Pos - Pos;
      AB.Normalize();
      Vector2 u = t.velocity;
      Vector2 uj = Vector2.Dot(AB, u) * AB;
      Vector2 ui = u - uj;
      float vLenSq = (float)Math.Pow(weapons[currentWeapon].Desc.projSpeed, 2);
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
    private void RestrictToScreen()
    {
      Pos = new Vector2(
        MathHelper.Clamp(Pos.X, 0, Game1.ScreenWidth),
        MathHelper.Clamp(Pos.Y, 0, Game1.ScreenHeight)
        );
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      if (layer != Layer.PLAYER_SHIP)
        return;
      if (Target != null)
      {
        Texture2D circle = AssetManager.GetTexture("crosshair_white_large");
        spriteBatch.Draw(circle, Target.Pos, null, Color.Green * 0.5f, (float)Math.PI / 4, new Vector2(circle.Width / 2, circle.Height / 2), 3.0f * Target.Scale, SpriteEffects.None, 0.0f);
      }
      Text weaponText = new Text(AssetManager.GetFont("font1"), CurrentWeapon.Name, Pos + Vector2.UnitY * 30, Color.Green);
      weaponText.Draw(spriteBatch);
    }
  }
}
