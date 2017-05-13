using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /// <summary>
  /// Contains player data and input classes
  /// </summary>
  public class Player
  {
    public bool Joined { get; set; }
    public string SelectedShip { get; set; }
    public Color Color { get; set; }
    public PlayerIndex Index { get; private set; }
    public float Score { get; set; }
    public PlayerController Controller { get; set; }

    public Player(PlayerIndex index)
    {
      this.Index = index;
      SelectedShip = "Rewinder";
    }
    public Ship GetShip()
    {
      if (Controller is PlayerShipController)
        return ((PlayerShipController)Controller).Ship;
      else
        return null;
    }
  }
}
