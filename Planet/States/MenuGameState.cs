using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  abstract class MenuGameState : GameState
  {
    protected float a = 0.0f;
    protected Timer fadeTimer = new Timer(1.0, null, false);
    private bool fadeIn;

    public abstract void Confirm(MenuController pi);
    public abstract void Cancel(MenuController pi);
    public void FadeTransition(float time, Action action = null, bool fadeIn = true)
    {
      if (fadeTimer.Counting)
        return;
      fadeTimer = new Timer(time, action, true, false);
      this.fadeIn = fadeIn;
    }
    public override void Update(GameTime gameTime)
    {
      fadeTimer.Update(gameTime);
      if (fadeIn)
        a = 1 * (float)fadeTimer.Fraction;
      else
        a = 1 - 1 * (float)fadeTimer.Fraction;
    }
  }
}
