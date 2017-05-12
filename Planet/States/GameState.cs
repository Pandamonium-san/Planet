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
    public abstract void Entered();
    public abstract void Leaving();
    public abstract void Revealed();
    public abstract void Obscuring();
    public abstract void Update(GameTime gt);
    public abstract void Draw(SpriteBatch spriteBatch);

    private bool updateEnabled = true;
    private bool drawEnabled = true;
  }
}
