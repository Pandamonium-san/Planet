using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
    public class Player
    {
        private PlayerIndex playerIndex;
        private PlayerController pc;
        private Ship ship;

        public Player(PlayerIndex index)
        {
            playerIndex = index;
            pc = new PlayerController(index);

            pc.SetBinding(PlayerInput.Up, "Move", -Vector2.UnitY, true);
            pc.SetBinding(PlayerInput.Down, "Move", Vector2.UnitY, true);
            pc.SetBinding(PlayerInput.Right, "Move", Vector2.UnitX, true);
            pc.SetBinding(PlayerInput.Left, "Move", -Vector2.UnitX, true);

            pc.SetBinding(PlayerInput.Yellow, "Fire1", null, true);
            pc.SetBinding(PlayerInput.Red, "Fire2", null, false);
            pc.SetBinding(PlayerInput.Blue, "Fire3", null, false);
            pc.SetBinding(PlayerInput.A, "Aim", null, true);
        }

        public void Update(GameTime gt)
        {
            pc.Update(gt);
        }

        public void SetShip(Ship a)
        {
            ship = a;
            pc.SetActor(a);
        }

        public Ship GetShip()
        {
            return ship;
        }
    }
}
