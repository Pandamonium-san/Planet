using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Planet
{
    public class Actor : GameObject
    {
        public Actor(Vector2 pos)
            : base(pos)
        {

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public Vector2 GetDirection()
        {
            return Utility.AngleToVector2(Rotation);
        }

        public void Invoke(string name, object[] parameters = null)
        {
            MethodInfo method = this.GetType().GetMethod(name);
            method.Invoke(this, parameters);
        }
    }
}
