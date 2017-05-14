using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class GameStateInputName : MenuGameState
  {
    private GameStateManager gsm;
    private SpriteFont future18, future48;

    private NameInput ni1, ni2;
    private NameInputController nic1, nic2;
    private Text titleText;
    private Text score1, score2, totalScore;
    private Sprite overlay;
    private bool sendScore;
    private int wave, p1Score, p2Score, total;

    public GameStateInputName(GameStateManager gameStateManager, int wave)
    {
      this.gsm = gameStateManager;
      future18 = AssetManager.GetFont("future18");
      future48 = AssetManager.GetFont("future48");

      titleText = new Text(future48, "GAME OVER", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      titleText.Scale = 1.5f;

      this.wave = wave;
      p1Score = ((int)gsm.P1.Score) / 10 * 10;
      p2Score = ((int)gsm.P2.Score) / 10 * 10;
      total = p1Score + p2Score;

      if (gsm.P1.Joined)
      {
        score1 = new Text(future48, p1Score.ToString("D10"), new Vector2(Game1.ScreenWidth / 3, 500), gsm.P1.Color);
        score1.Scale = 0.7f;
        ni1 = new NameInput(AssetManager.GetFont("title"), score1.Pos + new Vector2(0, 100), 70, gsm.P1.Color, 0.5f);
        nic1 = new NameInputController(this, ni1, gsm.P1);
      }
      if (gsm.P2.Joined)
      {
        score2 = new Text(future48, p2Score.ToString("D10"), new Vector2(Game1.ScreenWidth / 3 * 2, 500), gsm.P2.Color);
        score2.Scale = 0.7f;
        ni2 = new NameInput(AssetManager.GetFont("title"), score2.Pos + new Vector2(0, 100), 70, gsm.P2.Color, 0.5f);
        nic2 = new NameInputController(this, ni2, gsm.P2);
      }
      totalScore = new Text(future48, total.ToString("D10"), new Vector2(Game1.ScreenWidth / 2, 300), Color.White);
      totalScore.Scale = 1.0f;

      if (((int)gsm.P1.Score) / 10 * 10 + ((int)gsm.P2.Score) / 10 * 10 > GetLowestScore(gsm.Highscores))
      {
        sendScore = true;
      }

      overlay = new Sprite(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), AssetManager.Pixel);
      overlay.Scale = 2000;
      overlay.color = Color.Black;
      overlay.alpha = 0.5f;
    }
    public override void Confirm(PlayerController mc)
    {
      mc.IsActive = false;
      ((NameInputController)mc).NameInput.HideCursor = true;
      if ((nic1 == null || !nic1.IsActive) && (nic2 == null || !nic2.IsActive) || !sendScore)
      {
        FadeTransition(1.0f, ToScoreScreen, false);
      }
      AudioManager.PlaySound("boop");
    }
    public override void Cancel(PlayerController mc)
    {
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (nic1 != null)
        nic1.Update(gameTime);
      if (nic2 != null)
        nic2.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      overlay.Draw(spriteBatch, a);

      if (sendScore)
      {
        if (ni1 != null)
          ni1.Draw(spriteBatch, a);
        if (ni2 != null)
          ni2.Draw(spriteBatch, a);
      }

      titleText.Draw(spriteBatch, a);
      if (score1 != null)
        score1.Draw(spriteBatch, a);
      if (score2 != null)
        score2.Draw(spriteBatch, a);
      totalScore.Draw(spriteBatch, a);
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
      gsm.Pop();
    }
    public override void Obscuring()
    {
      UpdateEnabled = false;
      DrawEnabled = false;
    }
    private int GetLowestScore(HighScoreList scores)
    {
      if (scores.Count < 10)
        return int.MinValue;
      return scores.Last.TotalScore;
    }
    private void ToScoreScreen()
    {
      HighScoreEntry entry = null;
      if (sendScore)
      {
        string p1Name = "", p2Name = "";
        if (ni1 != null)
          p1Name = ni1.ToString();
        if (ni2 != null)
          p2Name = ni2.ToString();
        entry = new HighScoreEntry(wave, p1Name, p1Score, gsm.P1.SelectedShip, p2Name, p2Score, gsm.P2.SelectedShip);
      }
      gsm.Push(new GameStateScoreScreen(gsm, entry));
    }
  }
}
