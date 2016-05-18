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

        public Ship(Vector2 pos)
            : base(pos)
        {
            this.SetTexture(AssetManager.GetTexture("Ship1"));
        }

        public override void Update(GameTime gt)
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

            base.Update(gt);
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
            destroyed = true;
        }

        public virtual void Fire1()
        {
            //if (shotDelay < 0)
            //{
            //    Vector2 dir = Utility.AngleToVector2(rotation);
            //    Vector2 pos1 = new Vector2(dir.Y, -dir.X) * 5;
            //    Vector2 pos2 = new Vector2(dir.Y, -dir.X) * -5;

            //    AddProjectile(new Projectile(pos + pos1, dir, 750, this));
            //    AddProjectile(new Projectile(pos + pos2, dir, 750, this));
            //    AddProjectile(new Projectile(pos + pos1, dir + Utility.AngleToVector2(rotation + -0.5f), 750, this));
            //    AddProjectile(new Projectile(pos + pos1, dir + Utility.AngleToVector2(rotation + 0.5f), 750, this));

            //    shotDelay = 1 / shotsPerSecond;
            //}
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

        public void AddVelocity(Vector2 v)
        {
            currentVelocity += v;
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

        protected void AddProjectile(Projectile p)
        {
            Game1.objMgr.PostGameObj(p);
        }

    }
}
