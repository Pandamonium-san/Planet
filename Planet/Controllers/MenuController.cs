using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  /// <summary>
  /// Handles input for navigating menus
  /// </summary>
  class MenuController : PlayerController
  {
    IMenuGameState gs;
    MenuCursor cursor;
    public MenuController(PlayerIndex index, MenuCursor cursor, IMenuGameState gs)
      : base(index)
    {
      this.gs = gs;
      this.cursor = cursor;

      SetBinding(PlayerInput.Up, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Down, Next, InputType.Pressed);
      SetBinding(PlayerInput.Left, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Right, Next, InputType.Pressed);

      SetBinding(PlayerInput.Start, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Yellow, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Red, Cancel, InputType.Pressed);
    }
    public MenuCursor GetCursor()
    {
      return cursor;
    }
    public void SetCursor(MenuCursor cursor)
    {
      this.cursor = cursor;
    }
    public Button GetSelected()
    {
      return cursor.GetSelected();
    }
    public void Next()
    {
      cursor.Next();
    }
    public void Previous()
    {
      cursor.Previous();
    }
    private void Confirm() { gs.Confirm(this); }
    private void Cancel() { gs.Cancel(this); }
  }
}
