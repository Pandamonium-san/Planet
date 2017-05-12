using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateTitleScreen : GameState, IMenuGameState
  {
    private GameStateManager gsm;
    private SpriteFont future18;
    private Background bg;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Menu mainMenu;

    private Text bgmVol, sfxVol;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      gsm = gameStateManager;
      future18 = AssetManager.GetFont("future18");
      AudioManager.PlayBgm("PerituneMaterial_splash");
      bg = new Background(1.0f, 20, 100);
      bg.DriftSpeed = 20;
      bg.StarDensity = 100;

      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      mainMenu = new Menu(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300), "Play");
      b.AddText(future18, "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400), "Options");
      b.AddText(future18, "Music +Vol-");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500), "Credits");
      b.AddText(future18, "Sound +Vol-");
      mainMenu.AddSelection(2, b);

      cursor1 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), false);
      cursor1.color = Color.PaleTurquoise;
      mc1 = new MenuController(PlayerIndex.One, cursor1, this);

      cursor2 = new MenuCursor(mainMenu, AssetManager.GetTexture("grey_sliderRight"), true);
      cursor2.color = Color.CornflowerBlue;
      mc2 = new MenuController(PlayerIndex.Two, cursor2, this);

      bgmVol = new Text(future18, "", Vector2.Zero, Color.White, Text.Align.Left);
      sfxVol = new Text(future18, "", new Vector2(0, 30), Color.White, Text.Align.Left);
      bgmVol.Set("BGM: " + MediaPlayer.Volume.ToString("N2"));
      sfxVol.Set("SFX: " + SoundEffect.MasterVolume.ToString("N2"));
    }
    public void Confirm(MenuController mc)
    {
      switch (mc.GetSelected().Name)
      {
        case "Play":
          gsm.Push(new GameStateCharacterSelect(gsm));
          //gsm.Push(new GameStatePlaying(gsm));
          break;
        case "Options":
          MediaPlayer.Volume += 0.01f;
          bgmVol.Set("BGM: " + MediaPlayer.Volume.ToString("N2"));
          //gsm.Push(new GameStateCharacterSelect(gsm));
          //push options state to gsm
          break;
        case "Credits":
          SoundEffect.MasterVolume = Math.Min(SoundEffect.MasterVolume + 0.05f, 1.0f);
          sfxVol.Set("SFX: " + SoundEffect.MasterVolume.ToString("N2"));
          // ???
          break;
      }
    }
    public void Cancel(MenuController mc)
    {
      switch (mc.GetSelected().Name)
      {
        case "Options":
          MediaPlayer.Volume -= 0.01f;
          bgmVol.Set("BGM: " + MediaPlayer.Volume.ToString("N2"));
          //gsm.Push(new GameStateCharacterSelect(gsm));
          //push options state to gsm
          break;
        case "Credits":
          SoundEffect.MasterVolume = Math.Max(SoundEffect.MasterVolume - 0.05f, 0.0f);
          sfxVol.Set("SFX: " + SoundEffect.MasterVolume.ToString("N2"));
          // ???
          break;
      }
    }
    public override void Update(GameTime gameTime)
    {
      bg.Update(gameTime);
      mc1.Update(gameTime);
      mc2.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      bg.Draw(spriteBatch);
      cursor1.Draw(spriteBatch);
      cursor2.Draw(spriteBatch);
      titleText.Draw(spriteBatch);
      mainMenu.Draw(spriteBatch);
      bgmVol.Draw(spriteBatch);
      sfxVol.Draw(spriteBatch);
      spriteBatch.End();
    }
    public override void Entered()
    {

    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
      AudioManager.Resume();
    }
    public override void Obscuring()
    {
      AudioManager.Pause();
    }
  }
}
