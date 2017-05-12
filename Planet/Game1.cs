using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /*  TO DO
   *  - Add sound
   *      - find more fitting bgm?
   *  - Design UI
   *      - Title screen
   *      - Pause menu
   *      - Character select
   *      - Highscore list
   *      - Credits
   *  - Fancify the background
   *  - Make proper weapons
   *      - Missiles?
   *  - Enemies
   *      - VS Mode?
   *      - Waves/Levels?
   *      - More enemy types
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
    public static bool debugMode;
    public static int frames;
    public static Vector2 intersectPoint;
    public static int collisionChecksPerFrame;
    int slowFrames;
    SpriteFont debugFont;

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
      Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = 0.5f;
      AssetManager.LoadContent(Content);
      gameStateManager = new GameStateManager();
      gameStateManager.Push(new GameStateTitleScreen(gameStateManager));
      debugFont = AssetManager.GetFont("font1");
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
      if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Side) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Side, true) &&
      InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Side) && InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Side, true))
      {
        debugMode = !debugMode;
      }
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
      GraphicsDevice.Clear(Color.Black);
      fc.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

      gameStateManager.Draw(spriteBatch);

      // draw debug info
      if (debugMode)
      {
        spriteBatch.Begin();
        spriteBatch.DrawString(debugFont, "FPS: " + fc.CurrentFramesPerSecond.ToString(), new Vector2(0, 0), Color.Red);
        if (fc.CurrentFramesPerSecond < 30)
        {
          slowFrames++;
        }
        spriteBatch.DrawString(debugFont, "slow: " + runningSlowly.ToString(), new Vector2(150, 0), Color.Red);
        spriteBatch.DrawString(debugFont, "slow frames: " + slowFrames.ToString(), new Vector2(150, 20), Color.Red);
        spriteBatch.DrawString(debugFont, "Memory:" + GC.GetTotalMemory(false) / 1024, new Vector2(150, 40), Color.White);
        spriteBatch.DrawString(debugFont, "Collision checks: " + (collisionChecksPerFrame).ToString(), new Vector2(0, 60), Color.Red);
        spriteBatch.DrawString(debugFont, "Game State: " + gameStateManager.Peek().ToString(), new Vector2(300, 0), Color.Red);
        spriteBatch.End();
      }
      base.Draw(gameTime);
    }
  }
}
