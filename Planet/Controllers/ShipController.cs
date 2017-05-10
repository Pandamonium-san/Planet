using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public interface ShipController
  {
    Ship GetShip();
    void SetShip(Ship ship);
  }
}
