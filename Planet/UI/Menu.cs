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
    public int Length { get { return selections.Length; } }
    SelectionBox[] selections;
    public Menu(int size)
    {
      selections = new SelectionBox[size];
    }
    public SelectionBox GetButton(int index)
    {
      return selections[index];
    }
    public SelectionBox[] GetBoxes()
    {
      return selections;
    }
    public void AddSelection(int index, SelectionBox button)
    {
      selections[index] = button;
    }
    public void Draw(SpriteBatch spriteBatch, float a = 1.0f)
    {
      for (int i = 0; i < selections.Length; i++)
      {
        selections[i].Draw(spriteBatch, a);
      }
    }
    public static Menu Main()
    {
      Menu menu = new Menu(3);
      SpriteFont future18 = AssetManager.GetFont("future18");
      Texture2D tex = AssetManager.GetTexture("blue_Button05");

      SelectionBox b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 400), "Play");
      b.Scale = 1.3f;
      b.alpha = 0.7f;
      b.SetText(future18, "Play");
      b.color = new Color(200, 200, 200);
      menu.AddSelection(0, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 525), "Highscore");
      b.Scale = 1.3f;
      b.alpha = 0.7f;
      b.SetText(future18, "Highscore");
      b.color = new Color(200, 200, 200);
      menu.AddSelection(1, b);

      b = new SelectionBox(tex, new Vector2(Game1.ScreenWidth / 2f, 650), "Credits");
      b.Scale = 1.3f;
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

      SelectionBox b = new SelectionBox(AssetManager.GetTexture(@"ships\blue\spaceShips_002"), new Vector2(Game1.ScreenWidth / 2f - 250, Game1.ScreenHeight / 2f), "Rewinder");
      menu.AddSelection(0, b);

      b = new SelectionBox(AssetManager.GetTexture(@"ships\blue\spaceShips_001"), new Vector2(Game1.ScreenWidth / 2f, Game1.ScreenHeight / 2f), "Blinker");
      menu.AddSelection(1, b);

      b = new SelectionBox(AssetManager.GetTexture(@"ships\blue\spaceShips_009"), new Vector2(Game1.ScreenWidth / 2f + 250, Game1.ScreenHeight / 2f), "Possessor");
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
