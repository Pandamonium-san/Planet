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
  class PlayerShipController : PlayerController, ShipController
  {
    public Ship Ship { get; set; }
    public PlayerShipController(PlayerIndex index, Ship ship)
      : base(index)
    {
      SetShip(ship);

      SetBinding(PlayerInput.Up, Up, InputType.Down);
      SetBinding(PlayerInput.Down, Down, InputType.Down);
      SetBinding(PlayerInput.Left, Left, InputType.Down);
      SetBinding(PlayerInput.Right, Right, InputType.Down);

      SetBinding(PlayerInput.Yellow, Fire1, InputType.Down);
      SetBinding(PlayerInput.B, Fire2, InputType.Pressed);
      SetBinding(PlayerInput.Blue, UnlockTarget, InputType.Down);
      SetBinding(PlayerInput.Blue, SwitchTarget, InputType.Released);
      SetBinding(PlayerInput.Red, DashPressed, InputType.Down);
      SetBinding(PlayerInput.Red, DashReleased, InputType.Up);
      SetBinding(PlayerInput.A, Switch, InputType.Pressed);
    }
    public override void Update(GameTime gt)
    {
      if (Ship != null && !Ship.Disposed)
        base.Update(gt);
      else
        Ship = null;
    }
    private void Up() { Ship.Move(-Vector2.UnitY); }
    private void Down() { Ship.Move(Vector2.UnitY); }
    private void Left() { Ship.Move(-Vector2.UnitX); }
    private void Right() { Ship.Move(Vector2.UnitX); }
    private void Fire1() { Ship.Fire1(); }
    private void Fire2() { Ship.Fire2(); }
    private void UnlockTarget() { Ship.unlockTarget = true; }
    private void SwitchTarget() { Ship.SwitchTarget(); }
    private void DashPressed() { Ship.SetDash(true); }
    private void DashReleased() { Ship.SetDash(false); }
    private void Switch() { Ship.Switch(); }

    public void SetShip(Ship ship)
    {
      if (Ship != null)
        Ship.Controller = null;
      if (ship != null)
        ship.Controller = this;
      this.Ship = ship;
      Ship.ClampToScreen = true;
    }
  }
}
