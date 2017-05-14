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

      //spawnQueue.AddFirst(MakeSpawn(new Vector2(500, 500), 4, 3, 1, 3));
      //spawnQueue.AddFirst(MakeSpawn(new Vector2(500, 500), 5, 4, 1, 3));
      //DequeueSpawn();
      //SendNextWave();
    }
    public void Update(GameTime gt)
    {
      spawnTimer.Update(gt);
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
      controllers.RemoveAll(ai => ai.GetShip() == null);
    }
    public bool WaveDefeated()
    {
      return controllers.Count == 0 && spawnQueue.Count == 0 && !spawnTimer.Counting;
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
    public void SendNextWave(float delay = 0.0f)
    {
      resources += resourcesPerWave + resourcesPerWave2 * WaveCounter - 1;
      ++WaveCounter;
      if (WaveCounter == 10)
      {
        spawnQueue.AddLast(MakeSpawn(new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), 5, 4, 3, 2));
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
      spawnQueue.First.Value.timeToSpawn += delay;
      DequeueSpawn();
    }
    private void SpawnNext()
    {
      Ship enemy = nextSpawn.enemy;
      world.PostGameObj(enemy);

      AIController sc = nextSpawn.controller;
      sc.IsActive = false;
      sc.SetShip(enemy);
      controllers.Add(sc);

      DequeueSpawn();
    }
    private Spawn MakeRandomSpawn(out float cost)
    {
      float P = Utility.RandomFloat(0, 1);
      int ship;
      if (P > 0.6f)
        ship = 1;
      else if (P > 0.45f)
        ship = 2;
      else if (P > 0.30f)
        ship = 3;
      else if (P > 0.15f)
        ship = 6;
      else if (P > 0.075f)
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
      return MakeSpawn(out cost, pos, shipType, controllerType, spawnTime, activationTime);
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
          cost += 150;
          break;
        case 2:
          ship = new Enemy2(pos, world);
          cost += 300;
          break;
        case 3:
          ship = new Enemy3(pos, world);
          controller = new ECWanderer(world, activationTime);
          cost += 250;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
        case 4:
          ship = new Enemy4(pos, world);
          cost += 450;
          break;
        case 5:
          ship = new EnemyBoss(pos, world);
          controller = new ECBoss(world, activationTime);
          cost += 5000;
          spawn = new Spawn(ship, controller, spawnTime);
          return spawn;
        case 6:
          ship = new Enemy5(pos, world);
          controller = new ECChaser(world, activationTime, 0.05f, 5.0f, 60);
          cost += 40;
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
