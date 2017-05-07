using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class ECBoss : AIController
  {
    enum Phase
    {
      One,
      Two
    }
    Vector2 targetPos;
    Phase phase;
    Timer relocateTimer;
    Timer phaseTwoTimer;
    int cycles;

    public ECBoss(World world)
      : base(world)
    {
      relocateTimer = new Timer(3, Relocate, false);
      phaseTwoTimer = new Timer(20, InitPhaseOne, false);
      targetPos = new Vector2(Utility.RandomFloat(300, Game1.ScreenWidth - 300), Utility.RandomFloat(300, Game1.ScreenHeight - 300));
    }
    protected override void DoUpdate(GameTime gt)
    {
      switch (phase)
      {
        case Phase.One:
          if (!relocateTimer.Counting)
          {
            MoveTowards(targetPos, true);
            if (Vector2.DistanceSquared(ship.Pos, targetPos) < 50)
            {
              MoveTowards(targetPos, false);
              if (cycles++ == 6)
              {
                InitPhaseTwo();
                return;
              }
              int wpn = Utility.RandomInt(0, 2);
              ship.SetWeapon(wpn);
              if (wpn == 0)
                relocateTimer.Start(2.7f);
              else
                relocateTimer.Start(3.5f);
            }
          }
          else
          {
            relocateTimer.Update(gt);
            ship.Fire1();
          }
          break;
        case Phase.Two:
          phaseTwoTimer.Update(gt);
          if (Vector2.DistanceSquared(ship.Pos, targetPos) > 50)
          {
            MoveTowards(targetPos);
          }
          if (phaseTwoTimer.elapsedSeconds > 4)
          {
            ship.Fire1();
          }
          else if (phaseTwoTimer.elapsedSeconds < 4)
          {
            FindTargetAndLockRotation();
          }
          if (phaseTwoTimer.elapsedSeconds > 11 && phaseTwoTimer.elapsedSeconds < 14)
          {
            FindTargetAndLockRotation();
          }
          break;
      }
    }
    void FindTargetAndLockRotation()
    {
      FindNearestTarget();
      if (ship.Target == null)
        return;
      ship.Rotation = Utility.Vector2ToAngle(ship.Target.Pos - ship.Pos);
      ship.rotationModifier = 0.0f;
    }
    void InitPhaseOne()
    {
      phase = Phase.One;
      ship.rotationModifier = 1.0f;
      Relocate();
      cycles = 0;
    }
    void InitPhaseTwo()
    {
      phase = Phase.Two;
      ship.SetWeapon(2);
      targetPos = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
      ship.Target = null;
      phaseTwoTimer.Start();
    }
    void Relocate()
    {
      while (Vector2.Distance(ship.Pos, targetPos) < 300)
        targetPos = new Vector2(Utility.RandomFloat(300, Game1.ScreenWidth - 300), Utility.RandomFloat(300, Game1.ScreenHeight - 300));
      FindNearestTarget();
    }
  }
}
