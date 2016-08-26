using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
  class AIController : ShipController
  {
    protected GameObject target;

    public AIController() { }
    public AIController(Ship ship, World world) : base(ship, world) { }

    protected override void DoUpdate(GameTime gt)
    {
      target = FindNearestTarget();
      if (target != null && target.isActive && target.Pos != ship.Pos)
      {
        //if(Utility.Distance(ship.Pos, target.Pos) >= 200)
          MoveTowards(new Vector2(300, 300));
        ship.TurnTowardsPoint(target.Pos);
      }

      ship.Invoke("Fire1");
    }
    protected virtual void MoveTowards(Vector2 point)
    {
      Vector2 direction = point - ship.Pos;
      ship.Move(direction);
    }
    protected virtual void LookAt(Vector2 point)
    {
      Vector2 direction = point - ship.Pos;
      ship.Rotation = Utility.Vector2ToAngle(direction);
    }
    protected virtual GameObject FindNearestTarget()
    {
      List<GameObject> players = world.GetPlayers();
      GameObject nearest = null;
      float nDistance = 1000000;
      foreach (GameObject p in players)
      {
        float distance = Utility.Distance(p.Pos, ship.Pos);
        if (distance < nDistance)
        {
          nearest = p;
          nDistance = distance;
        }
      }
      return nearest;
    }
  }

}
