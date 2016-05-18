using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class RewinderShip : Ship
    {
        public RewinderShip(Vector2 pos):base(pos)
        {
            layer = Layer.PLAYER_SHIP;
            rotationSpeed = 15;
        }

        public override void Fire1()
        {

        }

        public override void Fire2()
        {
        }

        public override void Fire3()
        {

        }
    }
}
