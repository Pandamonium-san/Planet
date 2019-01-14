﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Planet
{
  /// <summary>
  /// This is the main type for your game.
  /// </summary>
  public class Game1 : Game
  {
    public static readonly int ScreenWidth = 1920;
    public static readonly int ScreenHeight = 1080;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    private FrameCounter fc = new FrameCounter();
    private bool runningSlowly;

    private GameStateManager gameStateManager;

    //debug variables
    int frames;
    public static bool debugMode;
    public static int collisionChecksPerFrame;
    int slowFrames;
    SpriteFont debugFont;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = ScreenWidth;
      graphics.PreferredBackBufferHeight = ScreenHeight;
      Content.RootDirectory = "Content";
      InputHandler.InitializeBindings();
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = 1.0f;
      AssetManager.LoadContent(Content);
      debugFont = AssetManager.GetFont("font1");
      gameStateManager = new GameStateManager();
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
      InputHandler.Update();
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
      ++frames;
      //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Side) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Side, true) &&
      //InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Side) && InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Side, true))
      //{
      //  debugMode = !debugMode;
      //}
      if (debugMode)
      {
        //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Side, true) &&
        //  InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.A, true) &&
        //  InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.A, false))
        //{
        //  MediaPlayer.Volume = Math.Min(1.0f, MediaPlayer.Volume + 0.1f);
        //}
        //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Side, true) &&
        //  InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.A, true) &&
        //  InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.A, false))
        //{
        //  MediaPlayer.Volume = Math.Max(0.0f, MediaPlayer.Volume - 0.1f);
        //}
        //if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Side, true) &&
        //  InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.A, true) &&
        //  InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.A, false))
        //{
        //  SoundEffect.MasterVolume = Math.Min(1.0f, SoundEffect.MasterVolume + 0.1f);
        //}
        //if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Side, true) &&
        //  InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.A, true) &&
        //  InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.A, false))
        //{
        //  SoundEffect.MasterVolume = Math.Max(0.0f, SoundEffect.MasterVolume - 0.1f);
        //}
        collisionChecksPerFrame = 0;
        runningSlowly = gameTime.IsRunningSlowly;
      }
      Timer.UpdateGlobalTimers(gameTime);
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
