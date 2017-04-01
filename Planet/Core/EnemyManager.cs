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
    World world;
    List<AIController> controllers;
    Dictionary<int, List<Spawn>> spawnDict;
    FrameTimer rewindTimer;
    bool rewinding;
    
    public EnemyManager(World world)
    {
      this.world = world;

      controllers = new List<AIController>();
      spawnDict = new Dictionary<int, List<Spawn>>();

      for (int i = 0; i < 98; i++)
      {
        float x = Utility.RandomFloat(0, 800);
        float y = Utility.RandomFloat(0, 600);
        float c = 50;
        AddSpawn(new PumpkinShip(new Vector2(x, y), world), new AIController(world), 600 * i);
        AddSpawn(new PumpkinShip(new Vector2(x+c, y), world), new AIController(world), 600 * i);
        AddSpawn(new PumpkinShip(new Vector2(x, y+c), world), new AIController(world), 600 * i);
        AddSpawn(new PumpkinShip(new Vector2(x-c, y), world), new AIController(world), 600 * i);
        AddSpawn(new PumpkinShip(new Vector2(x, y-c), world), new AIController(world), 600 * i);
      }
      //AddSpawn(new PumpkinShip(new Vector2(500, 500), world), new AIController(world), 50);
    }
    public void Update(GameTime gt)
    {
      if (rewinding)
      {
        --frames;
        rewindTimer.Update();
        return;
      }
      if(spawnDict.ContainsKey(frames))
      {
        foreach(Spawn s in spawnDict[frames])
        {
          CreateEnemy(s);
        }
      }
      foreach (AIController aic in controllers)
      {
        aic.Update(gt);
      }
      ++frames;
    }
    public void StartRewind(int x)
    {
      rewinding = true;
      rewindTimer = new FrameTimer(x, () => rewinding = false, true);
    }
    void CreateEnemy(Spawn spawn)
    {
      Type enemyType = spawn.enemy.GetType();
      Type aic = spawn.controller.GetType();

      Ship enemy = (Ship)Activator.CreateInstance(enemyType, new object[] { spawn.enemy.Pos, world });
      AIController sc = (AIController)Activator.CreateInstance(aic, world);
      sc.SetShip(enemy);

      controllers.Add(sc);
      world.PostGameObj(enemy);
    }
    public void AddSpawn(Ship enemy, AIController controller, int spawnFrame)
    {
      Spawn s = new Spawn(enemy, controller, spawnFrame);
      if(spawnDict.ContainsKey(spawnFrame))
      {
        spawnDict[spawnFrame].Add(s);
      }
      else
      {
        List<Spawn> spawnlist = new List<Spawn>();
        spawnlist.Add(s);
        spawnDict[spawnFrame] = spawnlist;
      }
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
  }
}
