using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Items
{
    public class BaseItem : IDBElement
    {
        public enum ItemType
        {
            REGULAR,
        }
        public uint Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public uint MaxStack { get; set; }
        public string[] MetaDataDescription { get; set; }

        public BaseItem()
        {
            //
        }


    }
}
