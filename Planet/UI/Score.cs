using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Score
  {
    public Text text;
    private Player player;
    private int oldScore;
    private Timer glowTimer;

    public Score(Player player, Text text)
    {
      this.text = text;
      this.player = player;
      text.alpha = 0.70f;
      text.Scale = 1.2f;
      glowTimer = new Timer(0.15f);
    }
    public void Update(GameTime gameTime)
    {
      int rounded = ((int)player.Score) / 10 * 10;
      text.Set(rounded.ToString("D10"));
      glowTimer.Update(gameTime);
      if (rounded != oldScore)
      {
        glowTimer.Start();
      }
      if (glowTimer.Counting)
      {
        text.alpha = 1.0f - (float)glowTimer.Fraction * 0.30f;
        text.Scale = 1.35f - (float)glowTimer.Fraction * 0.15f;
      }
      oldScore = rounded;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      text.Draw(spriteBatch);
    }
  }
}
