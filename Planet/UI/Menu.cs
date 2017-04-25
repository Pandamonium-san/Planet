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
    public int Length { get { return buttons.Length; } }
    Button[] buttons;
    public Menu(int size)
    {
      buttons = new Button[size];
    }
    public Button GetButton(int index)
    {
      return buttons[index];
    }
    public void AddSelection(int index, Button button)
    {
      buttons[index] = button;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < buttons.Length; i++)
      {
        buttons[i].Draw(spriteBatch);
      }
    }
  }
}
