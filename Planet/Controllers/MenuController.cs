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
    MenuGameState gs;
    public MenuCursor Cursor { get; set; }
    public MenuController(Player player, MenuCursor cursor, MenuGameState gs)
      : base(player)
    {
      this.gs = gs;
      this.Cursor = cursor;

      SetBinding(PlayerInput.Up, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Down, Next, InputType.Pressed);
      SetBinding(PlayerInput.Left, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Right, Next, InputType.Pressed);

      SetBinding(PlayerInput.Start, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Yellow, Confirm, InputType.Pressed);
#if DEBUG
      SetBinding(PlayerInput.Red, Cancel, InputType.Pressed);
#else
      SetBinding(PlayerInput.Green, Cancel, InputType.Pressed);
#endif
    }
    public SelectionBox GetSelected()
    {
      return Cursor.GetSelected();
    }
    public void Next()
    {
      Cursor.Next();
    }
    public void Previous()
    {
      Cursor.Previous();
    }
    private void Confirm() { gs.Confirm(this); }
    private void Cancel() { gs.Cancel(this); }
  }
}
