using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;

namespace Lambda.Items
{
    public class Inventory
    {
        public enum InventoryType
        {
            DEFAULT,
            CHARACTER,
            VEHICLE,
            AREA
        }

        public uint Id { get; }
        public InventoryType Type { get; }
        public List<Item> Items { get; set; }

        public Inventory(InventoryType type = InventoryType.DEFAULT, uint id = 0)
        {
            Id = id;
            Type = type;
            Items = new List<Item>();
        }

        public bool AddItem(uint id, uint amount)
        {
            Item itemWithLessStack = GetItemWithLessStack(id);
            if (itemWithLessStack != null)
            {
                uint nbrToAdd = itemWithLessStack.Base.MaxStack - itemWithLessStack.Amount;
                amount -= nbrToAdd;
                itemWithLessStack.Amount += nbrToAdd;
            }
            Alt.Log(id + ") Création d'un item avec " + amount + " de quantité ");
            Item item = new Item(id, amount);
            while (item.Amount > item.Base.MaxStack)
            {
                item.Amount = item.Base.MaxStack;
                amount -= item.Base.MaxStack;
                Items.Add(item);
                item = new Item(id, amount);
            }
            Items.Add(item);
            return true;
        }

        public Item GetItemWithLessStack(uint id)
        {
            Item currentItem = null;
            foreach (Item item in Items)
            {
                if (item.Id == id)
                {

                    if (currentItem == null || item.Amount < currentItem.Amount)
                    {
                        currentItem = item;
                    }

                }
            }

            return currentItem;
        }
        public static Inventory Empty = new Inventory();
    }
}
