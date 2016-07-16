using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
  class AIController : Controller
  {
    protected GameObject target;

    public AIController() { }
    public AIController(Ship ship, World world) : base(ship, world) { }

    protected override void DoUpdate(GameTime gt)
    {
      ship.speedModifier -= 0.85f;
      target = FindNearestTarget();
      if (target != null && target.isActive && target.Pos != ship.Pos)
      {
        Vector2 direction = target.Pos - ship.Pos;
        direction.Normalize();
        ship.Move(direction);
      }

      ship.Invoke("Fire1");
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
