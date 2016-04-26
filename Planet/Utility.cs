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
            angle += (float)Math.PI / 2f;
            return new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));
        }

        public static float Vector2ToAngle(Vector2 v)
        {
            double x = Math.Atan2(v.Y, v.X);
            if (x != 0)
                x = 0;
            return (float)Math.Atan2(v.Y, v.X);
        }

        public static float Wrap(float value, float max, float min)
        {

            return value;
        }
    }
}
