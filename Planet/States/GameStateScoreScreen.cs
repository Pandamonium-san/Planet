using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateScoreScreen : MenuGameState
  {
    private GameStateManager gsm;
    private HighScoreDisplay hsd;
    private Menu menu;
    private MenuCursor cursor1, cursor2;
    private MenuController mc1, mc2;
    private Text titleText;
    private Sprite overlay;

    public GameStateScoreScreen(GameStateManager gsm, HighScoreEntry entry) : base()
    {
      this.gsm = gsm;
      if (entry != null)
      {
        gsm.Highscores.AddEntry(entry);
        gsm.Highscores.SaveHighScores();
      }
      hsd = new HighScoreDisplay(gsm.Highscores, new Vector2(Game1.ScreenWidth / 2, 150), entry);

      titleText = new Text(AssetManager.GetFont("future48"), "Highscores", new Vector2(Game1.ScreenWidth / 2f, 75), Color.White);

      menu = new Menu(1);
      SelectionBox sb = new SelectionBox(AssetManager.GetTexture("blueButton05"), new Vector2(Game1.ScreenWidth / 2f, 950), "Continue");
      sb.SetText(new Text(AssetManager.GetFont("future18"), "Continue", sb.Pos, Color.White));
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

    public override void Confirm(PlayerController pi)
    {
      if (fadeTimer.Counting)
        return;
      FadeTransition(1.0f, gsm.Pop, false);
      AudioManager.PlaySound("boop");
    }
    public override void Cancel(PlayerController pi)
    {
      FadeTransition(1.0f, gsm.Pop, false);
      AudioManager.PlaySound("boop");
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
      hsd.Draw(spriteBatch, a);
      spriteBatch.End();
    }

    public override void Entered()
    {
      FadeTransition(0.5f);
    }

    public override void Leaving()
    {
    }

    public override void Obscuring()
    {
    }

    public override void Revealed()
    {
    }
  }
}
