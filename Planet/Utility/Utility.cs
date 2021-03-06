﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  public static class Utility
  {
    private static Random rnd = new Random();

    /// <summary>
    /// Converts a rotation into a direction. 0 rotation means straight up.
    /// </summary>
    /// <param name="radians">Rotation to convert, in radians.</param>
    /// <returns>Returns a normalized vector.</returns>
    public static Vector2 AngleToVector2(float radians)
    {
      Vector2 direction = new Vector2((float)Math.Sin(radians), (float)-Math.Cos(radians));
      return direction;
    }
    /// <summary>
    /// Converts a Vector2 into a rotation. A direction of (0, -1) is 0 rotation.
    /// </summary>
    /// <param name="v">Vector2 to convert.</param>
    /// <returns>Rotation in radians.</returns>
    public static float Vector2ToAngle(Vector2 v)
    {
      return (float)Math.Atan2(v.Y, v.X) + (float)Math.PI / 2f; ;
    }
    public static float AngleBetweenVectors(Vector2 v1, Vector2 v2)
    {
      float cross = v1.X * v2.Y - v1.Y * v2.X;  // find sign of angle
      float dot = Vector2.Dot(v1, v2);
      dot = MathHelper.Clamp(dot, 0.000001f, 0.999999f);  // make sure value stays between 0 and 1, otherwise Acos returns NaN
      float angleDifference = (float)Math.Acos(dot);
      angleDifference = cross < 0 ? -angleDifference : angleDifference;
      return angleDifference;
    }
    public static float Distance(Vector2 v1, Vector2 v2)
    {
      return (v2 - v1).Length();
    }
    public static float DistanceSquared(Vector2 v1, Vector2 v2)
    {
      return (v2 - v1).LengthSquared();
    }
    /// <summary>
    /// Rotate a Vector2 around an origin by amount of radians.
    /// </summary>
    public static Vector2 RotateVector2(Vector2 v, Vector2 origin, float radians)
    {
      Matrix rot = Matrix.CreateRotationZ(radians);
      return Vector2.Transform(v - origin, rot) + origin;
    }
    public static float RandomFloat(float min, float max)
    {
      return (float)(rnd.NextDouble() * (max - min) + min);
    }
    /// <param name="min">Inclusive lower bound</param>
    /// <param name="max">Exclusive upper bound</param>
    public static int RandomInt(int min, int max)
    {
      return rnd.Next(min, max);
    }
    // Raycast against rectangle.
    public static bool RayCast(Vector2 origin, Vector2 dir, Rectangle rec, ref Vector2 intersection, float range = 0)
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
    // Raycast against circle.
    public static bool RayCast(Vector2 start, Vector2 end, Vector2 center, float radius, ref Vector2 intersection)
    {
      Vector2 d = end - start;
      Vector2 f = start - center;

      if (f.Length() <= radius)
        return true;

      float a = Vector2.Dot(d, d);
      float b = 2 * Vector2.Dot(f, d);
      float c = Vector2.Dot(f, f) - radius * radius;

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
          intersection = start + d * t2;
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
    public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int width)
    {
      Texture2D texture;
      texture = AssetManager.GetTexture("laserBlue_m");

      //texture = new Texture2D(1, 1); //fails because of project template?
      //texture.SetData<Color>(new Color[] { Color.White });

      Vector2 edge = end - start;
      float angle = (float)-Math.Atan2(edge.X, edge.Y);
      spriteBatch.Draw(
        texture,
        new Rectangle((int)start.X, (int)start.Y, width, (int)edge.Length()),
        null,
        color,
        angle,
        new Vector2(texture.Width / 2f, 0),
        SpriteEffects.None,
        1);
    }
  }
}
