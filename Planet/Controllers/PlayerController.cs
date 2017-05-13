using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
  /// <summary>
  /// Handles bindings for players
  /// </summary>
  public abstract class PlayerController
  {
    public Player Player { get; private set; }
    private List<KeyBinding> bindings;

    public PlayerController(Player player)
    {
      this.Player = player;
      player.Controller = this;
      bindings = new List<KeyBinding>();
    }
    public virtual void Update(GameTime gt)
    {
      foreach (KeyBinding kb in bindings)
      {
        kb.Update();
      }
    }
    public void SetBinding(PlayerInput input, Action action, InputType inputType)
    {
      KeyBinding kb = bindings.Find(x => x.input == input && x.type == inputType);
      if (kb == null)
        bindings.Add(new KeyBinding(Player.Index, input, action, inputType));
      else
        kb = new KeyBinding(Player.Index, input, action, inputType);
    }

    class KeyBinding
    {
      public PlayerInput input;
      public InputType type;
      PlayerIndex index;
      Action action;

      public KeyBinding(PlayerIndex index, PlayerInput input, Action action, InputType inputType)
      {
        this.index = index;
        this.input = input;
        this.action = action;
        this.type = inputType;
      }
      public void Update()
      {
        if (Activated())
          action.Invoke();
      }
      private bool Activated()
      {
        switch (type)
        {
          case InputType.Pressed:
            return InputHandler.IsButtonDown(index, input, false) && InputHandler.IsButtonUp(index, input, true);
          case InputType.Released:
            return InputHandler.IsButtonUp(index, input, false) && InputHandler.IsButtonDown(index, input, true);
          case InputType.Down:
            return InputHandler.IsButtonDown(index, input, false);
          case InputType.Up:
            return InputHandler.IsButtonUp(index, input, false);
        }
        return false;
      }
    }
  }
  /// <summary>
  /// Defines what type of interaction with the key will activate the binding.
  /// </summary>
  public enum InputType 
  { 
    /// <summary> Key is pressed from a released state </summary>
    Pressed,
    /// <summary> Key is released from a pressed state </summary>
    Released,
    /// <summary> Key is pressed/held </summary>
    Down,
    /// <summary> Key is released </summary>
    Up
  }

}
