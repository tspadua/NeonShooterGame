
using Microsoft.Xna.Framework;
using System;

namespace NeonShooter
{
    static class Extensions
    {
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
        public static Vector2 ScaleTo(this Vector2 vector, float length)
        {
            return vector * (length / vector.Length());
        }

        /// <summary> <c>NextFloat</c> generates Random Numbers in a custom range </summary> ///
        public static float NextFloat(this Random rand, float minValue, float maxValue)
        {
            // NextDouble has a range of 0 to 1, so we multiply it for its range and add the base
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}