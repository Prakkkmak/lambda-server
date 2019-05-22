using AltV.Net.Data;
using Lambda.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lambda.Entity;

namespace Lambda.Utils
{
    public class Interior : IDBElement

    {
        public uint Id { get; set; }


        public string Name { get; set; }
        private List<string> IPLs { get; set; }
        public Position Position { get; set; }
        public Area Area { get; set; }
        public Interior()
        {
            Id = 0;
            IPLs = new List<string>();
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
            IPLs = iplstr.Split(",").ToList();
        }

        public void AddIpl(string iplstr)
        {
            IPLs.Add(iplstr);
        }

        public string[] GetIPLs()
        {
            return IPLs.ToArray();
        }

        public Dictionary<string, string> GetData()
        {
            return null;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Id = uint.Parse(data["int_id"]);
            SetIPLs(data["int_ipl"]);
            Position pos = new Position();
            pos.X = int.Parse(data["int_position_x"]);
            pos.Y = int.Parse(data["int_position_y"]);
            pos.Z = int.Parse(data["int_position_z"]);
            Position = pos;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public static void LoadInteriors()
        {
            Interiors = DatabaseElement.GetAllInteriors().ToList();
        }

        public static Interior GetInterior(uint id)
        {
            foreach (Interior interior in Interiors)
            {
                if (interior.Id == id) return interior;
            }
            return null;
        }
        public static Interior[] GetInteriors(string name)
        {
            List<Interior> inters = new List<Interior>();
            foreach (Interior interior in Interiors)
            {
                if (interior.Id.ToString().Equals(name)) inters.Add(interior);
                if (interior.Name.Replace(" ", "_").StartsWith(name)) inters.Add(interior);
            }
            return inters.ToArray();
        }

        public static List<Interior> Interiors = new List<Interior>();
    }
}
