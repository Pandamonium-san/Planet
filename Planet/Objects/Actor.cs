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
    public Actor(Vector2 pos, World world)
        : base(pos, world)
    {

    }


    public void Invoke(string name, object[] parameters = null)
    {
      MethodInfo method = this.GetType().GetMethod(name);
      method.Invoke(this, parameters);
    }
  }
}
