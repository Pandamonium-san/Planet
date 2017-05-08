using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public class Spawn
  {
    public Ship enemy;
    public AIController controller;
    /// <summary>
    /// Once this object reaches the top of the queue, it will be delayed before spawning
    /// </summary>
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
