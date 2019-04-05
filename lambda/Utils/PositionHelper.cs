using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda.Utils
{
    public class PositionHelper
    {
        public static float Distance(Position pos1, Position pos2)
        {
            return (float)Math.Sqrt(Math.Pow(pos2.X - pos1.X, 2) + Math.Pow(pos2.Y - pos1.Y, 2) + Math.Pow(pos2.Z - pos1.Z, 2));
        }
    }
}
