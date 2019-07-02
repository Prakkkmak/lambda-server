using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Housing
{
    public class House : IDBElement
    {
        public uint Id { get; set; } = 0;
        public uint Owner = 0;
        public uint Renter = 0;
        public uint Rent = 0;

        public string Name = "House";

        public Checkpoint Exterior = null;
        public Checkpoint Interior = null;

        public List<string> Ipls = new List<string>();

        public Inventory Inventory = null;

        public House(Position position, short dimension = 0)
        {
            Exterior = new Checkpoint(position, dimension, player => { });
            Houses.Add(this);
        }
        public void SetInterior(Position position)
        {
            Interior = new Checkpoint(position, (short)Id, player =>
           {
               if (Exterior == null) return;
               player.Goto(Exterior);
               player.UnloadIpl(Ipls);
           });
        }

        public void SetExterior(Position position)
        {
            Exterior = new Checkpoint(position, 0, player =>
            {
                if (Interior == null) return;
                player.Goto(Interior);
                player.LoadIpl(Ipls);
            });
        }
        public void SetIpls(string ipls)
        {
            Ipls = ipls.Split(",").ToList();
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["hou_rent"] = Rent.ToString();
            data["hou_ipls"] = string.Join(",", Ipls);
            if (Owner != 0) data["hou_owner"] = Owner.ToString();
            if (Renter != 0) data["hou_renter"] = Renter.ToString();
            if (Interior != null) data["hou_interior"] = PositionHelper.StringFromPosition(Interior.Position);
            if (Exterior != null) data["hou_exterior"] = PositionHelper.StringFromPosition(Exterior.Position);

            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Rent = Convert.ToUInt32(data["hou_rent"]);
            Ipls = data["hou_ren"].Split(',').ToList();
            if (!string.IsNullOrWhiteSpace(data["hou_owner"])) Owner = Convert.ToUInt32(data["hou_owner"]);
            if (!string.IsNullOrWhiteSpace(data["hou_renter"])) Renter = Convert.ToUInt32(data["hou_renter"]);
            if (!string.IsNullOrWhiteSpace(data["hou_interior"]))
                SetInterior(PositionHelper.PositionFromString(data["hou_interior"]));
            if (!string.IsNullOrWhiteSpace(data["hou_exterior"]))
                SetExterior(PositionHelper.PositionFromString(data["hou_exterior"]));
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("House Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Delete(this);
            Alt.Log("House deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("House Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task DeleteAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.DeleteAsync(this);
            Alt.Log("House deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public static List<House> Houses = new List<House>();
    }
}
