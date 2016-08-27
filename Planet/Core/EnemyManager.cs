using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class EnemyManager
  {
    World world;
    List<AIController> controllers;
    Queue<Spawn> spawnQueue;
    FrameTimer spawnTimer;

    public EnemyManager(World world)
    {
      controllers = new List<AIController>();
      spawnQueue = new Queue<Spawn>();
      spawnTimer = new FrameTimer();
      this.world = world;
    }
    public void CreateEnemy(Ship enemy, AIController controller)
    {
      controller.SetShip(enemy);
      controllers.Add(controller);
      world.PostGameObj(enemy);
    }
    public void Update(GameTime gt)
    {
      spawnTimer.Update();
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
    }
  }
  internal struct Spawn
  {
    public enum Type
    {
      Spawn,
      Wait
    }
    Type type;
    int value;
    public Spawn(Type type, int value)
    {
      this.type = type;
      this.value = value;
    }
  }
}
