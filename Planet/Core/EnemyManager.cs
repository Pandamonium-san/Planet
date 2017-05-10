using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class EnemyManager
  {
    public int WaveCounter { get; private set; }

    private Timer spawnTimer;
    private World world;
    private List<AIController> controllers;
    private LinkedList<Spawn> spawnQueue;
    private Spawn nextSpawn;

    private float resources;
    private float resourcesPerWave;
    private float resourcesPerWave2;

    public EnemyManager(World world)
    {
      this.world = world;

      controllers = new List<AIController>();
      spawnQueue = new LinkedList<Spawn>();
      spawnTimer = new Timer(0, SpawnNext, false);

      resources = 500;
      resourcesPerWave = 500;
      resourcesPerWave2 = 500;

      spawnQueue.AddFirst(MakeSpawn(new Vector2(500, 500), 4, 3, 1, 3));
      MakeWave();
      //MakeSpawn(new Vector2(500, 500), 5, 4, 1, 3);
      //for (int i = 0; i < 1; i++)
      //{
      //  float x = Utility.RandomFloat(0, 800);
      //  float y = Utility.RandomFloat(0, 600);
      //  float c = 50;
      //  if (i == 0)
      //  else
      //    QueueSpawn(new Enemy3(new Vector2(x, y - c), world), new AIController(world), 0.1f);
      //  //QueueSpawn(new Enemy1(new Vector2(x, y), world), new AIController(world), 0);
      //  //QueueSpawn(new Enemy1(new Vector2(x + c, y), world), new AIController(world), 0);
      //  //QueueSpawn(new Enemy1(new Vector2(x, y + c), world), new AIController(world), 0);
      //  //QueueSpawn(new Enemy1(new Vector2(x - c, y), world), new AIController(world), 0);
      //}
      //Vector2 spawnPos = new Vector2(100, 100);
      //for (int i = 0; i < 15; i++)
      //{
      //  AIController aic;
      //  if (i % 2 == 0)
      //    aic = new ECWanderer(world);
      //  else
      //    aic = new ECChaser(world);
      //  QueueSpawn(new Enemy1(spawnPos, world), aic, 1);
      //  spawnPos += new Vector2(50, 0);
      //}
      //for (int i = 0; i < 7; i++)
      //{
      //  AIController aic;
      //  if (i % 2 == 0)
      //    aic = new AIController(world);
      //  else
      //    aic = new ECWanderer(world);
      //  QueueSpawn(new Enemy2(spawnPos, world), aic, 1);
      //  spawnPos += new Vector2(0, 50);
      //}
      //for (int i = 0; i < 15; i++)
      //{
      //  AIController aic;
      //  aic = new ECWanderer(world);
      //  if (i % 3 == 0)
      //    QueueSpawn(new Enemy3(spawnPos, world), aic, 1);
      //  else
      //    QueueSpawn(new Enemy1(spawnPos, world), aic, 1);
      //  spawnPos += new Vector2(-50, 0);
      //}
      //for (int i = 0; i < 7; i++)
      //{
      //  AIController aic;
      //  if (i % 2 == 0)
      //    aic = new AIController(world);
      //  else
      //    aic = new ECChaser(world);
      //  if (i % 3 == 0)
      //    QueueSpawn(new Enemy4(spawnPos, world), aic, 1);
      //  else
      //    QueueSpawn(new Enemy2(spawnPos, world), aic, 1);
      //  spawnPos += new Vector2(0, -50);
      //}
      //QueueSpawn(new EnemyBoss(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), world), new ECBoss(world), 20);
      //DequeueSpawn();
    }
    public void Update(GameTime gt)
    {
      spawnTimer.Update(gt);
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
      controllers.RemoveAll(ai => ai.GetShip() == null);

      if (controllers.Count == 0 && spawnQueue.Count == 0)
      {
        resources += resourcesPerWave + resourcesPerWave2 * WaveCounter - 1;
        MakeWave();
      }
    }
    private void DequeueSpawn()
    {
      if (spawnQueue.Count == 0)
        return;
      nextSpawn = spawnQueue.First();
      spawnQueue.RemoveFirst();
      if (nextSpawn.timeToSpawn == 0)
        SpawnNext();
      else
        spawnTimer.Start(nextSpawn.timeToSpawn);
    }
    private void SpawnNext()
    {
      Ship enemy = nextSpawn.enemy;
      enemy.Flash(1.0f, Color.White, false, 1.0f);
      world.PostGameObj(enemy);

      AIController sc = nextSpawn.controller;
      sc.IsActive = false;
      controllers.Add(sc);
      sc.SetShip(enemy);

      DequeueSpawn();
    }
    private void MakeWave()
    {
      ++WaveCounter;
      if (WaveCounter == 10)
      {
        spawnQueue.AddLast(MakeSpawn(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight), 5, 4, 1, 3));
        resources -= 5000;
      }
      while (resources > 150)
      {
        float cost;
        Spawn spawn = MakeRandomSpawn(out cost);
        if (cost <= resources)
        {
          spawnQueue.AddLast(spawn);
          resources -= cost;
        }
      }
      DequeueSpawn();
    }
    private Spawn MakeRandomSpawn(out float cost)
    {
      float P = Utility.RandomFloat(0, 1);
      int ship;
      if (P > 0.5f)
        ship = 1;
      else if (P > 0.30f)
        ship = 2;
      else if (P > 0.10f)
        ship = 3;
      else if (P > 0.05f)
        ship = 4;
      else
        ship = 5;
      int controller = Utility.RandomInt(1, 4);
      float spawnTime = Utility.RandomFloat(0.25f, 3.0f);
      if (ship == 5)
        spawnTime += 10;
      Vector2 pos = new Vector2(Utility.RandomFloat(100, Game1.ScreenWidth - 100), Utility.RandomFloat(100, Game1.ScreenHeight - 100));
      return MakeSpawn(out cost, pos, ship, controller, spawnTime);
    }
    public Spawn MakeSpawn(Vector2 pos, int shipType, int controllerType, double spawnTime = 1, double activationTime = 1)
    {
      float cost;
      return MakeSpawn(out cost, pos, shipType, controllerType, spawnTime);
    }
    public Spawn MakeSpawn(out float cost, Vector2 pos, int shipType, int controllerType, double spawnTime = 1, double activationTime = 1)
    {
      Spawn spawn;
      Ship ship;
      AIController controller;
      cost = 0;
      switch (shipType)
      {
        case 1:
        default:
          ship = new Enemy1(pos, world);
          cost += 100;
          break;
        case 2:
          ship = new Enemy2(pos, world);
          cost += 300;
          break;
        case 3:
          ship = new Enemy3(pos, world);
          controller = new ECWanderer(world, activationTime);
          cost += 200;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
        case 4:
          ship = new Enemy4(pos, world);
          cost += 500;
          break;
        case 5:
          ship = new EnemyBoss(pos, world);
          controller = new ECBoss(world, activationTime);
          cost += 5000;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
      }
      switch (controllerType)
      {
        case 1:
        default:
          controller = new AIController(world, activationTime);
          cost *= 1.0f;
          break;
        case 2:
          controller = new ECChaser(world, activationTime);
          cost *= 1.2f;
          break;
        case 3:
          controller = new ECWanderer(world, activationTime);
          cost *= 1.2f;
          break;
      }
      spawn = new Spawn(ship, controller, spawnTime);
      return spawn;
    }
  }
}
