using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Items;

namespace Lambda.Database
{
    public class DbItem : DbElement<Item>
    {
        public DbItem(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Item item)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["ite_amount"] = item.Amount.ToString();
            data["ite_metadata"] = item.MetaData;
            data["inv_id"] = item.GetInventory().Id.ToString();
            data["itd_id"] = item.GetBaseItem().Id.ToString();
            return data;
        }

        public override void SetData(Item item, Dictionary<string, string> data)
        {
            item.Amount = uint.Parse(data["ite_amount"]);
            item.MetaData = data["ite_metadata"];
            item.SetBaseItem(Game.GetBaseItem(uint.Parse(data["itd_id"])));

        }
        public Item[] GetAll(Inventory inventory)
        {
            List<Item> items = new List<Item>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "inv_id";
            where[index] = inventory.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(TableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Item item = new Item();
                SetData(item, result);
                item.Id = uint.Parse(result[Prefix + "_id"]);
                items.Add(item);
                item.SetInventory(inventory);
            }

            return items.ToArray();
        }
    }
}
