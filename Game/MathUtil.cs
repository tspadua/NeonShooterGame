
using Microsoft.Xna.Framework;
using System;

namespace NeonShooter
{
    static class MathUtil
    {

        /// <summary> <c>FromPolar</c> returns a Vector by providing its angle and length (magnitude)</summary> ///
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}