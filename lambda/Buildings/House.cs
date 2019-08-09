using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Buildings;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Buildings
{
    public class House : Building
    {
        public uint Renter = 0;
        public uint Rent = 0;

        public Checkpoint InteriorCheckpoint = null;

        public List<string> Ipls = new List<string>();

        public Inventory Inventory = null;

        public House(Position position, short dimension = 0) : base(position, dimension)
        {
            Checkpoint.Color = new Rgba(0, 100, 100, 100);
            Checkpoint.DrawCheckpoint();
            Houses.Add(this);
        }
        public void SetInterior(Position position)
        {
            if (InteriorCheckpoint != null)
            {
                Checkpoint.Checkpoints.Remove(InteriorCheckpoint);
                InteriorCheckpoint.AltCheckpoint.Remove();
            }
            InteriorCheckpoint = new Checkpoint(position, (short)Id, player =>
            {
                if (Checkpoint == null) return;
                player.Goto(Checkpoint);
                player.UnloadIpl(Ipls);
            });
        }

        public void SetExterior(Position position)
        {
            if(Checkpoint != null)
            {
                Checkpoint.Checkpoints.Remove(Checkpoint);
                Checkpoint.AltCheckpoint.Remove();
            }

            Checkpoint = new Checkpoint(position, 0, player =>
            {
                if (InteriorCheckpoint == null) return;
                player.Goto(InteriorCheckpoint);
                player.LoadIpl(Ipls);
            });
        }
        public void SetIpls(string ipls)
        {
            Ipls = ipls.Split(",").ToList();
        }

        public override Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = base.GetData();
            data["hou_rent"] = Rent.ToString();
            data["hou_ipls"] = string.Join(",", Ipls);
            if (Renter != 0) data["hou_renter"] = Renter.ToString();
            if (InteriorCheckpoint != null) data["hou_interior"] = PositionHelper.StringFromPosition(InteriorCheckpoint.Position);
            return data;
        }

        public override void SetData(Dictionary<string, string> data)
        {
            base.SetData(data);
            Rent = Convert.ToUInt32(data["hou_rent"]);
            Ipls = data["hou_rent"].Split(',').ToList();
            //Price = Convert.ToUInt32(data["hou_price"]);
            // if (!string.IsNullOrWhiteSpace(data["hou_renter"])) Renter = Convert.ToUInt32(data["hou_renter"]);
            if (data.ContainsKey("hou_interior") && !string.IsNullOrWhiteSpace(data["hou_interior"]))
                SetInterior(PositionHelper.PositionFromString(data["hou_interior"]));
        }

        public override void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("House Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public override void Delete()
        {
           
            long t = DateTime.Now.Ticks;
            DatabaseElement.Delete(this);
            Alt.Log("House deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
            base.Delete();
        }

        public override async Task SaveAsync()
        {
            await DatabaseElement.SaveAsync(this);
        }

        public override async Task DeleteAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.DeleteAsync(this);
            Alt.Log("House deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
            await base.DeleteAsync();
        }
        public static void LoadHouses()
        {
            Houses.AddRange(DatabaseElement.GetAllHouses());
        }

        public static List<House> Houses = new List<House>();
    }
}
