using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Items
{
    public class Item : IDBElement
    {
        private Inventory inventory;
        private BaseItem baseItem;


        public uint Id { get; set; }

        public uint Amount { get; set; }
        public string MetaData { get; set; }


        public Item()
        {
            MetaData = "";
        }

        public Item(Inventory inventory, BaseItem baseItem, uint amount) : this()
        {
            this.baseItem = baseItem;
            this.inventory = inventory;
            Amount = amount;
        }



        public BaseItem GetBaseItem()
        {
            return baseItem;
        }

        public void SetBaseItem(BaseItem baseItem)
        {
            this.baseItem = baseItem;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public void SetInventory(Inventory inv)
        {
            inventory = inv;
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["ite_amount"] = Amount.ToString();
            data["ite_metadata"] = MetaData;
            data["inv_id"] = GetInventory().Id.ToString();
            data["itd_id"] = GetBaseItem().Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Amount = uint.Parse(data["ite_amount"]);
            MetaData = data["ite_metadata"];
            SetBaseItem(BaseItem.GetBaseItem(uint.Parse(data["itd_id"])));
        }
        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Item Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Item Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }



    }
}
