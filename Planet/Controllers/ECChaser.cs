using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class ECChaser : AIController
  {
    public float Range { get; set; }

    Timer chasingTimer;
    float chaseTime;
    Timer shootingTimer;
    float shootTime;

    public ECChaser(World world, double activationTime = 0, float shootTime = 1.2f, float chaseTime = 0.75f, float range = 300) : base(world, activationTime)
    {
      this.shootTime = shootTime;
      this.chaseTime = chaseTime;
      shootingTimer = new Timer(shootTime, StartChase, true);
      chasingTimer = new Timer(chaseTime, StopChase, false);
      Range = range;
    }
    public void SetChaseTimer(double time)
    {
      chasingTimer = new Timer(time, null, false);
    }
    void StartChase()
    {
      chasingTimer.Start(Utility.RandomFloat(0.8f * chaseTime, 1.2f * chaseTime));
    }
    void StopChase()
    {
      chasingTimer.ForceFinish();
      shootingTimer.Start(Utility.RandomFloat(0.8f * shootTime, 1.2f * shootTime));
      ship.SetDash(false);
    }
    protected override void DoUpdate(GameTime gt)
    {
      chasingTimer.Update(gt);
      shootingTimer.Update(gt);
      FindNearestTarget();
      if (ship.Target != null && ship.Target.IsActive && ship.Target.Pos != ship.Pos)
      {
        if (chasingTimer.Counting)
        {
          if (ship.CurrentWeapon.DashUsable)
            ship.Fire1();
          MoveTowards(ship.Target.Pos, true);
          if (Vector2.Distance(ship.Target.Pos, ship.Pos) < Range || chasingTimer.Finished)
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
