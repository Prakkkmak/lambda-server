using AltV.Net.Data;
using Lambda.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Utils
{
    public class Interior : IDBElement

    {
        public uint Id { get; set; }
        public string IPL { get; set; }
        public Position Position { get; set; }
        public Interior()
        {
            Id = 0;
            IPL = "";
            Position = new Position(0, 0, 0);
        }
    }
}
