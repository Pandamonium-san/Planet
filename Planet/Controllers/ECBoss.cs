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
    int relocations;

    public ECBoss(World world, double activationTime = 0)
      : base(world, activationTime)
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
              if (relocations > 0)
              {
                relocations--;
                relocateTimer.Start(0.3f);
                if (relocations == 0)
                  relocateTimer.Start(2.0f);
                return;
              }
              if (cycles++ == 6)
              {
                InitPhaseTwo();
                return;
              }
              int wpn = Utility.RandomInt(0, 3);
              ship.SetWeapon(wpn);
              if (wpn == 0)
                relocateTimer.Start(2.7f);
              else if (wpn == 1)
                relocateTimer.Start(3.5f);
              else
              {
                ship.SetWeapon(3);
                relocations = 3;
                relocateTimer.Start(0.3f);
              }
            }
          }
          else
          {
            relocateTimer.Update(gt);
            if (relocations <= 0)
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
      float d;
      do
      {
        targetPos = new Vector2(Utility.RandomFloat(300, Game1.ScreenWidth - 300), Utility.RandomFloat(300, Game1.ScreenHeight - 300));
        d = Vector2.Distance(ship.Pos, targetPos);
      } while (d < 300 && d > 1000);
      FindNearestTarget();
      ship.Rotation = Utility.Vector2ToAngle(targetPos - ship.Pos);
      FireNova();
    }
    void FireNova()
    {
      int i = ship.GetWeaponIndex();
      ship.SetWeapon(3);
      ship.CurrentWeapon.FinishShootTimer();
      ship.Fire1();
      ship.SetWeapon(i);
      ship.CurrentWeapon.FinishShootTimer();
    }
  }
}
