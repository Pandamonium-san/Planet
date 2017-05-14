using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class NameInputController : PlayerController
  {
    GameStateInputName gs;
    public NameInput NameInput { get; set; }
    public NameInputController(GameStateInputName gs, NameInput nameInput, Player player)
      : base(player)
    {
      this.gs = gs;
      this.NameInput = nameInput;

      SetBinding(PlayerInput.Up, nameInput.Up, InputType.Pressed);
      SetBinding(PlayerInput.Down, nameInput.Down, InputType.Pressed);
      SetBinding(PlayerInput.Left, nameInput.Left, InputType.Pressed);
      SetBinding(PlayerInput.Right, nameInput.Right, InputType.Pressed);

      SetBinding(PlayerInput.Start, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Yellow, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Red, Confirm, InputType.Pressed);
      SetBinding(PlayerInput.Green, Confirm, InputType.Pressed);
    }
    private void Confirm()
    {
      gs.Confirm(this);
    }
  }
}
