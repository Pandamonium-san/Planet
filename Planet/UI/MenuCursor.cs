using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class MenuCursor : Sprite
  {
    public bool Locked { get; private set; }

    private bool isRight;
    private Menu menu;
    private int selectedIndex;

    public MenuCursor(Menu menu, Texture2D tex, bool right) : base(Vector2.Zero, tex)
    {
      this.menu = menu;
      this.isRight = right;
      if (right)
        spriteEffects = SpriteEffects.FlipHorizontally;
      selectedIndex = -1;
      Next();
    }
    public void Update(GameTime gt)
    {

    }
    public void Lock()
    {
      Locked = true;
    }
    public void Unlock()
    {
      Locked = false;
    }
    public Button GetSelected()
    {
      return menu.GetButton(selectedIndex);
    }
    public void Next()
    {
      if (Locked)
        return;
      if (++selectedIndex == menu.Length)
        selectedIndex = 0;
      this.Parent = GetSelected();
      LocalPos = Vector2.UnitX * (GetSelected().tex.Width / 2 + tex.Width) * (isRight ? 1 : -1); 
    }
    public void Previous()
    {
      if (Locked)
        return;
      if (--selectedIndex < 0)
        selectedIndex = menu.Length - 1;
      this.Parent = GetSelected();
      LocalPos = Vector2.UnitX * (GetSelected().tex.Width / 2 + tex.Width) * (isRight ? 1: -1);
    }
  }
}
