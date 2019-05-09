using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lambda.Items;

namespace Lambda.Database
{
    public class DbInventory : DbElement<Inventory>
    {
        public DbInventory(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Inventory inventory)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["inv_type"] = inventory.Type.ToString();
            data["inv_money"] = inventory.Money.ToString();
            return data;
        }

        public override void SetData(Inventory inventory, Dictionary<string, string> data)
        {
            inventory.Id = uint.Parse(data["inv_id"]);
            inventory.Money = long.Parse(data["inv_money"]);
            inventory.Items = Game.DbItem.GetAll(inventory).ToList();
        }
        public override void Save(Inventory inventory)
        {
            foreach (Item inventoryItem in inventory.Items)
            {
                Game.DbItem.Save(inventoryItem);
            }
            base.Save(inventory);
        }
    }
}
