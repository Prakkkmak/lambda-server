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
            int size = GetBaseItem().MetaDataDescription.Length;
            MetaData = new string[size];
        }

        public BaseItem GetBaseItem()
        {
            return baseItem;
        }


    }
}
