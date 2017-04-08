using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Menu
  {
    Text titleText;
    SelectionList mainMenu;
    public Menu()
    {
      titleText = new Text(AssetManager.GetFont("titleFont"), "Planet", new Vector2(Game1.ScreenWidth/2f, 200), Color.White);
      mainMenu = new SelectionList(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300));
      b.AddText(AssetManager.GetFont("uiFont"), "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400));
      b.AddText(AssetManager.GetFont("uiFont"), "Options");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500));
      b.AddText(AssetManager.GetFont("uiFont"), "Exit");
      mainMenu.AddSelection(2, b);
    }
    public void Update(GameTime gt)
    {
      if(InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Up, false) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Up, true))
      {
        mainMenu.Previous();
      }
      if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Down, false) && InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Down, true))
      {
        mainMenu.Next();
      }
    }
    public void Draw(SpriteBatch sb)
    {
      sb.Begin();
      titleText.Draw(sb);
      mainMenu.Draw(sb);
      sb.End();
    }
  }
}
