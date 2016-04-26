using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
    class PlayerController
    {
        private Actor actor;
        private PlayerIndex index;
        private List<KeyBinding> bindings;

        public PlayerController(PlayerIndex index)
        {
            this.index = index;
            bindings = new List<KeyBinding>();
        }

        public void Update(GameTime gt)
        {
            if (actor != null && actor.destroy)
                actor = null;
            if (actor == null)
                return;
            foreach (KeyBinding kb in bindings)
            {
                if (InputHandler.IsButtonDown(index, kb.input, false))
                {
                    if (kb.rapidFire)
                        actor.Invoke(kb.name);
                    else if (InputHandler.IsButtonUp(index, kb.input, true))
                        actor.Invoke(kb.name);
                }
            }
        }

        public void SetActor(Actor actor)
        {
            this.actor = actor;
        }

        public void SetBinding(PlayerInput input, string name, bool rapidFire = false)
        {
            bindings.Add(new KeyBinding(input, name, rapidFire));
        }

        private struct KeyBinding
        {
            public PlayerInput input;
            public string name;
            public bool rapidFire;

            public KeyBinding(PlayerInput input, string name, bool rapidFire)
            {
                this.input = input;
                this.name = name;
                this.rapidFire = rapidFire;
            }
        }
    }

}
