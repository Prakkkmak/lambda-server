using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using AltV.Net;
using Lambda.Entity;

namespace Lambda.Items
{
    public class Inventory
    {
        public IEntity Entity;

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
        public long Money { get; private set; }


        public Inventory(IEntity entity, InventoryType type = InventoryType.DEFAULT, uint id = 0)
        {
            Id = id;
            Type = type;
            Items = new List<Item>();
            Money = 10000;
            Entity = entity;
        }

        public bool Withdraw(long amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            return true;
        }

        public bool Deposit(long amount)
        {
            Money += amount;
            return true;
        }


        public bool AddItem(uint id, uint amount)
        {
            Item itemWithLessStack = GetItemWithLessStack(id);
            if (itemWithLessStack != null)
            {
                uint nbrToAdd = itemWithLessStack.GetBaseItem().MaxStack - itemWithLessStack.Amount;
                amount -= nbrToAdd;
                itemWithLessStack.Amount += nbrToAdd;
            }
            Alt.Log(id + ") Création d'un item avec " + amount + " de quantité ");
            Item item = new Item(Entity.Game.GetBaseItem(id), amount);
            //if (item.GetBaseItem().MaxStack < 1) item.Amount = amount;
            while (item.Amount > item.GetBaseItem().MaxStack && item.GetBaseItem().MaxStack > 0)
            {
                item.Amount = item.GetBaseItem().MaxStack;
                amount -= item.GetBaseItem().MaxStack;
                Items.Add(item);
                item = new Item(Entity.Game.GetBaseItem(id), amount);
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
    }
}
