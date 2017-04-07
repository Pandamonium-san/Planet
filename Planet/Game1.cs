using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /*  TO DO
   *  - WEAPON SWITCHING
   *  - RESTRUCTURE SHIP CLASSES
   *  - Enemies
   *      x Spawn logic <-- make spawns stay the same after rewinding (might consider reworking how rewind works code wise)
   *          - Waves/Levels
   *          x Timing
   *      - Enemy types
   *          - Weapons/Stats
   *          - Behaviors
   *      - Enemy AI
   *          x Command queue
   *          x Targeting/Firing
   *          - Flight patterns
   *          - Bullet patterns
   *  - Weapons
   *      - Homing missiles
   *      - Explosions?
   *      - Projectile settings?
   *      - Projectile paths/patterns
   *      - Lasers
   *          x Laser sprites
   *          x Ray-Box detection
   *              x Hitscan weapons?
   *              x Thicker lasers?
   *  - Ships
   *      - Rewinder
   *          //CUT(Too resource-intensive) - Rewind everything, timers, world timers, everything (base everything on global time object?)
   *          - Rewinds only self
   *          - Manual save state (rewind to this point later)
   *          - Shadows
   *      - Possessor
   *      - Blinker
   *  - Game states
   *  - UI
   *  - Stage design
   *  - Score
   *  - Menus
   *      - Menu controller class
   *  - Title screen
   *  - Camera?
   *  - Destructible projectiles?
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

    public World world;
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

      //go = new BlinkerShip(new Vector2(500, 500));
      ship = new RewinderShip(new Vector2(500, 500), world);
      world.PostGameObj(ship);
      ship2 = new BlinkerShip(new Vector2(1000, 500), world);
      world.PostGameObj(ship2);

      s = new PumpkinShip(new Vector2(800, 500), world);
      ai = new AIController(s, world);
      world.PostGameObj(s);

      p1 = new Player(PlayerIndex.One, world);
      p2 = new Player(PlayerIndex.Two, world);
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

      p1.Update(gameTime);
      p2.Update(gameTime);
      ai.Update(gameTime);
      world.Update(gameTime);
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
      GraphicsDevice.Clear(Color.CornflowerBlue);
      fc.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

      world.Draw(spriteBatch);

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
      spriteBatch.Draw(AssetManager.GetTexture("Fill"), new Rectangle((int)intersectPoint.X, (int)intersectPoint.Y, 10, 10), Color.Red);
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
