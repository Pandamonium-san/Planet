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
        public bool drawHitbox;

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
        public static readonly int maxRewindableFrames = 180; // how many frames back we can go
        public int framesToSkipSaving = 1;   // 1 = save every frame, 2 = save every other frame, 3 = every third frame, etc. Has a large effect on performance
        public int bufferFrames;    // how many frame states actually saved
        protected FixedList<GameObjState> stateBuffer;
        public bool rewind;
        public int remainingFramesToRewind;

        public GameObject(Vector2 pos)
            : base(pos)
        {
            bufferFrames = maxRewindableFrames / framesToSkipSaving;
            stateBuffer = new FixedList<GameObjState>(bufferFrames);
            Scale = 2.0f;
        }

        public void Update(GameTime gt)
        {
            if (rewind)
            {
                DoRewind();
            }
            else
            {
                ++frame;
                if (isDead)
                {
                    framesTilDispose--;
                    if (framesTilDispose <= 0)
                        disposed = true;
                }
                if (isActive)
                {
                    DoUpdate(gt);
                }
                // comparing to 1 saves the first frame, which is important for determining whether an object was just created
                if (isRewindable && frame % framesToSkipSaving == 1)
                    SaveCurrentState();
            }
        }

        protected virtual void DoUpdate(GameTime gt)
        {
            UpdateHitboxPos();
        }

        private void DoRewind()
        {
            if (remainingFramesToRewind % framesToSkipSaving == 1)
                LoadPreviousState();
            else
                LerpBasedOnPreviousState();

            if (--remainingFramesToRewind <= 0)
                rewind = false;

            UpdateHitboxPos();
        }

        /// <summary>
        /// Goes x frames back in time
        /// </summary>
        public void StartRewind(int x)
        {
            if (!isRewindable)
                return;

            rewind = true;
            remainingFramesToRewind = x;
        }

        private void LerpBasedOnPreviousState()
        {
            GameObjState data = stateBuffer.Peek();
            if (data != null)
            {
                Vector2 newPos = new Vector2(
                    MathHelper.Lerp(Pos.X, data.Pos.X, 1.0f / framesToSkipSaving),
                    MathHelper.Lerp(Pos.Y, data.Pos.Y, 1.0f / framesToSkipSaving));
                Pos = newPos;
            }
        }

        protected void SaveCurrentState()
        {
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
                else if (remainingFramesToRewind == 0)
                    rewind = false;
                return;
            }
            SetState(stateBuffer.Pop());
        }

        protected virtual GameObjState GetState()
        {
            return new GameObjState(this);
        }

        protected virtual void SetState(GameObjState data)
        {
            this.Pos = data.Pos;
            this.Rotation = data.Rotation;
            this.Scale = data.Scale;
            this.Parent = data.Parent;
            this.frame = data.frame;
            this.isDead = data.isDead;
            this.isActive = data.isActive;
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
            framesTilDispose = maxRewindableFrames;
        }

        public bool IsColliding(GameObject other)
        {
            if ((layerMask & other.layer) != Layer.ZERO)
            {
                ++Game1.objMgr.collisionChecksPerFrame;
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

                GameObjState old = null;
                if (stateBuffer.Count > 0)
                    old = ((GameObjState)(stateBuffer.Last.Value));
                if (old != null)
                    spriteBatch.Draw(tex, old.Pos, spriteRec, Color.Red * alpha * 0.2f, old.Rotation, origin, Scale, SpriteEffects.None, layerDepth);

                //show hitboxes, may be slow
                if (drawHitbox)
                    spriteBatch.Draw(AssetManager.GetTexture("Fill"), hitbox, Color.Blue * 0.5f);
            }
        }

        /// <summary>
        /// Contains info required to load to a previous state.
        /// </summary>
        protected class GameObjState
        {
            public Vector2 Pos;
            public float Rotation;
            public float Scale;
            public Transform Parent;
            public Color color;
            public float alpha;
            public Rectangle hitbox;
            public int frame;
            public bool isRewindable = true;
            public bool disposed;               // if true, object will be deleted at the end of the frame
            public bool isActive = true;        // determines whether or not to draw/update/collision check the object
            public bool isDead;                 // will set to dispose after a number of frames

            public GameObjState(GameObject go)
            {
                this.Pos = go.Pos;
                this.Rotation = go.Rotation;
                this.Scale = go.Scale;
                this.Parent = go.Parent;
                this.color = go.color;
                this.alpha = go.alpha;
                this.hitbox = go.hitbox;
                this.frame = go.frame;
                this.isRewindable = go.isRewindable;
                this.disposed = go.disposed;
                this.isActive = go.isActive;
                this.isDead = go.isDead;
            }
        }
    }
}
