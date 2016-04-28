using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class TesekShip : Ship
    {
        public TesekShip(Vector2 pos):base(pos)
        {
            layer = Layer.PLAYER;
            rotationSpeed = 15;
        }

        public override void Fire1()
        {
            if (shotDelay < 0)
            {
                Vector2 dir = Utility.AngleToVector2(rotation);
                Vector2 pos1 = new Vector2(dir.Y, -dir.X) * 5;
                Vector2 pos2 = new Vector2(dir.Y, -dir.X) * -5;
                Projectile p = new Projectile(pos + pos1, dir, 750, 10, this);
                Projectile q = new Projectile(pos + pos2, dir, 750, 10, this);
                Projectile r = new Projectile(pos + pos1, dir + Utility.AngleToVector2(rotation + -0.5f), 750, 10, this);
                Projectile s = new Projectile(pos + pos1, dir + Utility.AngleToVector2(rotation + 0.5f), 750, 10, this);
                Game1.objMgr.PostGameObj(s);
                Game1.objMgr.PostGameObj(r);
                Game1.objMgr.PostGameObj(p);
                Game1.objMgr.PostGameObj(q);
                shotDelay = 1 / shotsPerSecond;
            }
        }

        public override void Fire2()
        {

        }

        public override void Fire3()
        {

        }
    }
}
