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

    private float waveStrength;
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
    public void SendNextWave(float delay = 0.0f)
    {
      resources += resourcesPerWave + resourcesPerWave2 * WaveCounter - 1;
      waveStrength = resources / 4;
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
          if (spawnQueue.Count > 0 && spawnQueue.Last.Value.enemy is EnemyBoss)
            spawn.timeToSpawn += 20;
          spawnQueue.AddLast(spawn);
          resources -= cost;
        }
      }
      spawnQueue.First.Value.timeToSpawn += delay;
      DequeueSpawn();
    }
    public float GetEnemyStrength()
    {
      float result = 0;
      foreach (GameObject go in world.GetGameObjects())
      {
        if (go is EnemyShip && go.Layer == Layer.ENEMY_SHIP)
        {
          result += ((EnemyShip)go).Cost;
        }
      }
      return result;
    }
    private void DequeueSpawn()
    {
      if (spawnQueue.Count == 0)
        return;
      nextSpawn = spawnQueue.First();
      spawnQueue.RemoveFirst();
      float spawnModifier = 0.5f + 1 * GetEnemyStrength() / waveStrength;
      if (nextSpawn.timeToSpawn == 0)
        SpawnNext();
      else
        spawnTimer.Start(nextSpawn.timeToSpawn * spawnModifier);
    }
    private void SpawnNext()
    {
      if (controllers.Count >= waveStrength)
      {
        spawnTimer.Start(nextSpawn.timeToSpawn / 2);
        return;
      }
      Ship enemy = nextSpawn.enemy;
      world.PostGameObj(enemy);

      AIController sc = nextSpawn.controller;
      sc.IsActive = false;
      sc.SetShip(enemy);
      controllers.Add(sc);
      AudioManager.PlaySound("whoosh2", 0.25f);

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
      float spawnTime = Utility.RandomFloat(0.5f, 2.5f);
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
      EnemyShip ship;
      AIController controller = null;
      switch (shipType)
      {
        case 1:
        default:
          ship = new Enemy1(pos, world);
          break;
        case 2:
          ship = new Enemy2(pos, world);
          break;
        case 3:
          ship = new Enemy3(pos, world);
          controller = new ECWanderer(world, activationTime);
          break;
        case 4:
          ship = new Enemy4(pos, world);
          break;
        case 5:
          ship = new EnemyBoss(pos, world);
          controller = new ECBoss(world, activationTime);
          break;
        case 6:
          ship = new Enemy5(pos, world);
          controller = new ECChaser(world, activationTime, 0.01f, 30.0f, -1);
          break;
      }
      if (controller == null)
        switch (controllerType)
        {
          case 1:
          default:
            controller = new AIController(world, activationTime);
            ship.CostModifier = 1.0f;
            break;
          case 2:
            controller = new ECChaser(world, activationTime);
            ship.CostModifier = 1.2f;
            break;
          case 3:
            controller = new ECWanderer(world, activationTime);
            ship.CostModifier = 1.2f;
            break;
        }
      cost = ship.Cost;
      return new Spawn(ship, controller, spawnTime);
    }
  }
}
