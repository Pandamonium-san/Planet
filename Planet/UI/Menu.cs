using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Menu
  {
    Text titleText;
    public Menu()
    {
      titleText = new Text(AssetManager.GetFont("titleFont"), "Planet", new Vector2(Game1.ScreenWidth/2f, 200), Color.White);
    }
    public void Update(GameTime gt)
    {

    }
    public void Draw(SpriteBatch sb)
    {
      sb.Begin();
      titleText.Draw(sb);
      sb.End();
    }
  }
}
