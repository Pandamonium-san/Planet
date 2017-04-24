using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class EnemyManager
  {
    int frames;
    Timer spawnTimer;
    World world;
    List<AIController> controllers;
    Queue<Spawn> spawnQueue;
    Spawn nextSpawn;

    public EnemyManager(World world)
    {
      this.world = world;

      controllers = new List<AIController>();
      spawnQueue = new Queue<Spawn>();
      spawnTimer = new Timer(0, SpawnNext, false);

      //for (int i = 0; i < 98; i++)
      //{
      //  float x = Utility.RandomFloat(0, 800);
      //  float y = Utility.RandomFloat(0, 600);
      //  float c = 50;
      //  if (i == 0)
      //    QueueSpawn(new Enemy1(new Vector2(x, y - c), world), new AIController(world), 0);
      //  else
      //    QueueSpawn(new Enemy1(new Vector2(x, y - c), world), new AIController(world), 10);
      //  QueueSpawn(new Enemy1(new Vector2(x, y), world), new AIController(world), 0);
      //  QueueSpawn(new Enemy1(new Vector2(x + c, y), world), new AIController(world), 0);
      //  QueueSpawn(new Enemy1(new Vector2(x, y + c), world), new AIController(world), 0);
      //  QueueSpawn(new Enemy1(new Vector2(x - c, y), world), new AIController(world), 0);
      //}

      Vector2 spawnPos = new Vector2(100, 100);
      for (int i = 0; i < 15; i++)
      {
        QueueSpawn(new Enemy1(spawnPos, world), new AIController(world), 0.1);
        spawnPos += new Vector2(50, 0);
      }
      for (int i = 0; i < 7; i++)
      {
        QueueSpawn(new Enemy1(spawnPos, world), new AIController(world), 0.1);
        spawnPos += new Vector2(0, 50);
      }
      for (int i = 0; i < 15; i++)
      {
        QueueSpawn(new Enemy1(spawnPos, world), new AIController(world), 0.1);
        spawnPos += new Vector2(-50, 0);
      }
      for (int i = 0; i < 7; i++)
      {
        QueueSpawn(new Enemy1(spawnPos, world), new AIController(world), 0.1);
        spawnPos += new Vector2(0, -50);
      }
      DequeueSpawn();
    }
    public void Update(GameTime gt)
    {
      ++frames;
      spawnTimer.Update(gt);
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
    }
    public void QueueSpawn(Ship enemy, AIController controller, double spawnTime)
    {
      Spawn s = new Spawn(enemy, controller, spawnTime);
      spawnQueue.Enqueue(s);
    }
    void DequeueSpawn()
    {
      if (spawnQueue.Count == 0)
        return;
      nextSpawn = spawnQueue.Dequeue();
      if (nextSpawn.timeToSpawn == 0)
        SpawnNext();
      else
        spawnTimer.Start(nextSpawn.timeToSpawn);
    }
    void SpawnNext()
    {
      Type enemyType = nextSpawn.enemy.GetType();
      Type aic = nextSpawn.controller.GetType();

      Ship enemy = (Ship)Activator.CreateInstance(enemyType, new object[] { nextSpawn.enemy.Pos, world });
      AIController sc = (AIController)Activator.CreateInstance(aic, world);
      sc.SetShip(enemy);

      controllers.Add(sc);
      world.PostGameObj(enemy);

      DequeueSpawn();
    }

    class Spawn
    {
      public Ship enemy;
      public AIController controller;
      public double timeToSpawn;
      public Spawn(Ship enemy, AIController controller, double time)
      {
        this.enemy = enemy;
        this.controller = controller;
        this.timeToSpawn = time;
        this.controller.SetShip(this.enemy);
      }
    }
  }
}
