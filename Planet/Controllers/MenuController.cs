using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  /// <summary>
  /// Sets bindings for, and controls the ship for the player
  /// </summary>
  class MenuController : PlayerController
  {
    private IMenuGameState menu;
    public MenuController(PlayerIndex index, IMenuGameState menu)
      : base(index)
    {
      this.menu = menu;

      SetBinding(PlayerInput.Up, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Down, Next, InputType.Pressed);
      SetBinding(PlayerInput.Left, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Right, Next, InputType.Pressed);

      SetBinding(PlayerInput.Start, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Yellow, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Red, Cancel, InputType.Pressed);
    }

    private void Next() { menu.Next(); }
    private void Previous() { menu.Previous(); }
    private void Confirm() { menu.Confirm(); }
    private void Cancel() { menu.Cancel(); }
  }
}
