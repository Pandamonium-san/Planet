﻿using System;
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

    public AIController(World world)
    {
      this.world = world;
      IsActive = true;
      targetRefreshTime = 1;
      targetRefresher = new Timer(targetRefreshTime, FindNearestTarget, true);
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
      if (targetRefresher.Counting)
        targetRefresher.Update(gt);
      else
        targetRefresher.Start(targetRefreshTime);
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
      foreach (Ship ship in players)
      {
        if (!ship.IsActive || ship.Untargetable)
          continue;
        float distance = Utility.DistanceSquared(ship.Pos, this.ship.Pos);
        if (distance < nDistance)
        {
          nearest = ship;
          nDistance = distance;
        }
      }
      ship.Target = nearest;
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