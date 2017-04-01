using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class ShipController
  {
    protected Ship ship;
    protected World world;

    public ShipController() { }
    public ShipController(Ship ship, World world)
    {
      this.ship = ship;
      this.world = world;
    }

    public void Update(GameTime gt)
    {
      if (ship != null && ship.disposed)
        ship = null;
      if (ship == null || ship.isDead || !ship.isActive)
        return;
      else
        DoUpdate(gt);
    }

    protected virtual void DoUpdate(GameTime gt)
    {

    }

    public void SetShip(Ship ship)
    {
      this.ship = ship;
    }
  }
}
