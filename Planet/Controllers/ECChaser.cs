﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class ECChaser : AIController
  {
    Timer chaseTimer;
    Timer moveTimer;
    bool chasing;
    public ECChaser(World world, double activationTime = 0) : base(world, activationTime)
    {
      moveTimer = new Timer(1.2f, StartChase, true);
      chaseTimer = new Timer(0.75, null, false);
    }
    void StartChase()
    {
      chasing = true;
      chaseTimer.Start(Utility.RandomFloat(0.75f, 1));
    }
    void StopChase()
    {
      chasing = false;
      moveTimer.Start(Utility.RandomFloat(1, 2));
      ship.SetDash(false);
    }
    protected override void DoUpdate(GameTime gt)
    {
      chaseTimer.Update(gt);
      moveTimer.Update(gt);
      FindNearestTarget();
      if (ship.Target != null && ship.Target.IsActive && ship.Target.Pos != ship.Pos)
      {

        if (chasing)
        {
          MoveTowards(ship.Target.Pos, true);
          if (Vector2.Distance(ship.Target.Pos, ship.Pos) < 300 || chaseTimer.Finished)
            StopChase();
        }
        else
        {
          ship.Fire1();
        }
      }
    }
  }
}
