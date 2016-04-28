using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
    public static class Utility
    {
        public static Vector2 AngleToVector2(float angle)
        {
            //angle += (float)Math.PI / 2f;
            return new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));
        }

        public static float Vector2ToAngle(Vector2 v)
        {

            return (float)Math.Atan2(v.Y, v.X) + (float)Math.PI / 2f; ;
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return (v2 - v1).Length();
        }
    }
}
