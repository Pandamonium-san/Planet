using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class Wave
  {
    public List<Spawn> Queue { get { return queue; } }
    List<Spawn> queue;
    EnemyManager em;
    public Wave(EnemyManager em)
    {
      this.em = em;
      queue = new List<Spawn>();
    }

    public static Wave Wave1(EnemyManager em)
    {
      Wave w1 = new Wave(em);

      return w1;
    }
  }
}
