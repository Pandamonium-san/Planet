using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateGameOver : MenuGameState
  {
    private GameStateManager gsm;
    private SpriteFont future18;
    private HighScoreList hsl;

    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Menu menu;
    private Sprite overlay;

    public GameStateGameOver(GameStateManager gameStateManager)
    {
      this.gsm = gameStateManager;
      future18 = AssetManager.GetFont("future18");
      hsl = new HighScoreList(new Vector2(Game1.ScreenWidth / 2, 150));
      hsl.AddEntry(new HighScoreEntry(14, "QQQ", ((int)gsm.P1.Score + 75000) / 10 * 10, "Rewinder", "WWW", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(9, "III", ((int)gsm.P1.Score + 5000) / 10 * 10, "Rewinder", "III", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(8, "ASD", ((int)gsm.P1.Score + 500000) / 10 * 10, "Rewinder", "ASD", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(77, "LFE", ((int)gsm.P1.Score + 1000) / 10 * 10, "Rewinder", "FDA", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(0, "ZXQ", ((int)gsm.P1.Score + 2000) / 10 * 10, "Rewinder", "BIS", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(14, "QQQ", ((int)gsm.P1.Score + 75000) / 10 * 10, "Rewinder", "WWW", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(9, "III", ((int)gsm.P1.Score + 5000) / 10 * 10, "Rewinder", "III", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(8, "ASD", ((int)gsm.P1.Score + 500000) / 10 * 10, "Rewinder", "ASD", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(77, "LFE", ((int)gsm.P1.Score + 1000) / 10 * 10, "Rewinder", "FDA", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(0, "ZXQ", ((int)gsm.P1.Score + 2000) / 10 * 10, "Rewinder", "BIS", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(14, "QQQ", ((int)gsm.P1.Score + 75000) / 10 * 10, "Rewinder", "WWW", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(9, "III", ((int)gsm.P1.Score + 5000) / 10 * 10, "Rewinder", "III", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(8, "ASD", ((int)gsm.P1.Score + 500000) / 10 * 10, "Rewinder", "ASD", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(77, "LFE", ((int)gsm.P1.Score + 1000) / 10 * 10, "Rewinder", "FDA", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(0, "ZXQ", ((int)gsm.P1.Score + 2000) / 10 * 10, "Rewinder", "BIS", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(14, "QQQ", ((int)gsm.P1.Score + 75000) / 10 * 10, "Rewinder", "WWW", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(9, "III", ((int)gsm.P1.Score + 5000) / 10 * 10, "Rewinder", "III", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(8, "ASD", ((int)gsm.P1.Score + 500000) / 10 * 10, "Rewinder", "ASD", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(77, "LFE", ((int)gsm.P1.Score + 1000) / 10 * 10, "Rewinder", "FDA", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));
      hsl.AddEntry(new HighScoreEntry(0, "ZXQ", ((int)gsm.P1.Score + 502000) / 10 * 10, "Rewinder", "BIS", ((int)gsm.P2.Score) / 10 * 10, "Blinker"));

      titleText = new Text(AssetManager.GetFont("future48"), "Highscores", new Vector2(Game1.ScreenWidth / 2f, 75), Color.White);
      titleText.Scale = 1.2f;
      menu = new Menu(1);
      SelectionBox sb = new SelectionBox(AssetManager.GetTexture("blue_button05"), new Vector2(Game1.ScreenWidth / 2, 950), "MainMenu");
      sb.SetText(new Text(future18, "Continue", sb.Pos, Color.White));
      sb.Scale = 1.5f;
      menu.AddSelection(0, sb);

      if (gsm.P1.Joined)
      {
        cursor1 = new MenuCursor(menu, gsm.P1.Color);
        mc1 = new MenuController(gsm.P1, cursor1, this);
      }
      if (gsm.P2.Joined)
      {
        cursor2 = new MenuCursor(menu, gsm.P2.Color);
        mc2 = new MenuController(gsm.P2, cursor2, this);
      }

      overlay = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), AssetManager.Pixel);
      overlay.Scale = 2000;
      overlay.color = Color.Black;
      overlay.alpha = 0.5f;
    }
    public override void Confirm(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      switch (mc.GetSelected().Name)
      {
        case "MainMenu":
          FadeTransition(0.5f, gsm.Reset, false);
          break;
      }
      AudioManager.PlaySound("boop");
    }
    public override void Cancel(MenuController mc)
    {
      if (fadeTimer.Counting)
        return;
      FadeTransition(0.5f, gsm.Reset, false);
      AudioManager.PlaySound("boop2");
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      foreach (SelectionBox sb in menu.GetBoxes())
        sb.alpha = 0.6f;
      if (mc1 != null)
      {
        mc1.Update(gameTime);
        mc1.GetSelected().alpha = 0.9f;
      }
      if (mc2 != null)
      {
        mc2.Update(gameTime);
        mc2.GetSelected().alpha = 0.9f;
      }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      overlay.Draw(spriteBatch, a);
      if (cursor1 != null)
        cursor1.Draw(spriteBatch, a);
      if (cursor2 != null)
        cursor2.Draw(spriteBatch, a);
      titleText.Draw(spriteBatch, a);
      menu.Draw(spriteBatch, a);
      hsl.Draw(spriteBatch);
      spriteBatch.End();
    }
    public override void Entered()
    {
      FadeTransition(1.0f);
    }
    public override void Leaving()
    {
    }
    public override void Revealed()
    {
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
      DrawEnabled = false;
    }
  }
}
