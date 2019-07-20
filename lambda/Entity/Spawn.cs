using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda
{
    public class Spawn
    {

        public Position Position { get; set; }

        public Spawn(Position pos)
        {
            Position = pos;
        }
        public static Spawn NewSpawn = new Spawn(new Position(1128.5f, 2661.917f, 38));

    }
}
