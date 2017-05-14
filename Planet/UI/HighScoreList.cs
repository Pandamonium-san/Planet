using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class HighScoreList : Transform
  {
    SpriteFont future48;
    List<HighScoreEntry> Scores;

    HighScoreEntry mostRecent;
    float a;

    public HighScoreList(Vector2 pos)
      : base(pos)
    {
      future48 = AssetManager.GetFont("future48");
      Scores = new List<HighScoreEntry>();
    }
    public void AddEntry(HighScoreEntry hse)
    {
      Scores.Add(hse);
      Scores.Sort(new HighScoreEntry.ByScore());
      mostRecent = hse;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      Text Wave = new Text(future48, " Wave", Pos + new Vector2(375, -20), Color.White, Text.Align.Left);
      Wave.Scale = 0.8f;
      Wave.Draw(spriteBatch);
      Text Score = new Text(future48, "  Score", Pos + new Vector2(-25, -20), Color.White, Text.Align.Left);
      Score.Scale = 0.8f;
      Score.Draw(spriteBatch);
      Text P1 = new Text(future48, "P1", Pos + new Vector2(-525, -20), Color.White, Text.Align.Left);
      P1.Scale = 0.8f;
      P1.Draw(spriteBatch);
      Text P2 = new Text(future48, "P2", Pos + new Vector2(-325, -20), Color.White, Text.Align.Left);
      P2.Scale = 0.8f;
      P2.Draw(spriteBatch);

      for (int i = 0; i < Scores.Count; i++)
      {
        if (i == 10)
          break;
        Color color = Color.White;
        if (Scores[i] == mostRecent)
          color = Color.Turquoise * (0.5f+(float)(Math.Sin(++a*0.1) + 1) / 4);
        Text T = new Text(future48, Scores.ElementAt(i).Wave.ToString(), Wave.Pos + new Vector2(110, 32 + 65 * (i + 1)), color, Text.Align.Center);
        T.Scale = 0.8f;
        T.Draw(spriteBatch);

        T = new Text(future48, Scores.ElementAt(i).TotalScore.ToString("D10"), Score.Pos + new Vector2(-55, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch);

        T = new Text(future48, Scores.ElementAt(i).Name1, P1.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch);

        T = new Text(future48, Scores.ElementAt(i).Name2, P2.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch);
      }
    }
  }
}
