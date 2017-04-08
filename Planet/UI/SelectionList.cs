using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class SelectionList
  {
    public Button Selected { get { return buttons[selectedIndex]; } }
    int selectedIndex;
    Button[] buttons;

    public SelectionList(int size)
    {
      selectedIndex = 0;
      buttons = new Button[size];
    }
    public void AddSelection(int index, Button button) { buttons[index] = button; }
    public void Next()
    {
      if (++selectedIndex == buttons.Length)
        selectedIndex = 0;
    }
    public void Previous()
    {
      if (--selectedIndex < 0)
        selectedIndex = buttons.Length - 1;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < buttons.Length; i++)
      {
        if(i == selectedIndex)
        {
          buttons[i].alpha = 1.0f;
        }
        else
        {
          buttons[i].alpha = 0.7f;
        }
        buttons[i].Draw(spriteBatch);
      }
    }
  }
}
