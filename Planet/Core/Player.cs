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
    public PlayerIndex Index { get; private set; }
    public Ship Ship { get; private set; }

    private PlayerShipController pc;

    public Player(PlayerIndex index)
    {
      this.Index = index;
    }

    public void Update(GameTime gt)
    {
      pc.Update(gt);
    }
    public void SetShip(Ship ship)
    {
      pc = new PlayerShipController(Index, ship);
    }
  }
}
