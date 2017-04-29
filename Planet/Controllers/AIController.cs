using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Planet
{
  public class AIController : ShipController
  {
    public bool IsActive { get; set; }
    protected Ship ship;
    protected World world;

    public AIController(World world)
    {
      this.world = world;
      IsActive = true;
    }
    public void Update(GameTime gt)
    {
      if (ship != null && ship.Disposed)
        ship = null;
      if (ship == null || !ship.IsActive || !IsActive)
        return;
      else
        DoUpdate(gt);
    }
    protected virtual void DoUpdate(GameTime gt)
    {
      ship.Target = FindNearestTarget();
      if (ship.Target != null && ship.Target.IsActive && ship.Target.Pos != ship.Pos)
      {
        ship.Fire1();
      }
    }
    protected virtual void MoveTowards(Vector2 point)
    {
      if (ship.Pos != point)
      {
        Vector2 direction = point - ship.Pos;
        if (direction != Vector2.Zero)
          ship.Move(direction);
      }
    }
    protected void Seek(Vector2 point)
    {
      Vector2 desired = Vector2.Normalize(point - ship.Pos) * ship.Speed;
      Vector2 steering = desired - ship.Acceleration;
      ship.Acceleration += steering;
      ship.SetAcceleration(Vector2.Normalize(ship.Acceleration) * ship.Speed);
    }
    protected virtual GameObject FindNearestTarget()
    {
      List<GameObject> players = world.GetPlayers();
      GameObject nearest = null;
      float nDistance = 1000000;
      foreach (GameObject p in players)
      {
        if (!p.IsActive)
          continue;
        float distance = Utility.DistanceSquared(p.Pos, ship.Pos);
        if (distance < nDistance)
        {
          nearest = p;
          nDistance = distance;
        }
      }
      return nearest;
    }
    public void SetShip(Ship ship)
    {
      if (this.ship != null)
        this.ship.Controller = null;
      if (ship != null)
        ship.Controller = this;
      this.ship = ship;
    }
  }

}