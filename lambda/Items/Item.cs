using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lambda.Items
{
    public class Item
    {
        public uint Id { get; set; }
        public uint Amount { get; set; }
        public string[] MetaData { get; set; }

        public BaseItem Base
        {
            get {
                return BaseItem.BaseItems.FirstOrDefault(baseItem => baseItem.Id == Id);
            }
        }

        public Item(uint id, uint amount)
        {
            Id = id;
            Amount = amount;
            int size = Base.MetaDataDescription.Length;
            MetaData = new string[size];
        }


    }
}
