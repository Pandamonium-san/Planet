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
    HighScoreList scores;
    HighScoreEntry recent;
    float x;

    public HighScoreDisplay(HighScoreList scores, Vector2 pos, HighScoreEntry recent = null)
      : base(pos)
    {
      future48 = AssetManager.GetFont("future48");
      future48nk = AssetManager.GetFont("future48_nk");
      this.scores = scores;
      this.recent = recent;
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

      for (int i = 0; i < scores.Count; i++)
      {
        if (i == 10)
          break;
        Color color = Color.White;
        if (scores[i] == recent)
          color = Color.Turquoise * (0.5f + (float)(Math.Sin(++x * 0.1) + 1) / 4);
        Text T = new Text(future48nk, scores[i].Wave.ToString(), Wave.Pos + new Vector2(110, 32 + 65 * (i + 1)), color, Text.Align.Center);
        T.Scale = 0.8f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, scores[i].TotalScore.ToString("D10"), Score.Pos + new Vector2(-55, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, scores[i].Name1, P1.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);

        T = new Text(future48nk, scores[i].Name2, P2.Pos + new Vector2(0, 5 + 65 * (i + 1)), color, Text.Align.Left);
        T.Scale = 0.7f;
        T.Draw(spriteBatch, a);
      }
    }
  }
}
