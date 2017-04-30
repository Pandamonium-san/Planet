using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class EnemyController1 : AIController
  {
    Timer moveTimer;
    bool chasing;
    public EnemyController1(World world) : base(world)
    {
      moveTimer = new Timer(2, StartChase, true);
    }
    void StartChase()
    {
      chasing = true;
    }
    protected override void DoUpdate(GameTime gt)
    {
      moveTimer.Update(gt);
      FindNearestTarget();
      if (ship.Target != null && ship.Target.IsActive && ship.Target.Pos != ship.Pos)
      {
        if (Vector2.Distance(ship.Target.Pos, ship.Pos) < 200)
        {
          chasing = false;
          moveTimer.Start();
          ship.SetDash(false);
        }
        if (chasing)
          MoveTowards(ship.Target.Pos, true);
        else
        {
          ship.Fire1();
        }
      }
    }
  }
}
