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
    private StylableText titleText;

    public GameStateTitleScreen(GameStateManager gameStateManager)
    {
      this.gsm = gameStateManager;
      settings = new GameSettings();

      bg = new Background(1.0f, 20, 100);
      bg.DriftSpeed = 20;
      bg.StarDensity = 100;

      titleText = new StylableText(AssetManager.GetFont("title"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), 150, Color.White);

      start1 = new Text(AssetManager.GetFont("future48"), "Press start", new Vector2(Game1.ScreenWidth / 4, Game1.ScreenHeight - 100), Color.White);
      start1.Scale = 0.8f;
      start2 = new Text(AssetManager.GetFont("future48"), "Press start", new Vector2(Game1.ScreenWidth / 4 * 3, Game1.ScreenHeight - 100), Color.White);
      start2.Scale = 0.8f;
    }
    public override void Update(GameTime gameTime)
    {
      bg.Update(gameTime);
      UpdateTitleTextEffects(gameTime);
      CheckInputAndUpdateText(gsm.P1, start1, gameTime);
      CheckInputAndUpdateText(gsm.P2, start2, gameTime);
    }
    public void Join(Player player)
    {
      GameStateMenuMain menu = new GameStateMenuMain(gsm);
      menu.Join(player);
      gsm.Push(menu);
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
    private void CheckInputAndUpdateText(Player player, Text start, GameTime gt)
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
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      bg.Draw(spriteBatch);
      titleText.Draw(spriteBatch);
      start1.Draw(spriteBatch);
      start2.Draw(spriteBatch);
      spriteBatch.End();
    }
    public override void Entered()
    {
      AudioManager.PlayBgm("PerituneMaterial_splash");
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
      if (gsm.Settings.startGame)
      {
        AudioManager.PlayBgm("Kubbi - Firelight", 0.02f);
        bg.DriftSpeed = 75;
        bg.StarDensity = 150;
        titleText.Visible = false;
        gsm.Push(new GameStatePlaying(gsm));

        start1 = new Text(AssetManager.GetFont("future18"), "Press start", new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight - 50), Color.White);
        start1.Scale = 1.2f;
        start2 = new Text(AssetManager.GetFont("future18"), "Press start", new Vector2(Game1.ScreenWidth / 6 * 5, Game1.ScreenHeight - 50), Color.White);
        start2.Scale = 1.2f;
      }
    }
    public override void Obscuring()
    {
    }
  }
}
