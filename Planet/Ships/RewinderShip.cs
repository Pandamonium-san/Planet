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
            hitboxOffset = new Vector2(10, 10);
            SetTexture(AssetManager.GetTexture("Ship1"));
            layer = Layer.PLAYER_SHIP;
            rotationSpeed = 15;

            wpn = new Weapon(this, WpnDesc.Spread());
            //wpn = new Weapon(this, WpnDesc.Circle(150));
            wpn = new CycloneGun(this);

            //objStates = new FixedList<RewinderShip>(queueSize);

            //GameObject[] tre = new GameObject[3];
            //RewinderShip[] tv2 = (RewinderShip[])tre;
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
                //if(g.layer != Layer.PLAYER_SHIP)
                g.StartRewind(GameObject.bufferFrames);
            }
            foreach (GameObject g in Game1.objMgr.projectiles)
            {
                g.StartRewind(GameObject.bufferFrames);
            }
        }

        public override void Fire3()
        {

        }

        protected override GameObject GetState()
        {
            RewinderShip g = new RewinderShip(Pos);
            g.Rotation = this.Rotation;
            g.Scale = this.Scale;
            g.Parent = this.Parent;
            g.frame = this.frame;
            //
            // class specific variables that need to be saved go here
            //
            return g;
        }

        protected override void SetState(GameObject other)
        {
            base.SetState(other);
            RewinderShip g = (RewinderShip)other;
            //
            // class specific variables that need to be loaded go here
            //
        }

    }
}
