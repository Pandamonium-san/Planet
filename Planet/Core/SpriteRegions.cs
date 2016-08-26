using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public static class SpriteRegions
  {
    private static Dictionary<string, Rectangle> spriteRectangles = new Dictionary<string, Rectangle>
    {
      { "Ship1", new Rectangle(0, 0, 15, 13) },
      { "Ship2", new Rectangle(0, 14, 9, 14) },
      { "Ship3", new Rectangle(0, 29, 9, 8) },
      { "Slime1", new Rectangle(0, 38, 10, 10) },
      { "Slime2", new Rectangle(0, 49, 16, 10) },
      { "Slime3", new Rectangle(0, 60, 21, 11) },
      { "Slime4", new Rectangle(0, 72, 25, 18) },
      { "Drone1", new Rectangle(0, 91, 8, 12) },
      { "Drone2", new Rectangle(0, 104, 10, 17) },
      { "Drone3", new Rectangle(0, 122, 20, 24) }
    };

    public static Rectangle Get(string name)
    {
      return spriteRectangles[name];
    }
  }
}
