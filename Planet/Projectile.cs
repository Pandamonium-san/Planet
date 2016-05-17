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

        public Vector2 dir;
        public Vector2 velocity;
        public float speed;
        protected float maxLifeTime;
        protected float currentLifeTime;
        protected Vector2 acceleration;

        public delegate void Pattern(Projectile p, GameTime gt);
        Pattern pattern;

        public Projectile(
            Texture2D tex,
            Vector2 pos,
            Vector2 dir,
            float speed,
            float damage = 1,
            GameObject instigator = null,
            float inaccuracy = 0,
            float lifeTime = 3,
            Pattern pattern = null)
            : base(pos)
        {
            this.pattern = pattern;

            if (tex != null)
                this.SetTexture(tex);
            this.speed = speed;
            this.instigator = instigator;
            this.damage = damage;

            if (dir != Vector2.Zero)
                dir.Normalize();
            this.dir = dir;

            maxLifeTime = lifeTime;
            currentLifeTime = lifeTime;
            velocity = this.dir * speed;

            if (instigator == null)
            {
                layer = Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE;
                layerMask = ~Layer.ZERO;
            }
            else if (instigator.layer == Layer.PLAYER_SHIP)
            {
                layer = Layer.PLAYER_PROJECTILE;
                layerMask = (Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE);
            }
            else if (instigator.layer == Layer.ENEMY_SHIP)
            {
                layer = Layer.ENEMY_PROJECTILE;
                layerMask = (Layer.PLAYER_SHIP | Layer.PLAYER_PROJECTILE);
            }

        }

        public override void Update(GameTime gt)
        {
            //perform bullet pattern operation
            if (pattern != null)
                pattern(this, gt);
            else
                Pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            currentLifeTime -= (float)gt.ElapsedGameTime.TotalSeconds;
            if (currentLifeTime <= 0)
                destroyed = true;
            base.Update(gt);
        }
    }
}
