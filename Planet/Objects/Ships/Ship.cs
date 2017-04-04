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
    protected GameObject target;
    protected Vector2 drift;
    protected Vector2 currentVelocity;
    protected float currentRotationSpeed;
    protected float baseSpeed = 400;
    protected float rotationSpeed = 5;

    protected bool dashing;
    public bool restrictToScreen;

    public float fireRateModifier = 1.0f;
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;

    public int currentHealth;
    public int maxHealth;

    protected List<Weapon> weapons;
    protected Timer damageTimer;

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
      if (target == null || !target.isActive )
        target = AcquireTarget();
      else if(!dashing)
      {
        currentRotationSpeed += TurnTowardsPoint(target.Pos);
      }

      Pos += CurrentVelocity() * (float)gt.ElapsedGameTime.TotalSeconds;
      Rotation += CurrentRotation() * (float)gt.ElapsedGameTime.TotalSeconds;

      currentVelocity = Vector2.Zero;
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
      if (currentVelocity != Vector2.Zero && !dashing)
      {
        //float maxSpeed = MathHelper.Clamp(currentVelocity.Length(), 0, baseSpeed);
        currentVelocity.Normalize();
        currentVelocity = currentVelocity * baseSpeed * speedModifier;
      }
      currentVelocity += drift * speedModifier;
      drift *= 0.95f;
      return currentVelocity;
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
    public virtual void Fire1() { }
    public virtual void Fire2() { }
    public virtual void Fire3() { }
    public virtual void Fire4()
    {
      dashing = true;
      Vector2 d = currentVelocity;
      if (d != Vector2.Zero)
        d.Normalize();
      else
        d = Forward();
      if (drift.Length() < baseSpeed * 1.5f)
        Rotation = Utility.Vector2ToAngle(d);
      drift = Forward() * baseSpeed * 2;
      currentRotationSpeed += TurnTowardsPoint(Pos + d);
      rotationModifier = 0.3f;
    }
    public virtual void Aim()
    {
      target = AcquireTarget();
    }
    public void AddDrift(Vector2 v)
    {
      drift += v;
    }
    public void SetDrift(Vector2 v)
    {
      drift = v;
    }
    public void Move(Vector2 direction)
    {
      currentVelocity += direction;
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
    }
  }
}
