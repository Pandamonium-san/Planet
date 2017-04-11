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
      titleText = new Text(AssetManager.GetFont("future48"), "Planet", new Vector2(Game1.ScreenWidth / 2f, 200), Color.White);
      mainMenu = new SelectionList(3);

      Button b = new Button(new Vector2(Game1.ScreenWidth / 2f, 300));
      b.AddText(AssetManager.GetFont("future18"), "Play");
      mainMenu.AddSelection(0, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 400));
      b.AddText(AssetManager.GetFont("future18"), "Options");
      mainMenu.AddSelection(1, b);

      b = new Button(new Vector2(Game1.ScreenWidth / 2f, 500));
      b.AddText(AssetManager.GetFont("future18"), "Exit");
      mainMenu.AddSelection(2, b);
    }
    public void Confirm()
    {

    }
    public void Cancel()
    {

    }
    public void NextSelection()
    {
      mainMenu.Next();
    }
    public void PreviousSelection()
    {
      mainMenu.Previous();
    }
    public void Update(GameTime gt)
    {

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
