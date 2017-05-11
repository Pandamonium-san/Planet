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
    protected double targetRefreshTime;
    protected Timer targetRefresher;
    protected Timer activationTimer;

    public AIController(World world, double activationTime = 0)
    {
      IsActive = false;
      activationTimer = new Timer(activationTime, () => IsActive = true, true, false);
      this.world = world;
      targetRefreshTime = 1;
      targetRefresher = new Timer(targetRefreshTime, FindNearestTarget, true, true);
    }
    public void Update(GameTime gt)
    {
      activationTimer.Update(gt);
      if (ship != null && ship.Disposed)
        ship = null;
      if (ship == null || !ship.IsActive || !IsActive)
        return;
      else
        DoUpdate(gt);
    }
    protected virtual void DoUpdate(GameTime gt)
    {
      targetRefresher.Update(gt);
      if (ship.Target != null && ship.Target.IsActive && !ship.Target.Untargetable)
      {
        ship.Fire1();
      }
    }
    protected void MoveTowards(Vector2 point, bool dash = false)
    {
      if (ship.Pos != point)
      {
        Vector2 direction = point - ship.Pos;
        if (direction != Vector2.Zero)
          ship.Move(direction);
        ship.SetDash(dash);
      }
    }
    protected void FindNearestTarget()
    {
      List<Ship> players = world.GetPlayers();
      Ship nearest = null;
      float nDistance = 1000000;
      foreach (Ship s in players)
      {
        if (!s.IsActive || s.Untargetable)
          continue;
        float distance = Utility.Distance(s.Pos, this.ship.Pos);
        if (distance < nDistance)
        {
          nearest = s;
          nDistance = distance;
        }
      }
      ship.Target = nearest;
    }
    public Ship GetShip()
    {
      return ship;
    }
    public void SetShip(Ship ship)
    {
      if (this.ship != null)
        this.ship.Controller = null;
      if (ship != null)
      {
        ship.Flash((float)activationTimer.Remaining, Color.White, false, 1.0f);
        ship.Controller = this;
      }
      this.ship = ship;
    }
  }

}