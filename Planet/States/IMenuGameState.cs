using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  interface IMenuGameState
  {
    void Confirm(MenuController pi);
    void Cancel(MenuController pi);
  }
}
