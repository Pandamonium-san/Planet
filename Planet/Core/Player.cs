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
    private PlayerIndex playerIndex;
    private PlayerController pc;
    private Ship ship;

    public Player(PlayerIndex index, World world)
    {
      playerIndex = index;
      pc = new PlayerController(index, world);
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

    public void SetShip(Ship a)
    {
      ship = a;
      pc.SetShip(a);
      a.restrictToScreen = true;
    }

    public Ship GetShip()
    {
      return ship;
    }
  }
}
