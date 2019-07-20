using AltV.Net;
using AltV.Net.Data;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lambda.Buildings
{
    public abstract class Building : IDBElement
    {
        public uint Id { get; set; } = 0;
        public uint Owner = 0;
        public uint Price = 0;
        public uint Money = 0;


        public string Name = "";

        public Checkpoint Checkpoint = null;

        public Blip Blip = null;
        public Building(Position pos, short dimension = 0)
        {
            Checkpoint = new Checkpoint(pos, dimension, player => { });
            Blip = new Blip(pos, dimension);
        }
        public virtual Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["bui_type"] = 0 + "";
            data["bui_position"] = PositionHelper.StringFromPosition(Checkpoint.Position);
            if (Owner != 0) data["bui_owner"] = Owner.ToString();
            data["bui_price"] = Price.ToString();
            data["bui_money"] = Money.ToString();
            return data;
        }

        public virtual void SetData(Dictionary<string, string> data)
        {
            Checkpoint = new Checkpoint(PositionHelper.PositionFromString(data["bui_position"]), 0, (p) => { });
            if (!string.IsNullOrWhiteSpace(data["bui_owner"])) Owner = Convert.ToUInt32(data["bui_owner"]);
            Price = Convert.ToUInt32(data["bui_price"]);
            Money = Convert.ToUInt32(data["bui_money"]);
        }

        public virtual void Save()
        {

            DatabaseElement.Save(this);
        }

        public virtual void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public virtual async Task SaveAsync()
        {
            await DatabaseElement.SaveAsync(this);
        }

        public virtual async Task DeleteAsync()
        {
            await DatabaseElement.DeleteAsync(this);
        }
    }
}
