using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    }
}
