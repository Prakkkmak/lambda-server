using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda.Utils
{
    public struct Location
    {
        public Position Position;
        public Interior Interior;
        public short Dimension;

        public Location(Position position, Interior interior, short dimension)
        {
            Position = position;
            Interior = interior;
            Dimension = dimension;
        }
    }
}
