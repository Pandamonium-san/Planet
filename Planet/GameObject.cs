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
        public int framesTilDispose;
        public bool isRewindable = true;

        //possible way to handle deathstates
        //public enum State
        //{
        //    Active,           // normal state
        //    Inactive,         // stop updating/drawing/collision checking
        //    Dead,             // delete object after some time
        //    Disposed          // delete object
        //}
        //State state = State.Active;

        public bool disposed;               // if true, object will be deleted at the end of the frame
        public bool isActive = true;        // determines whether or not to draw/update/collision check the object
        public bool isDead;                 // will set to dispose after a number of frames

        //rewind variables
        public static readonly int bufferFrames = 180;    // how many frames to save that can then be rewinded to
        public FixedList<GameObject> stateBuffer;
        public bool rewind;
        public int framesToRewind;

        public GameObject(Vector2 pos)
            : base(pos)
        {
            stateBuffer = new FixedList<GameObject>(bufferFrames);
        }

        public void Update(GameTime gt)
        {
            if (rewind)
                Rewind();
            else
            {
                ++frame;
                if (isDead)
                {
                    framesTilDispose--;
                    if (framesTilDispose <= 0)
                        disposed = true;
                }
                DoUpdate(gt);
            }
        }

        protected virtual void DoUpdate(GameTime gt)
        {
            UpdateHitboxPos();
            SaveCurrentState();
        }

        private void Rewind()
        {
            LoadPreviousState();
            if (--framesToRewind <= 0)
                rewind = false;
        }

        protected void SaveCurrentState()
        {
            if (isRewindable)
                stateBuffer.AddFirst(GetState());
        }

        public void LoadPreviousState()
        {
            // end of queue has been reached
            if (stateBuffer.Count <= 0)
            {
                // remove object if rewinding to before object was created
                if (frame == 1)
                    disposed = true;
                // ends rewind for all objects (hopefully)
                else
                    rewind = false;
                return;
            }
            SetState(stateBuffer.Pop());
        }

        public void StartRewind(int frames)
        {
            if (!isRewindable)
                return;

            rewind = true;
            framesToRewind = frames;
        }

        /// <summary>
        /// Gets all neccessary data in order to rewind to this state (all the dynamic variables)
        /// </summary>
        protected virtual GameObject GetState()
        {
            GameObject g = new GameObject(Pos);
            g.Rotation = this.Rotation;
            g.Scale = this.Scale;
            g.Parent = this.Parent;
            g.frame = this.frame;
            g.isDead = this.isDead;
            g.isActive = this.isActive;
            return g;
        }

        /// <summary>
        /// Sets the neccessary variables
        /// </summary>
        protected virtual void SetState(GameObject other)
        {
            this.Pos = other.Pos;
            this.Rotation = other.Rotation;
            this.Parent = other.Parent;
            this.frame = other.frame;
            this.isDead = other.isDead;
            this.isActive = other.isActive;
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

        public void Die()
        {
            isDead = true;
            isActive = false;
            framesTilDispose = bufferFrames;
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

            if (tex != null && isActive)
            {
                spriteBatch.Draw(tex, Pos, spriteRec, color * alpha, Rotation, origin, Scale, SpriteEffects.None, layerDepth);

                GameObject old = null;
                if (stateBuffer.Count > 0)
                    old = ((GameObject)(stateBuffer.Last.Value));
                if (old != null)
                    spriteBatch.Draw(tex, old.Pos, spriteRec, Color.Red * alpha * 0.2f, old.Rotation, origin, Scale, SpriteEffects.None, layerDepth);
            }
            //show hitboxes, may be slow
            //spriteBatch.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Blue * 0.5f);
        }
    }
}
