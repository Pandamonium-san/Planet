using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Ship : Actor
  {
    protected Vector2 currentVelocity;
    protected float currentRotationSpeed;
    protected float baseSpeed = 400;
    protected float rotationSpeed = 10;

    protected bool aiming;

    public float fireRateModifier = 1.0f;
    public float speedModifier = 1.0f;
    public float rotationModifier = 1.0f;

    protected List<Weapon> weapons;

    public Ship(Vector2 pos, World world)
        : base(pos, world)
    {
      weapons = new List<Weapon>();
      this.SetTexture(AssetManager.GetTexture("Ship1"));
    }

    protected override void DoUpdate(GameTime gt)
    {
      DoAiming();

      CalculateCurrentVelocity();
      Pos += currentVelocity * speedModifier * (float)gt.ElapsedGameTime.TotalSeconds;

      CalculateCurrentRotation();
      Rotation += currentRotationSpeed * rotationModifier * (float)gt.ElapsedGameTime.TotalSeconds;

      currentVelocity = Vector2.Zero;
      currentRotationSpeed = 0;
      speedModifier = 1.0f;
      rotationModifier = 1.0f;

      foreach (Weapon wpn in weapons)
      {
        wpn.Update(gt);
      }

      base.DoUpdate(gt);
    }

    protected virtual void DoAiming()
    {
      if (aiming)
      {
        speedModifier -= 0.5f;
        rotationModifier -= 1.0f;
        aiming = false;
      }

    }

    protected virtual void CalculateCurrentVelocity()
    {
      if (currentVelocity != Vector2.Zero)
      {
        float maxSpeed = MathHelper.Clamp(currentVelocity.Length(), 0, baseSpeed);
        currentVelocity.Normalize();
        currentVelocity = currentVelocity * baseSpeed;
      }
    }

    protected virtual void CalculateCurrentRotation()
    {
      if (currentVelocity != Vector2.Zero)
        TurnTowardsPoint(currentVelocity);
    }

    public override void DoCollision(GameObject other)
    {
      Die();
    }

    public virtual void Fire1()
    {

    }
    public virtual void Fire2()
    {

    }

    public virtual void Fire3()
    {

    }

    public virtual void Aim()
    {
      aiming = true;
    }

    public void Move(Vector2 direction)
    {
      currentVelocity += direction * baseSpeed;
    }

    protected void TurnTowardsPoint(Vector2 point)
    {
      Rotation = MathHelper.WrapAngle(Rotation);

      float desiredAngle = Utility.Vector2ToAngle(point);
      desiredAngle = MathHelper.WrapAngle(desiredAngle);

      // Calculate angle to target and use it to lerp rotationspeed. Lerping rotation->desiredAngle does not wrap properly.
      float angleToTarget = desiredAngle - Rotation;
      angleToTarget = MathHelper.WrapAngle(angleToTarget);

      currentRotationSpeed += MathHelper.Lerp(0, angleToTarget, rotationSpeed);
    }
    public Vector2 GetDirection()
    {
      return Utility.AngleToVector2(Rotation);
    }
    public override GOState GetState()
    {
      return new ShipState(this);
    }
    public override void SetState(GOState other)
    {
      base.SetState(other);
      ShipState parent = (ShipState)other;
      foreach (Weapon wpn in weapons)
      {
        foreach (WeaponState wpnState in parent.weaponStates)
        {
          wpn.SetState(wpnState);
        }
      }
    }
    protected class ShipState : GOState
    {
      public List<WeaponState> weaponStates;

      public ShipState(Ship s)
          : base(s)
      {
        weaponStates = new List<WeaponState>();
        foreach (Weapon wpn in s.weapons)
        {
          weaponStates.Add(new WeaponState(wpn));
        }
      }
    }
  }
}
