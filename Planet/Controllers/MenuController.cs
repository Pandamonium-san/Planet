﻿using Microsoft.Xna.Framework;
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
    private Menu menu;
    public MenuController(PlayerIndex index, Menu menu)
      : base(index)
    {
      this.menu = menu;

      SetBinding(PlayerInput.Up, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Down, Next, InputType.Pressed);
      SetBinding(PlayerInput.Left, Previous, InputType.Pressed);
      SetBinding(PlayerInput.Right, Next, InputType.Pressed);
    }

    private void Next() { menu.NextSelection(); }
    private void Previous() { menu.PreviousSelection(); }
    private void Confirm() { menu.Confirm(); }
    private void Cancel() { menu.Cancel(); }
  }
}
