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
        protected const float LifeTime = 10;
        Vector2 dir;
        Vector2 velocity;
        GameObject instigator;

        float speed;
        float currentLifeTime;

        public Projectile(Vector2 pos, Vector2 dir, float speed, float lifeTime = LifeTime, GameObject instigator = null)
            : base(pos)
        {
            this.SetTexture(AssetManager.GetTexture("Proj1"));

            this.dir = dir;
            if (dir != Vector2.Zero)
                dir.Normalize();
            this.speed = speed;
            this.instigator = instigator;

            currentLifeTime = LifeTime;
            velocity = dir * speed;

            if(instigator.layer == Layer.PLAYER)
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

        public override void Update(GameTime gt)
        {
            pos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            currentLifeTime -= (float)gt.ElapsedGameTime.TotalSeconds;
            if (currentLifeTime <= 0)
                destroy = true;
            base.Update(gt);
        }
    }
}
