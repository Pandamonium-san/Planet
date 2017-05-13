using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class MenuCursor : Cursor
  {
    public bool Locked { get; private set; }

    private Menu menu;
    private int selectedIndex;

    public MenuCursor(Menu menu, Color color) : base()
    {
      this.menu = menu;
      this.color = color;
      selectedIndex = 0;
      UpdateSpacing();
    }
    private void UpdateSpacing()
    {
      Parent = GetSelected();
      LocalPos = Vector2.Zero;
      Width = GetSelected().tex.Width / 2;
      Height = GetSelected().tex.Height / 2;
    }
    public void SetMenu(Menu menu)
    {
      this.menu = menu;
      selectedIndex = 0;
      UpdateSpacing();
    }
    public void Lock()
    {
      if (Locked)
        return;
      Width *= 0.95f;
      Height *= 0.95f;
      alpha = 0.75f;
      Locked = true;
    }
    public void Unlock()
    {
      if (!Locked)
        return;
      Width /= 0.95f;
      Height /= 0.95f;
      alpha = 1.0f;
      Locked = false;
    }
    public SelectionBox GetSelected()
    {
      return menu.GetButton(selectedIndex);
    }
    public void Next()
    {
      if (Locked)
        return;
      if (++selectedIndex == menu.Length)
        selectedIndex = 0;
      UpdateSpacing();
      AudioManager.PlaySound("click4");
    }
    public void Previous()
    {
      if (Locked)
        return;
      if (--selectedIndex < 0)
        selectedIndex = menu.Length - 1;
      UpdateSpacing();
      AudioManager.PlaySound("click4");
    }
  }
}
