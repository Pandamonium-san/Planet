using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
    class PlayerController : Controller
    {
        private PlayerIndex index;
        private List<KeyBinding> bindings;

        public PlayerController(PlayerIndex index, Ship ship = null)
            : base(ship)
        {
            this.ship = ship;
            this.index = index;
            bindings = new List<KeyBinding>();
        }

        public override void DoUpdate(GameTime gt)
        {
            foreach (KeyBinding kb in bindings)
            {
                if (InputHandler.IsButtonDown(index, kb.input, false))
                {
                    if (kb.rapidFire)
                        ship.Invoke(kb.name, kb.args);
                    else if (InputHandler.IsButtonUp(index, kb.input, true))
                        ship.Invoke(kb.name, kb.args);
                }
            }
        }

        public void SetBinding(PlayerInput input, string name, object[] args = null, bool rapidFire = false)
        {
            bindings.Add(new KeyBinding(input, name, args, rapidFire));
        }
        public void SetBinding(PlayerInput input, string name, object args, bool rapidFire = false)
        {
            bindings.Add(new KeyBinding(input, name, new object[] { args }, rapidFire));
        }

        private struct KeyBinding
        {
            public PlayerInput input;
            public string name;
            public bool rapidFire;
            public object[] args;

            public KeyBinding(PlayerInput input, string name, object[] args, bool rapidFire)
            {
                this.input = input;
                this.name = name;
                this.rapidFire = rapidFire;
                this.args = args;
            }
        }
    }

}
