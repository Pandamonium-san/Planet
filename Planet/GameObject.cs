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
        protected Texture2D tex;

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
        public bool isRewindable = true;
        public bool destroyed;

        //rewind variables
        public FixedList<GameObject> objStates;
        public int iterator;
        public static readonly int queueSize = 120;
        public bool rewind;

        public GameObject(Vector2 pos)
            : base(pos)
        {
            objStates = new FixedList<GameObject>(queueSize);
        }

        //public GameObject()
        //    : base(Vector2.Zero)
        //{
        //    objStates = new FixedList<GameObject>(queueSize);
        //}

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
            if (rewind)
            {
                LoadState(1);
                return;
            }
            UpdateHitboxPos();
            SaveState();
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

        protected void SaveState()
        {
            if (isRewindable)
                objStates.AddFirst(GetState());
        }

        public void LoadState(int frames)
        {
            if (!isRewindable)
                return;

            if (objStates.Count <= frames)
            {
                frames = objStates.Count;
                rewind = false;
            }

            for (int i = 0; i < frames - 1; i++)
                objStates.Pop();

            SetState(objStates.Pop());
        }

        protected virtual GameObject GetState()
        {
            GameObject g = new GameObject(Pos);
            g.Rotation = this.Rotation;
            g.Scale = this.Scale;
            g.Parent = this.Parent;
            g.frame = this.frame;
            return g;
        }

        protected virtual void SetState(GameObject other)
        {
            if (other == null)
            {
                //destroyed = true;
                return;
            }
            GameObject g = (GameObject)other;
            this.Pos = other.Pos;
            this.Rotation = other.Rotation;
            this.Parent = other.Parent;
            this.frame = other.frame;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
            {
                spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);

                GameObject old = null;
                if (objStates.Count > 0)
                    old = ((GameObject)(objStates.Last.Value));
                if(old != null)
                spriteBatch.Draw(tex, old.Pos, spriteRec, Color.Red * alpha * 0.2f, old.Rotation, origin, Scale, SpriteEffects.None, layerDepth);
            }
            //show hitboxes, may be slow
            //spriteBatch.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Red * 0.5f);
        }
    }
}
