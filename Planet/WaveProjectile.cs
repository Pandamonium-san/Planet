using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
    class WaveProjectile : Projectile
    {
        protected Vector2 basepos;
        protected float amplitude;
        protected float frequency;
        protected float timeOffset;

        public WaveProjectile(
            Vector2 pos,
            Vector2 dir,
            float speed,
            float amplitude,
            float frequency,
            float timeOffset,
            float damage = 1,
            GameObject instigator = null,
            float inaccuracy = 0,
            float lifeTime = 3)
            : base(AssetManager.GetTexture("Proj1"), pos, dir, speed, damage, instigator, inaccuracy, lifeTime)
        {
            basepos = pos;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.timeOffset = timeOffset;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            basepos += velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 d = new Vector2(dir.Y, -dir.X);
            //pos = realPos + d * 100 * (float)Math.Sin(gt.ElapsedGameTime.TotalMilliseconds);
            float time = currentLifeTime - maxLifeTime + timeOffset;
            pos = basepos + d * amplitude * (float)Math.Sin(frequency * time);
        }
    }
}
