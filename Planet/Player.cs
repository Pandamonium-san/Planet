using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Planet
{
    class Player
    {
        PlayerIndex playerIndex;
        PlayerController pc;
        Ship ship;

        public Player(PlayerIndex index)
        {
            pc = new PlayerController(index);

            pc.SetBinding(PlayerInput.Up, "Forward", true);
            pc.SetBinding(PlayerInput.Down, "Backward", true);
            pc.SetBinding(PlayerInput.Right, "Right", true);
            pc.SetBinding(PlayerInput.Left, "Left", true);
            pc.SetBinding(PlayerInput.Yellow, "Fire1", true);
            pc.SetBinding(PlayerInput.Green, "Fire2", false);
            pc.SetBinding(PlayerInput.Blue, "Fire3", false);
            pc.SetBinding(PlayerInput.Red, "Aim", true);
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
    }
}
