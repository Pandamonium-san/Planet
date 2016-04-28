using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
    class AIController
    {
        protected Ship ship;
        protected Player target;

        public AIController()
        {

        }

        public void Update(GameTime gt)
        {
            ship.speedModifier -= 0.8f;
            target = FindNearestTarget();
            Vector2 direction = target.GetShip().pos - ship.pos;
            direction.Normalize();
            ship.Move(direction);

            ship.Invoke("Fire1");
        }

        public void SetActor(Ship ship)
        {
            this.ship = ship;
        }

        protected virtual Player FindNearestTarget()
        {
            List<Player> players = Game1.objMgr.GetPlayers();
            Player nearest = null;
            float nDistance = 1000000;
            foreach (Player p in players)
            {
                float distance = Utility.Distance(p.GetShip().pos, ship.pos);
                if (distance < nDistance)
                {
                    nearest = p;
                    nDistance = distance;
                }
            }
            return nearest;
        }
    }

}
