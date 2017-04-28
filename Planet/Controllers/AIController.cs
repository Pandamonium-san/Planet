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
    protected Ship ship;
    protected World world;
    protected List<Command> commands = new List<Command>();

    public AIController(World world)
    {
      this.world = world;
      commands = new List<Command>();
      AddCommand(CommandType.SetVelocity, -10, 0, 0, 1);
      AddCommand(CommandType.AddVelocity, 5, 5, 0, 200);
      AddCommand(CommandType.AddVelocity, 5, -5, 200, 400);
      AddCommand(CommandType.AddVelocity, -5, -5, 400, 600);
      AddCommand(CommandType.AddVelocity, -5, 5, 600, 800);
      AddCommand(CommandType.SetVelocity, 0, 0, 800, 801);

      //AddCommand(Command.Type.AddVelocity, 0, 1, 0, 100);

      //AddCommand(Command.Type.Move, -1, 0, 0, 300);
      //AddCommand(Command.Type.Move, 0, 1, 200, 600);
      //AddCommand(Command.Type.Move, 1, 0, 600, 900);
      //AddCommand(Command.Type.Rotate, -0.1f, 0, 0, 10000);
      //AddCommand(CommandType.LookAtTarget, 0, 0, 0, 10000);
      AddCommand(CommandType.Fire, 0, 0, 0, 10000);
    }
    public void Update(GameTime gt)
    {
      if (ship != null && ship.Disposed)
        ship = null;
      if (ship == null || !ship.IsActive)
        return;
      else
        DoUpdate(gt);
    }
    protected virtual void DoUpdate(GameTime gt)
    {
      ship.Target = FindNearestTarget();
      if (ship.Target != null && ship.Target.IsActive && ship.Target.Pos != ship.Pos)
      {
        foreach (Command command in commands)
        {
          if (command.startFrame <= ship.frame && ship.frame < command.endFrame)
            ExecuteCommand(command);
        }
      }
    }
    protected virtual void MoveTowards(Vector2 point)
    {
      if (ship.Pos != point)
      {
        Vector2 direction = point - ship.Pos;
        ship.Move(direction);
      }
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
    public void AddCommand(CommandType type, float x, float y, int startFrame, int endFrame)
    {
      commands.Add(new Command(type, x, y, startFrame, endFrame));
    }

    public void ExecuteCommand(Command c)
    {
      switch (c.type)
      {
        case CommandType.Move:
          ship.Move(new Vector2(c.x, c.y));
          break;
        case CommandType.MoveTo:
          MoveTowards(new Vector2(c.x, c.y));
          break;
        case CommandType.Fire:
          ship.Fire1();
          break;
        case CommandType.LookAtPoint:
          ship.TurnTowards(new Vector2(c.x, c.y));
          break;
        case CommandType.LookAtTarget:
          ship.TurnTowards(ship.Target.Pos);
          break;
        case CommandType.Rotate:
          ship.Rotation += c.x;
          break;
        case CommandType.SetVelocity:
          ship.SetAcceleration(new Vector2(c.x, c.y));
          break;
        case CommandType.AddVelocity:
          ship.AddAcceleration(new Vector2(c.x, c.y));
          break;
        default:
          break;
      }
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