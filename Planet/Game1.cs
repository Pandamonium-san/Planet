using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
    /*  TO DO
     *  x Weapon settings
     *  x Weapon templates
     *  x Bullet patterns
     *  x Make Transform class for GameObject to make transform hierarchies
     *  - Put bullet pattern in class?
     *  - Destructible projectiles
     *  - Health
     *  - Enemy spawns
     *  - Enemy variations
     *  - Projectile variations
     *      - Homing missiles
     *      - Explosions?
     *      - Projectile settings?
     *      - Projectile paths/patterns
     *  - Smaller hitboxes
     *  - Show hitbox while aiming
     *  - (Lasers)
     *  - Game states
     *  - Rewinder
     *      - Fix structure of rewind code
     *      - Proper copy constructors
     *      - Manual save state (rewind to this point later)
     *  - Possessor
     */

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
#if (!ARCADE)
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
#else
		public override string GameDisplayName { get { return "Planet"; } }
#endif
        public static Random rnd = new Random();
        public static ObjectManager objMgr;
        private FrameCounter fc = new FrameCounter();
        private bool runningSlowly;

        public Game1()
        {
#if (!ARCADE)
            graphics = new GraphicsDeviceManager(this);
#endif
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if (!ARCADE)
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
#endif
            AssetManager.LoadContent(Content);
            objMgr = new ObjectManager();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if (!ARCADE)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            // TODO: Add your update logic here
            runningSlowly = gameTime.IsRunningSlowly;

            objMgr.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            fc.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // TODO: Add your drawing code here
            objMgr.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(AssetManager.GetFont("font1"), "FPS: " + fc.AverageFramesPerSecond.ToString(), new Vector2(0, 0), Color.Red);
            spriteBatch.DrawString(AssetManager.GetFont("font1"), "slow: " + runningSlowly.ToString(), new Vector2(150, 0), Color.Red);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
