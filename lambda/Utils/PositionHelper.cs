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
        public static Position PositionInAngle(Position pos, Rotation angle, float distance)
        {
            float x = pos.X;
            float y = pos.Y;
            x += distance * (float)Math.Sin(angle.Yaw);
            y += distance * (float)Math.Cos(angle.Yaw);
            return new Position(x, y, pos.Z);
        }
    }
}
