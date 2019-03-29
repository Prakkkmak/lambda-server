using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda.Utils
{
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Position Position => new Position(X, Y, Z);

        public float Magnitude
        {
            get { return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z)); }
        }

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(Vector3 pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }
        public Vector3(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        public static Position Near(Position position, int distance = 2)
        {
            return new Position(position.X + distance, position.Y, position.Z);
        }



        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2) + Math.Pow(v2.Z - v1.Z, 2));
        }
        public static Vector3 Up = new Vector3(0, 1, 0);
        public static Vector3 Left = new Vector3(-1, 0, 0);
        public static Vector3 Down = new Vector3(0, -1, 0);
        public static Vector3 Right = new Vector3(1, 0, 0);
        public static Vector3 Forward = new Vector3(0, 0, 1);
        public static Vector3 Backward = new Vector3(0, 0, -1);
        public static Vector3 Zero = new Vector3(0, 0, 0);
        public static Vector3 One = new Vector3(1, 1, 1);
    }

}
