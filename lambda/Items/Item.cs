using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lambda.Items
{
    public class Item
    {
        private Inventory inventory;
        private BaseItem baseItem;


        public uint Id { get; set; }
        public uint Amount { get; set; }
        public string[] MetaData { get; set; }




        public Item(BaseItem baseItem, uint amount)
        {
            this.baseItem = baseItem;
            int sizeMetaDate = baseItem.MetaDataDescription.Length;
            MetaData = new string[sizeMetaDate];
            Amount = amount;
        }

        public BaseItem GetBaseItem()
        {
            return baseItem;
        }


    }
}
