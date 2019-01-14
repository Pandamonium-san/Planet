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
    public Ship Ship { get; private set; }

    public PlayerShipController(Player player, Ship ship)
      : base(player)
    {
      SetShip(ship);
    }
    public override void Update(GameTime gt)
    {
      if (Ship != null && !Ship.Disposed)
        base.Update(gt);
    }
    public Ship GetShip()
    {
      return Ship;
    }
    public void SetShip(Ship ship)
    {
      if (ship != null)
        ship.Controller = null;
      if (ship != null)
        ship.Controller = this;
      this.Ship = ship;
      InitBindings();
      ship.ClampToScreen = true;
    }
    private void InitBindings()
    {
      SetBinding(PlayerInput.Up, Up, InputType.Down);
      SetBinding(PlayerInput.Down, Down, InputType.Down);
      SetBinding(PlayerInput.Left, Left, InputType.Down);
      SetBinding(PlayerInput.Right, Right, InputType.Down);

      SetBinding(PlayerInput.Yellow, Fire1, InputType.Down);
      SetBinding(PlayerInput.A, Fire2, InputType.Pressed);
      SetBinding(PlayerInput.Blue, UnlockTarget, InputType.Down);
      SetBinding(PlayerInput.Blue, SwitchTarget, InputType.Released);
      SetBinding(PlayerInput.Green, DashPressed, InputType.Down);
      SetBinding(PlayerInput.Green, DashReleased, InputType.Up);
      SetBinding(PlayerInput.Red, Switch, InputType.Pressed);
    }
    private void Up()
    {
      if (GamePad.GetState(Player.Index).IsConnected)
        Ship.Move(-Vector2.UnitY * Math.Abs(GamePad.GetState(Player.Index).ThumbSticks.Left.Y));
      else
        Ship.Move(-Vector2.UnitY);
    }
    private void Down()
    {
      if (GamePad.GetState(Player.Index).IsConnected)
        Ship.Move(Vector2.UnitY * Math.Abs(GamePad.GetState(Player.Index).ThumbSticks.Left.Y));
      else
        Ship.Move(Vector2.UnitY);
    }
    private void Left()
    {
      if (GamePad.GetState(Player.Index).IsConnected)
        Ship.Move(-Vector2.UnitX * Math.Abs(GamePad.GetState(Player.Index).ThumbSticks.Left.X));
      else
        Ship.Move(-Vector2.UnitX);
    }
    private void Right()
    {
      if (GamePad.GetState(Player.Index).IsConnected)
        Ship.Move(Vector2.UnitX * Math.Abs(GamePad.GetState(Player.Index).ThumbSticks.Left.X));
      else
        Ship.Move(Vector2.UnitX);
    }
    private void Fire1() { Ship.Fire1(); }
    private void Fire2() { Ship.Fire2(); }
    private void UnlockTarget() { Ship.freeAim = true; }
    private void SwitchTarget() { Ship.freeAim = false; Ship.SwitchTarget(); }
    private void DashPressed() { Ship.SetDash(true); }
    private void DashReleased() { Ship.SetDash(false); }
    private void Switch() { Ship.Switch(); }
  }
}
