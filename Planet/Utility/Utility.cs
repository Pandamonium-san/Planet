using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  public static class Utility
  {
    public static Vector2 AngleToVector2(float radians)
    {
      return new Vector2((float)Math.Sin(radians), (float)-Math.Cos(radians));
    }

    public static float Vector2ToAngle(Vector2 v)
    {

      return (float)Math.Atan2(v.Y, v.X) + (float)Math.PI / 2f; ;
    }

    public static float Distance(Vector2 v1, Vector2 v2)
    {
      return (v2 - v1).Length();
    }

    /// <summary>
    /// Rotate a Vector2 around an origin by amount of radians.
    /// </summary>
    public static Vector2 RotateVector2(Vector2 v, Vector2 origin, float radians)
    {
      Matrix rot = Matrix.CreateRotationZ(radians);
      return Vector2.Transform(v - origin, rot) + origin;
    }

    public static float GetRandom(Random rnd, float min, float max)
    {
      return (float)(rnd.NextDouble() * (max - min) + min);
    }

    public static bool RayTrace(Vector2 org, Vector2 dir, Rectangle rec, ref Vector2 intersection)
    {
      Vector2 rMin = new Vector2((float)rec.X, (float)rec.Y);
      Vector2 rMax = new Vector2(rMin.X + rec.Width, rMin.Y + rec.Height);

      // compute x-planes
      float tMin = (rMin.X - org.X) / dir.X;
      float tMax = (rMax.X - org.X) / dir.X;
      // tMin stays Min
      if (tMin > tMax)
        Swap<float>(ref tMin, ref tMax);
      // compute y-planes
      float tyMin = (rMin.Y - org.Y) / dir.Y;
      float tyMax = (rMax.Y - org.Y) / dir.Y;
      // tyMin stays Min
      if (tyMin > tyMax)
        Swap<float>(ref tyMin, ref tyMax);

      // if ray missed box
      if ((tMin > tyMax) || (tyMin > tMax))
        return false;

      // find which point is on the box
      if (tyMin > tMin)
        tMin = tyMin;
      if (tyMax < tMax)
        tMax = tyMax;

      intersection = org + dir * tMin;
      return true;
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
      T temp = lhs;
      lhs = rhs;
      rhs = temp;
    }
  }
}
