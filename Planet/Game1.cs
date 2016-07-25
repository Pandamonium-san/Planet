﻿using System;
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
   *  - Change art
   *  - Destructible projectiles
   *  x Health
   *  - AI
   *      - FuSM?
   *      - Command queue?
   *      - Pathing
   *      - Targeting/Firing
   *  - Enemy spawns
   *  - Enemy variations
   *  - Projectile variations
   *      - Homing missiles
   *      - Explosions?
   *      - Projectile settings?
   *      - Projectile paths/patterns
   *      - Lasers
   *          x Laser sprites
   *          x Ray-Box detection
   *              - Hitscan weapons?
   *              - Thicker lasers?
   *  - Smaller hitboxes
   *  - Show hitbox while aiming
   *  - Game states
   *  - Rewinder
   *      - Manual save state (rewind to this point later)
   *      - Shadows
   *  - Possessor
   *  . Blinker
   *  - Score
   *  - UI
   *  - Stage design
   *  - Menu
   *  - Title screen
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

    public static Random rnd = new Random();
    private FrameCounter fc = new FrameCounter();
    private bool runningSlowly;

    public World world;
    private Player p1, p2;
    private AIController ai;
    private RewinderShip ship;
    public static Ship s;

    //debug variables
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

      s = new PumpkinShip(new Vector2(800, 500), world);
      ai = new AIController(s, world);
      world.PostGameObj(s);

      p1 = new Player(PlayerIndex.One, world);
      p2 = new Player(PlayerIndex.Two, world);
      p1.SetShip(ship);

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
      runningSlowly = gameTime.IsRunningSlowly;

      p1.Update(gameTime);
      ai.Update(gameTime);
      world.Update(gameTime);

      base.Update(gameTime);
    }
    int slowFrames;
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
      spriteBatch.Draw(AssetManager.GetTexture("Fill"), new Rectangle((int)intersectPoint.X, (int)intersectPoint.Y, 10, 10), Color.Red);
      collisionChecksPerFrame = 0;
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
