using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    public class Projectile : GameObject
    {
        public float damage;
        public GameObject instigator;

        protected Vector2 dir;
        protected Vector2 velocity;
        protected float speed;
        protected float currentLifeTime;
        protected Vector2 acceleration;

        public Projectile(
            Vector2 pos,
            Vector2 dir,
            float speed,
            float damage = 1,
            GameObject instigator = null,
            float inaccuracy = 0,
            float lifeTime = 3)
            : base(pos)
        {
            this.SetTexture(AssetManager.GetTexture("Proj1"));
            this.speed = speed;
            this.instigator = instigator;
            this.damage = damage;

            if (dir != Vector2.Zero)
                dir.Normalize();
            this.dir = dir;

            if (inaccuracy != 0)
                AddSpread(inaccuracy);

            currentLifeTime = lifeTime;
            velocity = this.dir * speed;

            if (instigator == null)
            {
                layer = Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE;
                layerMask = ~Layer.ZERO;
            }
            else if (instigator.layer == Layer.PLAYER)
            {
                layer = Layer.PLAYER_PROJECTILE;
                layerMask = (Layer.ENEMY | Layer.ENEMY_PROJECTILE);
            }
            else if (instigator.layer == Layer.ENEMY)
            {
                layer = Layer.ENEMY_PROJECTILE;
                layerMask = (Layer.PLAYER | Layer.PLAYER_PROJECTILE);
            }
        }
        private void AddSpread(float spread)
        {
            float deviation = (float)((Game1.rnd.NextDouble()-0.5) * spread);
            dir = Utility.RotateVector2(dir, MathHelper.ToRadians(deviation));
        }

        protected virtual void CalculateAcceleration()
        {
        }

        public override void Update(GameTime gt)
        {
            CalculateAcceleration();
            velocity += acceleration * (float)gt.ElapsedGameTime.TotalSeconds;
            pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            currentLifeTime -= (float)gt.ElapsedGameTime.TotalSeconds;
            if (currentLifeTime <= 0)
                destroy = true;
            base.Update(gt);
        }
    }
}
