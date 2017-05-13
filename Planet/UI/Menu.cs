using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Menu
  {
    public int Length { get { return buttons.Length; } }
    SelectionBox[] buttons;
    public Menu(int size)
    {
      buttons = new SelectionBox[size];
    }
    public SelectionBox GetButton(int index)
    {
      return buttons[index];
    }
    public void AddSelection(int index, SelectionBox button)
    {
      buttons[index] = button;
    }
    public void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      for (int i = 0; i < buttons.Length; i++)
      {
        buttons[i].Draw(spriteBatch, a);
      }
    }
    public static Menu Main()
    {
      Menu menu = new Menu(3);
      SpriteFont future18 = AssetManager.GetFont("future18");
      Texture2D tex = AssetManager.GetTexture("blue_Button05");

      SelectionBox b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 450), "Play");
      b.alpha = 0.7f;
      b.SetText(future18, "Play");
      b.color = new Color(200, 200, 200);
      menu.AddSelection(0, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 550), "Options");
      b.alpha = 0.7f;
      b.SetText(future18, "Options");
      b.color = new Color(200, 200, 200);
      menu.AddSelection(1, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 650), "Credits");
      b.alpha = 0.7f;
      b.SetText(future18, "Credits");
      b.color = new Color(200, 200, 200);
      menu.AddSelection(2, b);
      return menu;
    }
    public static Menu CharSelect()
    {
      Menu menu = new Menu(3);
      SpriteFont future18 = AssetManager.GetFont("future18");
      Texture2D tex = AssetManager.GetTexture("blue_Button05");

      SelectionBox b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 300), "Rewinder");
      b.SetText(future18, "Rewinder");
      menu.AddSelection(0, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 400), "Blinker");
      b.SetText(future18, "Blinker");
      menu.AddSelection(1, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 500), "Possessor");
      b.SetText(future18, "Possessor");
      menu.AddSelection(2, b);
      return menu;
    }
    public static Menu Pause()
    {
      Menu menu = new Menu(2);
      SpriteFont future18 = AssetManager.GetFont("future18");
      Texture2D tex = AssetManager.GetTexture("blue_Button05");

      SelectionBox b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 300), "Resume");
      b.SetText(future18, "Resume");
      menu.AddSelection(0, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 400), "Main Menu");
      b.SetText(future18, "Main Menu");
      menu.AddSelection(1, b);
      return menu;
    }
  }
}
