using AltV.Net.Data;
using Lambda.Database;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Lambda.Entity;

namespace Lambda.Utils
{
    public class Interior : IDBElement

    {
        public uint Id { get; set; }
        public string Name { get; set; }
        private string[] IPLs { get; set; }
        public Position Position { get; set; }
        public Area Area { get; set; }
        public Interior()
        {
            Id = 0;
            IPLs = new string[0];
            Position = new Position(0, 0, 0);
        }


        public Interior Clone()
        {
            Interior interior = new Interior();
            interior.Position = this.Position;
            interior.IPLs = this.IPLs;
            interior.Id = 0;
            interior.Area = Area;
            return interior;
        }

        public void SetIPLs(string iplstr)
        {
            iplstr.Replace(" ", "");
            IPLs = iplstr.Split(",");
        }

        public string[] GetIPLs()
        {
            return IPLs;
        }
    }
}
