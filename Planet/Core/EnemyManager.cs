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
    //Queue<Spawn> spawnQueue;
    SortedSet<Spawn> spawns;
    FrameTimer spawnTimer;
    
    public EnemyManager(World world)
    {
      controllers = new List<AIController>();
      //spawnQueue = new Queue<Spawn>();
      spawns = new SortedSet<Spawn>(new SpawnComp());
      this.world = world;

      for (int i = 0; i < 98; i++)
      {
        float x = Utility.RandomFloat(0, 800);
        float y = Utility.RandomFloat(0, 600);
        AddToQueue(new PumpkinShip(new Vector2(x, y), world), new AIController(world), 1*i);
      }
      spawnTimer = new FrameTimer(spawns.Min.spawnFrame - world.Frames, SpawnEnemy, true);
    }
    public void Update(GameTime gt)
    {
      spawnTimer.Update();
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
    }
    void SpawnEnemy()
    {
      CreateEnemy(spawns.Min);
      spawns.Remove(spawns.Min);
      if (spawns.Count != 0)
      {
        spawnTimer = new FrameTimer(spawns.Min.spawnFrame - world.Frames, SpawnEnemy, true);
        spawnTimer.Start();
      }
    }
    void CreateEnemy(Spawn spawn)
    {
      controllers.Add(spawn.controller);
      world.PostGameObj(spawn.enemy);
    }
    public void AddToQueue(Ship enemy, AIController controller, int spawnFrame)
    {
      Spawn s = new Spawn(enemy, controller, spawnFrame);
      spawns.Add(s);
    }
    class Spawn
    {
      public Ship enemy;
      public AIController controller;
      public int spawnFrame;
      public Spawn(Ship enemy, AIController controller, int spawnFrame)
      {
        this.enemy = enemy;
        this.controller = controller;
        this.spawnFrame = spawnFrame;
        this.controller.SetShip(this.enemy);
      }
    }
    class SpawnComp : Comparer<Spawn>
    {
      public override int Compare(Spawn x, Spawn y)
      {
        return x.spawnFrame.CompareTo(y.spawnFrame);
      }
    }
  }
}
