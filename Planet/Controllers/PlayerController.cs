using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
  /// <summary>
  /// Handles bindings for players and controls ships.
  /// </summary>
  class PlayerController : ShipController
  {
    private PlayerIndex index;
    private List<KeyBinding> bindings;

    public PlayerController(PlayerIndex index, World world, Ship ship = null)
        : base(ship, world)
    {
      this.ship = ship;
      this.index = index;
      bindings = new List<KeyBinding>();

      SetBinding(PlayerInput.Up, "MoveAndTurn", -Vector2.UnitY, true);
      SetBinding(PlayerInput.Down, "MoveAndTurn", Vector2.UnitY, true);
      SetBinding(PlayerInput.Right, "MoveAndTurn", Vector2.UnitX, true);
      SetBinding(PlayerInput.Left, "MoveAndTurn", -Vector2.UnitX, true);

      SetBinding(PlayerInput.Yellow, "Fire1", null, true);
      SetBinding(PlayerInput.Red, "Fire2", null, true);
      SetBinding(PlayerInput.Blue, "Fire3", null, true);
      SetBinding(PlayerInput.A, "Aim", null, true);
    }

    protected override void DoUpdate(GameTime gt)
    {
      foreach (KeyBinding kb in bindings)
      {
        if (InputHandler.IsButtonDown(index, kb.input, false))
        {
          if (kb.rapidFire)
            ship.Invoke(kb.name, kb.args);
          else if (InputHandler.IsButtonUp(index, kb.input, true))
            ship.Invoke(kb.name, kb.args);
        }
      }
    }

    public void SetBinding(PlayerInput input, string name, object[] args = null, bool rapidFire = false)
    {
      bindings.Add(new KeyBinding(input, name, args, rapidFire));
    }
    public void SetBinding(PlayerInput input, string name, object args, bool rapidFire = false)
    {
      bindings.Add(new KeyBinding(input, name, new object[] { args }, rapidFire));
    }
  }
  internal struct KeyBinding
  {
    public PlayerInput input;
    public string name;
    public bool rapidFire;
    public object[] args;

    public KeyBinding(PlayerInput input, string name, object[] args, bool rapidFire)
    {
      this.input = input;
      this.name = name;
      this.rapidFire = rapidFire;
      this.args = args;
    }
  }

}
