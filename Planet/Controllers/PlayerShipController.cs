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
    protected Ship ship;
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
      if (ship != null && !ship.Disposed)
        base.Update(gt);
      else
        ship = null;
    }
    private void Up() { ship.Move(-Vector2.UnitY); }
    private void Down() { ship.Move(Vector2.UnitY); }
    private void Left() { ship.Move(-Vector2.UnitX); }
    private void Right() { ship.Move(Vector2.UnitX); }
    private void Fire1() { ship.Fire1(); }
    private void Fire2() { ship.Fire2(); }
    private void UnlockTarget() { ship.freeAim = true; }
    private void SwitchTarget() { ship.freeAim = false; ship.SwitchTarget(); }
    private void DashPressed() { ship.SetDash(true); }
    private void DashReleased() { ship.SetDash(false); }
    private void Switch() { ship.Switch(); }

    public Ship GetShip()
    {
      return ship;
    }
    public void SetShip(Ship ship)
    {
      if (ship != null)
        ship.Controller = null;
      if (ship != null)
        ship.Controller = this;
      this.ship = ship;
      ship.ClampToScreen = true;
    }
  }
}
