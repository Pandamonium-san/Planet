using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class EnemyManager
  {
    private Timer spawnTimer;
    private World world;
    private List<AIController> controllers;
    private Queue<Spawn> spawnQueue;
    private Spawn nextSpawn;

    private Timer activationTimer;
    private Ship justSpawned;

    private Timer waveTimer;
    private float resources;
    private float resourcesPerSecond;
    private float resourcesPerSecond2;

    public EnemyManager(World world)
    {
      this.world = world;

      controllers = new List<AIController>();
      spawnQueue = new Queue<Spawn>();
      spawnTimer = new Timer(0, SpawnNext, false);

      activationTimer = new Timer(1.0f, ActivateSpawned, false);

      waveTimer = new Timer(0, MakeWave, true);
      resources = 100;
      resourcesPerSecond = 50;
      resourcesPerSecond2 = 1;

      QueueSpawn(new EnemyBoss(new Vector2(500,500), world), new ECBoss(world), 1);
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
      activationTimer.Update(gt);
      spawnTimer.Update(gt);
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }

      resources += resourcesPerSecond * (float)gt.ElapsedGameTime.TotalSeconds;
      resourcesPerSecond += resourcesPerSecond2 * (float)gt.ElapsedGameTime.TotalSeconds;
      waveTimer.Update(gt);
    }
    public void QueueSpawn(Ship enemy, AIController controller, double spawnTime)
    {
      Spawn s = new Spawn(enemy, controller, spawnTime);
      spawnQueue.Enqueue(s);
    }
    private void DequeueSpawn()
    {
      if (spawnQueue.Count == 0)
        return;
      nextSpawn = spawnQueue.Dequeue();
      if (nextSpawn.timeToSpawn == 0)
        SpawnNext();
      else
        spawnTimer.Start(nextSpawn.timeToSpawn);
    }
    private void SpawnNext()
    {
      Type enemyType = nextSpawn.enemy.GetType();
      Type aic = nextSpawn.controller.GetType();

      Ship enemy = (Ship)Activator.CreateInstance(enemyType, new object[] { nextSpawn.enemy.Pos, world });
      justSpawned = enemy;
      enemy.Flash(1.0f, Color.White, false, 1.0f);
      world.PostGameObj(enemy);

      AIController sc = (AIController)Activator.CreateInstance(aic, world);
      sc.IsActive = false;
      controllers.Add(sc);
      sc.SetShip(enemy);

      activationTimer.Start();
      DequeueSpawn();
    }
    public void StartWave(Wave wave)
    {

    }
    private void ActivateSpawned()
    {
      ((AIController)justSpawned.Controller).IsActive = true;
    }
    private void MakeWave()
    {
      int N = Utility.RandomInt(10, 100);
      float nextWave = Utility.RandomInt(2, 15) * (resourcesPerSecond / 100.0f);
      nextWave = MathHelper.Clamp(nextWave, 3, 30);
      for (int i = 0; i < N; i++)
      {
        float cost;
        Spawn spawn = MakeRandomSpawn(out cost);
        if (cost <= resources)
        {
          if (spawn.enemy is EnemyBoss)
            nextWave += 20;
          spawnQueue.Enqueue(spawn);
          resources -= cost;
        }
      }
      waveTimer.Start(nextWave);
      DequeueSpawn();
    }
    private Spawn MakeRandomSpawn(out float cost)
    {
      int ship = Utility.RandomInt(1, 3);
      if (ship == 2)
      {
        ship = Utility.RandomInt(2, 5);
        if (ship == 4)
        {
          ship = Utility.RandomInt(0, 5);
          if (ship == 0)
            ship = 5;
          else
            ship = 4;
        }
      }
      int controller = Utility.RandomInt(1, 4);
      float spawnTime = 1.1f;
      Vector2 pos = new Vector2(Utility.RandomFloat(100, Game1.ScreenWidth - 100), Utility.RandomFloat(100, Game1.ScreenHeight - 100));
      return MakeSpawn(out cost, pos, ship, controller, spawnTime);
    }
    public Spawn MakeSpawn(Vector2 pos, int shipType, int controllerType, double spawnTime = 1)
    {
      float cost;
      return MakeSpawn(out cost, pos, shipType, controllerType, spawnTime);
    }
    public Spawn MakeSpawn(out float cost, Vector2 pos, int shipType, int controllerType, double spawnTime = 1)
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
          controller = new ECWanderer(world);
          cost += 200;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
        case 4:
          ship = new Enemy4(pos, world);
          cost += 500;
          break;
        case 5:
          ship = new EnemyBoss(pos, world);
          controller = new ECBoss(world);
          cost += 5000;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
      }
      switch (controllerType)
      {
        case 1:
        default:
          controller = new AIController(world);
          cost *= 1.0f;
          break;
        case 2:
          controller = new ECChaser(world);
          cost *= 1.2f;
          break;
        case 3:
          controller = new ECWanderer(world);
          cost *= 1.2f;
          break;
      }
      spawn = new Spawn(ship, controller, spawnTime);
      return spawn;
    }
  }
}
