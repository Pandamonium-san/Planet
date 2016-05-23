using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class Controller
    { 
        protected Ship ship;

        public Controller() { }
        public Controller(Ship ship) { this.ship = ship; }

        public void Update(GameTime gt)
        {
            if (ship != null && ship.disposed)
                ship = null;
            if (ship == null || ship.isDead || !ship.isActive || ship.isRewinding)
                return;

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
