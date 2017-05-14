using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class HighScoreDisplay : Transform
  {
    SpriteFont future48, future48nk;
    List<HighScoreEntry> Scores;

    HighScoreEntry mostRecent;
    float x;

    public HighScoreDisplay(Vector2 pos)
      : base(pos)
    {
      future48 = AssetManager.GetFont("future48");
      future48nk = AssetManager.GetFont("future48_nk");
      Scores = new List<HighScoreEntry>();
    }
    public int GetLowestScore()
    {
      if (Scores.Count == 0)
        return 0;
      return Scores.Last().TotalScore;
    }
    public void AddEntry(HighScoreEntry hse)
    {
      Scores.Add(hse);
      Scores.Sort(new HighScoreEntry.ByScore());
      mostRecent = hse;
    }
    public void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      Text Wave = new Text(future48, " Wave", Pos + new Vector2(375, -20), Color.White, Text.Align.Left);
      Wave.Scale = 0.8f;
      Wave.Draw(spriteBatch, a);
      Text Score = new Text(future48, "  Score", Pos + new Vector2(-25, -20), Color.White, Text.Align.Left);
      Score.Scale = 0.8f;
      Score.Draw(spriteBatch, a);
      Text P1 = new Text(future48, "P1", Pos + new Vector2(-525, -20), Color.White, Text.Align.Left);
      P1.Scale = 0.8f;
      P1.Draw(spriteBatch, a);
      Text P2 = new Text(future48, "P2", Pos + new Vector2(-325, -20), Color.White, Text.Align.Left);
      P2.Scale = 0.8f;
      P2.Draw(spriteBatch, a);

      for (int i = 0; i < Scores.Count; i++)
      {
        if (i == 10)
          break;
        Color color = Color.White;
        if (Scores[i] == mostRecent)
          color = Color.Turquoise * (0.5f + (float)(Math.Sin(++x * 0.1) + 1) / 4);
        Text T = new Text(future48nk, Scores.ElementAt(i).Wave.ToString(), Wave.Pos + new Vector2(110, 32 + 65 * (i + 1)), color, Text.Align.Center);
        T.Scale = 0.8f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, Scores.ElementAt(i).TotalScore.ToString("D10"), Score.Pos + new Vector2(-55, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, Scores.ElementAt(i).Name1, P1.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, Scores.ElementAt(i).Name2, P2.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);
      }
    }
  }
}
