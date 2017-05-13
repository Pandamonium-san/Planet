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
      if (index == PlayerIndex.One)
        Color = new Color(73, 231, 108);
      else if (index == PlayerIndex.Two)
        Color = new Color(0, 148, 255);
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
