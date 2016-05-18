using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
    public enum Layer
    {
        ZERO = 0x00000000,
        PLAYER_SHIP = 0x00000001,
        PLAYER_PROJECTILE = 0x00000002,
        ENEMY_SHIP = 0x00000004,
        ENEMY_PROJECTILE = 0x00000008
    }

    public class GameObject : Transform
    {
        private Texture2D tex;

        public Vector2 origin;
        public Rectangle spriteRec;
        public Color color = Color.White;
        public float alpha = 1f;
        public float layerDepth = 0f;

        public Rectangle hitbox;
        public Vector2 hitboxOffset;

        public Layer layer;
        public Layer layerMask;

        public int frame;
        public bool destroyed;

        public GameObject(Vector2 pos)
            : base(pos)
        {
            this.Pos = pos;
        }

        public GameObject()
            : base(Vector2.Zero)
        {
            this.Pos = Vector2.Zero;
        }

        protected void SetTexture(Texture2D tex)
        {
            this.tex = tex;
            spriteRec = new Rectangle(0, 0, tex.Width, tex.Height);
            origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
            CreateHitbox();
        }
        protected void SetTexture(Texture2D tex, Rectangle spriteRec)
        {
            this.tex = tex;
            this.spriteRec = spriteRec;
            origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);
            CreateHitbox();
        }

        public bool IsColliding(GameObject other)
        {
            ++Game1.objMgr.collisionChecksPerFrame;
            if ((layerMask & other.layer) != Layer.ZERO)
            {
                return hitbox.Intersects(other.hitbox);
            }
            return false;
        }

        public virtual void DoCollision(GameObject other)
        {

        }

        public virtual void Update(GameTime gt)
        {
            ++frame;
            UpdateHitboxPos();
        }

        public void UpdateHitboxPos()
        {
            hitbox.X = (int)(Pos.X - origin.X + 0.5f * (spriteRec.Width * (1.0f - Scale) + hitboxOffset.X));
            hitbox.Y = (int)(Pos.Y - origin.Y + 0.5f * (spriteRec.Height * (1.0f - Scale) + hitboxOffset.Y));
        }

        /// <summary>
        /// Automatically generates a hitbox based on the sprite.
        /// </summary>
        public void CreateHitbox()
        {
            if (tex == null)
                return;
            hitbox = new Rectangle(
                (int)(Pos.X - origin.X + 0.5f * (spriteRec.Width * (1.0f - Scale) + hitboxOffset.X)),
                (int)(Pos.Y - origin.Y + 0.5f * (spriteRec.Height * (1.0f - Scale) + hitboxOffset.Y)),
                (int)(spriteRec.Width * Scale - hitboxOffset.X),
                (int)(spriteRec.Height * Scale - hitboxOffset.Y)
                );
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
                spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);
            //show hitboxes, may be slow
            //spriteBatch.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Red * 0.5f);
        }
    }
}
