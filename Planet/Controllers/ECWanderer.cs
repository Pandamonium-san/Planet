using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class ECWanderer : AIController
  {
    Vector2 targetPos;
    public ECWanderer(World world, double activationTime = 0) : base(world, activationTime)
    {
      targetPos = new Vector2(Utility.RandomFloat(100, Game1.ScreenWidth - 100), Utility.RandomFloat(100, Game1.ScreenHeight - 100));
    }
    protected override void DoUpdate(GameTime gt)
    {
      ship.Fire1();
      MoveTowards(targetPos);
      if(Vector2.DistanceSquared(ship.Pos, targetPos) < 50)
        targetPos = new Vector2(Utility.RandomFloat(100, Game1.ScreenWidth - 100), Utility.RandomFloat(100, Game1.ScreenHeight - 100));
    }
    private bool IsOutsideScreen()
    {
      return ship.Pos.X > Game1.ScreenWidth || ship.Pos.X < 0 ||
      ship.Pos.Y > Game1.ScreenHeight || ship.Pos.Y > 0;
    }
  }
}
