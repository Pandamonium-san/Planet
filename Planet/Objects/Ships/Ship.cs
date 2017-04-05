using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public abstract class Ship : Actor
  {
    public Weapon CurrentWeapon { get { return weapons[currentWeapon]; } }

    protected GameObject target;
    protected Vector2 acceleration;
    protected Vector2 movementDirection;
    protected float currentRotationSpeed;
    protected float baseSpeed = 400;
    protected float rotationSpeed = 5;

    protected List<Weapon> weapons;
    protected int currentWeapon;
    protected Timer damageTimer;

    protected bool dashing;
    public bool restrictToScreen;

    public float fireRateModifier = 1.0f;
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;

    public int currentHealth;
    public int maxHealth;

    public Ship(Vector2 pos, World world)
      : base(pos, world)
    {
      weapons = new List<Weapon>();
      this.SetTexture(AssetManager.GetTexture("Ship1"));
      maxHealth = 10;
      currentHealth = maxHealth;

      damageTimer = new Timer(0.25f, () => color = Color.White, false);
    }

    protected override void DoUpdate(GameTime gt)
    {
      if (target == null || !target.isActive)
      {
        target = AcquireTarget();
        if (target == null)
          currentRotationSpeed += TurnTowardsPoint(Pos + movementDirection);
      }
      else if (!dashing)
      {
        if (target is Ship && this is RewinderShip)
        {
          //code for leading shots
          Ship t = (Ship)target;
          Vector2 AB = t.Pos - Pos;
          AB.Normalize();
          Vector2 u = t.acceleration - movementDirection * baseSpeed * speedModifier * (float)gt.ElapsedGameTime.TotalSeconds;
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
            v = t.Pos + u;
          currentRotationSpeed += TurnTowardsPoint(Pos + v);
        }
        else
        {
          currentRotationSpeed += TurnTowardsPoint(target.Pos);
        }
      }

      Pos += CurrentVelocity() * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation += CurrentRotation() * (float)gt.ElapsedGameTime.TotalSeconds;

      movementDirection = Vector2.Zero;
      currentRotationSpeed = 0;
      speedModifier = 1.0f;
      rotationModifier = 1.0f;
      dashing = false;


      foreach (Weapon wpn in weapons)
      {
        wpn.Update(gt);
      }

      if (restrictToScreen)
        RestrictToScreen();

      // flash white when damaged
      if (damageTimer.counting)
      {
        color.A = (byte)MathHelper.Lerp(100, 250, (float)damageTimer.Fraction);
        damageTimer.Update(gt);
      }
      base.DoUpdate(gt);
    }

    public virtual void Fire1()
    {
      CurrentWeapon.Fire();
    }
    public virtual void Fire2() { }
    public virtual void Switch()
    {
      if (++currentWeapon >= weapons.Count())
        currentWeapon = 0;
      CurrentWeapon.ResetShootTimer();
    }
    public virtual void Dash()
    {
      dashing = true;
      Vector2 d = movementDirection;
      if (d != Vector2.Zero)
        d.Normalize();
      else
        d = Forward();
      if (acceleration.Length() < baseSpeed * 1.5f)
        Rotation = Utility.Vector2ToAngle(d);
      acceleration = Forward() * baseSpeed * 2;
      currentRotationSpeed += TurnTowardsPoint(Pos + d);
      rotationModifier = 0.3f;
    }
    public virtual void Aim()
    {
      target = AcquireTarget();
    }

    protected virtual GameObject AcquireTarget()
    {
      List<GameObject> go = world.GetGameObjects();
      GameObject nearest = null;
      float nearestDistance = float.MaxValue;
      foreach (GameObject g in go)
      {
        if (g.layer != Layer.ENEMY_SHIP || !g.isActive || g == target)
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

    protected virtual Vector2 CurrentVelocity()
    {
      if (movementDirection != Vector2.Zero && !dashing)
      {
        //float maxSpeed = MathHelper.Clamp(currentVelocity.Length(), 0, baseSpeed);
        movementDirection.Normalize();
        movementDirection = movementDirection * baseSpeed * speedModifier;
      }
      movementDirection += acceleration * speedModifier;
      acceleration *= 0.95f;
      return movementDirection;
    }
    protected virtual float CurrentRotation()
    {
      return currentRotationSpeed * rotationModifier;
    }
    public override void DoCollision(GameObject other)
    {
      if (other is Projectile)
      {
        Projectile p = (Projectile)other;
        TakeDamage(p.damage);
      }
    }
    public void AddDrift(Vector2 v)
    {
      acceleration += v;
    }
    public void SetDrift(Vector2 v)
    {
      acceleration = v;
    }
    public void Move(Vector2 direction)
    {
      movementDirection += direction;
    }
    public float TurnTowardsPoint(Vector2 point)
    {
      Vector2 direction = point - Pos;
      Rotation = MathHelper.WrapAngle(Rotation);

      float desiredAngle = Utility.Vector2ToAngle(direction);
      desiredAngle = MathHelper.WrapAngle(desiredAngle);

      // Calculate angle to target and use it to lerp rotationspeed. Lerping rotation->desiredAngle does not wrap properly.
      float angleToTarget = desiredAngle - Rotation;
      angleToTarget = MathHelper.WrapAngle(angleToTarget);
      if (Math.Abs(angleToTarget * 60) < rotationSpeed)
        return angleToTarget * 60;

      return MathHelper.Lerp(0, angleToTarget, rotationSpeed);
    }
    public void TakeDamage(int amount)
    {
      currentHealth -= amount;
      if (currentHealth < 0)
        Die();
      damageTimer.Start();
    }
    private void RestrictToScreen()
    {
      Pos = new Vector2(
        MathHelper.Clamp(Pos.X, 0, Game1.ScreenWidth),
        MathHelper.Clamp(Pos.Y, 0, Game1.ScreenHeight)
        );
    }
    public Vector2 Forward()
    {
      return Utility.AngleToVector2(Rotation);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      if (target != null)
      {
        Texture2D circle = AssetManager.GetTexture("Circle");
        spriteBatch.Draw(circle, target.Pos, null, Color.Red * 0.3f, 0.0f, new Vector2(circle.Width / 2, circle.Height / 2), 0.2f, SpriteEffects.None, 0.0f);
      }
      spriteBatch.DrawString(AssetManager.GetFont("font1"), CurrentWeapon.Name, Pos + Vector2.UnitY * 20, Color.Green);
    }
  }
}
