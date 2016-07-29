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

    public static bool RayTrace(Vector2 origin, Vector2 dir, Rectangle rec, ref Vector2 intersection, float range = 0)
    {
      Vector2 rMin = new Vector2((float)rec.X, (float)rec.Y);
      Vector2 rMax = new Vector2(rMin.X + rec.Width, rMin.Y + rec.Height);

      // compute x-planes
      float tMin = (rMin.X - origin.X) / dir.X;
      float tMax = (rMax.X - origin.X) / dir.X;
      // tMin stays Min
      if (tMin > tMax)
        Swap<float>(ref tMin, ref tMax);
      // compute y-planes
      float tyMin = (rMin.Y - origin.Y) / dir.Y;
      float tyMax = (rMax.Y - origin.Y) / dir.Y;
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

      intersection = origin + dir * tMin;
      if (range > 0 && (intersection - origin).Length() > range)
        return false;
      return true;
    }
    public static bool RayCastCircle(Vector2 start, Vector2 end, Vector2 center, float radius, ref Vector2 intersection)
    {
      intersection = start;

      Vector2 d = end - start;
      Vector2 f = start - center;

      if (f.Length() <= radius)
        return true;

      float a = Vector2.Dot(d,d);
      float b = 2 * Vector2.Dot(f,d);
      float c = Vector2.Dot(f,f) - radius * radius;

      float discriminant = b * b - 4 * a * c;
      if (discriminant < 0)
      {
        return false;
      }
      else
      {
        // ray didn't totally miss sphere,
        // so there is a solution to
        // the equation.

        discriminant = (float)Math.Sqrt(discriminant);

        // either solution may be on or off the ray so need to test both
        // t1 is always the smaller value, because BOTH discriminant and
        // a are nonnegative.
        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        // 3x HIT cases:
        //          -o->             --|-->  |            |  --|->
        // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

        // 3x MISS cases:
        //       ->  o                     o ->              | -> |
        // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

        if (t1 >= 0 && t1 <= 1)
        {
          // t1 is the intersection, and it's closer than t2
          // (since t1 uses -b - discriminant)
          // Impale, Poke
          intersection = start + d * t1;
          return true;
        }

        // here t1 didn't intersect so we are either started
        // inside the sphere or completely past it
        if (t2 >= 0 && t2 <= 1)
        {
          // ExitWound
          return true;
        }

        // no intn: FallShort, Past, CompletelyInside
        return false;
      }
    }
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
      T temp = lhs;
      lhs = rhs;
      rhs = temp;
    }
  }
}
