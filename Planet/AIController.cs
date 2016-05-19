using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
    class AIController : Controller
    {
        protected Ship target;

        public AIController();
        public AIController(Ship ship) : base(ship) { }

        public override void DoUpdate(GameTime gt)
        {
            ship.speedModifier -= 0.85f;
            target = FindNearestTarget();
            Vector2 direction;
            if (target != null && target.isActive)
            {
                direction = target.Pos - ship.Pos;
                direction.Normalize();
                ship.Move(direction);
            }

            ship.Invoke("Fire1");
        }

        protected virtual Ship FindNearestTarget()
        {
            List<Player> players = Game1.objMgr.GetPlayers();
            Ship nearest = null;
            float nDistance = 1000000;
            foreach (Player p in players)
            {
                float distance = Utility.Distance(p.GetShip().Pos, ship.Pos);
                if (distance < nDistance)
                {
                    nearest = p.GetShip();
                    nDistance = distance;
                }
            }
            return nearest;
        }
    }

}
