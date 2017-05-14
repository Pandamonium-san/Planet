using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  abstract class GameState
  {
    public bool UpdateEnabled
    {
      get { return updateEnabled; }
      set { updateEnabled = value; }
    }
    public bool DrawEnabled
    {
      get { return drawEnabled; }
      set { drawEnabled = value; }
    }
    public void FadeTransition(float time, Action action = null, bool fadeIn = true)
    {
      if (fadeTimer.Counting)
        return;
      fadeTimer = new Timer(time, action, true, false);
      this.fadeIn = fadeIn;
    }
    public virtual void Update(GameTime gameTime)
    {
      fadeTimer.Update(gameTime);
      if (fadeIn)
        a = 1 * (float)fadeTimer.Fraction;
      else
        a = 1 - 1 * (float)fadeTimer.Fraction;
    }
    public abstract void Entered();
    public abstract void Leaving();
    public abstract void Revealed();
    public abstract void Obscuring();
    public abstract void Draw(SpriteBatch spriteBatch);

    protected float a = 0.0f;
    protected Timer fadeTimer = new Timer(1.0, null, false);
    private bool updateEnabled = true;
    private bool drawEnabled = true;
    private bool fadeIn;
  }
}
