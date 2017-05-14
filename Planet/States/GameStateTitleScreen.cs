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
  class GameStateTitleScreen : GameState, IJoinable
  {
    private GameStateManager gsm;
    private Background bg;
    private GameSettings settings;
    private Text start1, start2;
    private Sprite sb1, sb2;
    private StylableText titleText;

    private bool showHowTo = true;
    private Sprite howTo;
    private Timer howToAlphaTimer;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      this.gsm = gameStateManager;
      settings = new GameSettings();

      bg = new Background(1.0f, 20, 100);
      bg.DriftSpeed = 20;
      bg.StarDensity = 500;

      titleText = new StylableText(AssetManager.GetFont("title"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), 150, Color.White);

      start1 = new Text(AssetManager.GetFont("future48"), "Press ", new Vector2(Game1.ScreenWidth / 4 - 50, Game1.ScreenHeight - 100), Color.White);

      sb1 = new Sprite(start1.Pos + new Vector2(100, 0), AssetManager.GetTexture("p1start"));
      sb1.Parent = start1;
      start1.Scale = 0.5f;
      sb1.Scale = 0.7f;

      start2 = new Text(AssetManager.GetFont("future48"), "Press ", new Vector2(Game1.ScreenWidth / 4 * 3, Game1.ScreenHeight - 100), Color.White);
      sb2 = new Sprite(start2.Pos + new Vector2(100, 0), AssetManager.GetTexture("p2start"));
      sb2.Parent = start2;
      start2.Scale = 0.5f;
      sb2.Scale = 0.7f;

      howTo = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight - 80), AssetManager.GetTexture("howToplay"));
      howTo.alpha = 0.0f;
      howTo.Scale = 0.8f;
      showHowTo = false;
      howToAlphaTimer = new Timer(1.0, null, false);
      howToAlphaTimer.ForceFinish();
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if ((InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Side) &&
        InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Side, true)) ||
        (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Side) &&
        InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Side, true)) &&
        !howToAlphaTimer.Counting)
      {
        showHowTo = !showHowTo;
        howToAlphaTimer.Start(1.0f);
      }
      howToAlphaTimer.Update(gameTime);
      if (showHowTo)
        howTo.alpha = (float)howToAlphaTimer.Fraction;
      else
        howTo.alpha = 1 - (float)howToAlphaTimer.Fraction;

      bg.Update(gameTime);
      UpdateTitleTextEffects(gameTime);
      CheckInputAndUpdateText(gsm.P1, start1, sb1, gameTime);
      CheckInputAndUpdateText(gsm.P2, start2, sb2, gameTime);
      if (gsm.Settings.startGame && MediaPlayer.State == MediaState.Stopped)
      {
        int i = Utility.RandomInt(0, 2);
        if (i == 0)
          AudioManager.PlayBgm("Aethernaut_Shine_Get", 0.025f);
        else
          AudioManager.PlayBgm("Kubbi_Firelight", 0.03f);
        MediaPlayer.IsRepeating = false;
      }
    }
    public void Join(Player player)
    {
      GameStateMenuMain menu = new GameStateMenuMain(gsm);
      menu.Join(player);
      gsm.Push(menu);
      showHowTo = true;
      howToAlphaTimer.Start(2.0f);
    }
    private void UpdateTitleTextEffects(GameTime gameTime)
    {
      if (!titleText.Visible)
        return;
      Text[] array = titleText.GetArray();
      for (int i = 0; i < array.Length; i++)
      {
        float v = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * AudioManager.SplashSinCycle + i * 0.3f);
        array[i].alpha = 0.75f + 0.25f * v;
        array[i].color = new Color(
            0.1f + 0.15f * (float)Math.Sin(0.2f * i),
            0.3f + 0.2f * v + 0.2f * (float)Math.Sin(0.4f * i),
            0.7f + 0.15f * (float)Math.Sin(0.6f * i));
      }
    }
    private void CheckInputAndUpdateText(Player player, Text start, Sprite sb, GameTime gt)
    {
      if (!(gsm.Peek() is IJoinable))
        return;
      if (!player.Joined && InputHandler.IsButtonDown(player.Index, PlayerInput.Start))
      {
        ((IJoinable)gsm.Peek()).Join(player);
        player.Joined = true;
        AudioManager.PlaySound("whoosh", 0.5f);
      }
      if (!player.Joined)
        start.alpha = 0.5f + 0.5f * (float)Math.Sin(gt.TotalGameTime.TotalSeconds * AudioManager.SplashSinCycle * 4);
      else
        start.alpha = 0;
      sb.alpha = start.alpha;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      bg.Draw(spriteBatch, a);
      titleText.Draw(spriteBatch, a);
      howTo.Draw(spriteBatch, a);

      start1.Draw(spriteBatch, a);
      sb1.Draw(spriteBatch, a);
      start2.Draw(spriteBatch, a);
      sb2.Draw(spriteBatch, a);

      spriteBatch.End();
    }
    public override void Entered()
    {
      FadeTransition(5.0f);
      AudioManager.PlayBgm("PerituneMaterial_Splash", 0.05f);
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
      if (gsm.Settings.startGame)
      {
        AudioManager.PlayBgm("Kubbi_Firelight", 0.03f);
        MediaPlayer.IsRepeating = false;
        bg.DriftSpeed = 75;
        bg.StarDensity = 150;
        titleText.Visible = false;
        gsm.Push(new GameStatePlaying(gsm));

        start1 = new Text(AssetManager.GetFont("future18"), "Press", new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight - 75), Color.White);
        start1.Scale = 1.2f;
        sb1.Scale = 0.5f;
        sb1.Parent = start1;
        sb1.LocalPos = new Vector2(90, 0);

        start2 = new Text(AssetManager.GetFont("future18"), "Press", new Vector2(Game1.ScreenWidth / 6 * 5, Game1.ScreenHeight - 75), Color.White);
        start2.Scale = 1.2f;
        sb2.Scale = 0.5f;
        sb2.Parent = start2;
        sb2.LocalPos = new Vector2(90, 0);
      }
    }
    public override void Obscuring()
    {
    }
  }
}
