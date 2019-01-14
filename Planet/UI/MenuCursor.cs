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
      Scale = 1.2f;
      selectedIndex = 0;
      UpdateSpacing();
    }
    private void UpdateSpacing()
    {
      SelectionBox selected = GetSelected();
      Parent = selected;
      LocalPos = Vector2.Zero;
      Width = selected.tex.Width / 2 * selected.Scale;
      Height = selected.tex.Height / 2 * selected.Scale;
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
      Width *= 0.85f;
      Height *= 0.85f;
      alpha = 0.75f;
      Locked = true;
    }
    public void Unlock()
    {
      if (!Locked)
        return;
      Width /= 0.85f;
      Height /= 0.85f;
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
