using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /*  TO DO
   *  - Make proper weapons
   *      - How to instantiate projectile based on class?
   *      - How to implement different particle effects?
   *  - Enemies
   *      - Waves/Levels
   *      - Enemy types
   *      - Enemy AI
   *  - Score
   *  - Weapons
   *      - Missiles/destructible projectiles
   *      - Explosions
   *      - Lasers
   *  - Ships
   *      - Rewinder
   *          - Rewinds self
   *          - Manual save state (rewind to this point later)
   *          - Shadows
   *      - Possessor
   *          - Possess enemy ship with improved weaponry
   *      - Blinker
   *  - Stage design
   *  - Camera?
   *  - Effects/Shaders?
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
    public static readonly int ScreenWidth = 1920;
    public static readonly int ScreenHeight = 1080;

    private FrameCounter fc = new FrameCounter();
    private bool runningSlowly;

    private GameStateManager gameStateManager;

    //debug variables
    public static int frames;
    int slowFrames;
    public static Vector2 intersectPoint;
    public static int collisionChecksPerFrame;

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
      gameStateManager = new GameStateManager();
      gameStateManager.Push(new GameStateTitleScreen(gameStateManager));
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
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
      ++frames;
      collisionChecksPerFrame = 0;
      runningSlowly = gameTime.IsRunningSlowly;
      gameStateManager.Update(gameTime);

      base.Update(gameTime);
    }
    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.SlateGray);
      fc.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

      gameStateManager.Draw(spriteBatch);

      // draw debug info
      spriteBatch.Begin();
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "FPS: " + fc.CurrentFramesPerSecond.ToString(), new Vector2(0, 0), Color.Red);
      if (fc.CurrentFramesPerSecond < 30)
      {
        slowFrames++;
      }
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "slow: " + runningSlowly.ToString(), new Vector2(150, 0), Color.Red);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "slow frames: " + slowFrames.ToString(), new Vector2(150, 20), Color.Red);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "Memory:" + GC.GetTotalMemory(false) / 1024, new Vector2(150, 40), Color.White);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "Collision checks: " + (collisionChecksPerFrame).ToString(), new Vector2(0, 60), Color.Red);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "Game State: " + gameStateManager.Peek().ToString(), new Vector2(300, 0), Color.Red);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
