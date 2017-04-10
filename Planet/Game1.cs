using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /*  TO DO
   *  - Menus
   *      - Menu controller class
   *  - Game states
   *  - UI
   *  - Score
   *  - Enemies
   *      - Waves/Levels
   *      - Enemy types
   *      - Enemy AI
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

  public enum GameState
  {
    Menu,
    Playing
  }

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

    private GameState gameState;
    private World world;
    private Menu menu;
    private Player p1, p2;
    private AIController ai;
    private RewinderShip ship;
    private BlinkerShip ship2;

    public static Ship s;

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
      world = new World();
      menu = new Menu();

      ship = new RewinderShip(new Vector2(500, 500), world);
      world.PostGameObj(ship);
      ship2 = new BlinkerShip(new Vector2(1000, 500), world);
      world.PostGameObj(ship2);

      s = new PumpkinShip(new Vector2(800, 500), world);
      ai = new AIController(s, world);
      world.PostGameObj(s);

      p1 = new Player(PlayerIndex.One);
      p2 = new Player(PlayerIndex.Two);
      p1.SetShip(ship);
      p2.SetShip(ship2);
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
      if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Start) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Start, true))
      {
        gameState = (GameState)(((int)gameState + 1) % 2);
      }
      switch (gameState)
      {
        case GameState.Menu:
          menu.Update(gameTime);
          p1.MenuUpdate(gameTime);
          p2.MenuUpdate(gameTime);
          break;
        case GameState.Playing:
          p1.Update(gameTime);
          p2.Update(gameTime);
          ai.Update(gameTime);
          world.Update(gameTime);
          break;
        default:
          break;
      }
      /*
       * p1.setmenu(menu);
       * menu = new Menu(p1, p2);
       * case State.Menu:
       *   menu.Update(gt);
       * break;
       */

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

      switch (gameState)
      {
        case GameState.Menu:
          menu.Draw(spriteBatch);
          break;
        case GameState.Playing:
          world.Draw(spriteBatch);
          break;
        default:
          break;
      }

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
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "World frames: " + world.Frames.ToString(), new Vector2(0, 80), Color.Red);
      spriteBatch.DrawString(AssetManager.GetFont("font1"), "Game State: " + gameState.ToString(), new Vector2(300, 0), Color.Red);
      spriteBatch.Draw(AssetManager.GetTexture("pixel"), new Rectangle((int)intersectPoint.X, (int)intersectPoint.Y, 10, 10), Color.Red);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
