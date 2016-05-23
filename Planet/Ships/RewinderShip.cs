using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class RewinderShip : Ship
    {
        Weapon wpn;

        public RewinderShip(Vector2 pos)
            : base(pos)
        {
            hitboxOffset = new Vector2(50, 50);
            SetTexture(AssetManager.GetTexture("Ship1"));
            layer = Layer.PLAYER_SHIP;
            rotationSpeed = 15;

            //wpn = new Weapon(this, WpnDesc.Spread());
            //wpn = new Weapon(this, WpnDesc.Circle(150));
            //wpn = new Weapon(this, new WpnDesc());
            wpn = new CycloneGun(this);

            wpn.inaccuracy = 0;
            wpn.speedVariance = 0;
            drawHitbox = true;
        }

        protected override void DoUpdate(GameTime gt)
        {
            base.DoUpdate(gt);
            wpn.Update(gt);
        }

        public override void Fire1()
        {
            wpn.Fire();
        }

        public override void Fire2()
        {
            foreach (GameObject g in Game1.objMgr.gameObjects)
            {
                if (g.layer != Layer.PLAYER_SHIP)
                {
                    if(!g.isRewinding)
                        g.RewindOnce();
                    //g.LoadPreviousState();
                }
                //g.StartRewind(GameObject.maxRewindableFrames);
            }
            foreach (GameObject g in Game1.objMgr.projectiles)
            {
                if (!g.isRewinding)
                    g.RewindOnce();
                //g.LoadPreviousState();
                //g.StartRewind(GameObject.maxRewindableFrames);
            }
        }

        public override void Fire3()
        {

        }

        protected override GameObjState GetState()
        {
            return new RewinderShipState(this);
        }

        protected override void SetState(GameObjState other)
        {
            base.SetState(other);
            RewinderShipState g = (RewinderShipState)other;
            this.wpn.SetState(g.ws);
        }

        protected class RewinderShipState : GameObjState
        {
            public WeaponState ws;

            public RewinderShipState(RewinderShip s)
                : base(s)
            {
                ws = new WeaponState(s.wpn);
            }
        }
    }
}
