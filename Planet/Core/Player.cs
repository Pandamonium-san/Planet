using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
  /// <summary>
  /// Contains data about a player such as score and index.
  /// </summary>
  public class Player
  {
    public PlayerIndex Index { get; private set; }
    public Ship Ship { get; private set; }

    private PlayerShipController pc;

    public Player(PlayerIndex index)
    {
      Index = index;
    }

    public void Update(GameTime gt)
    {
      //case State.Playing:
      pc.Update(gt);
      /* case State.Menu:
      * menuController.Update(gt);
      * {
      *   if(UpPressed)
      *     ++menu.selectionIndex;
      *   if(EnterPressed)
      *     menu.Select();
      * }
      */
    }
    public void MenuUpdate(GameTime gt)
    {

    }

    public void SetShip(Ship ship)
    {
      this.Ship = ship;
      if (pc == null)
        pc = new PlayerShipController(Index, ship);
      else
        pc.Ship = ship;
      ship.restrictToScreen = true;
    }
  }
}
