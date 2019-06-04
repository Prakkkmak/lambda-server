using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;
using Lambda.Entity;

namespace Lambda.Items
{
    public class Inventory : IDBElement

    {
        public IEntity Entity;

        public enum InventoryType
        {
            DEFAULT,
            CHARACTER,
            VEHICLE,
            AREA
        }

        public uint Id { get; set; }


        public InventoryType Type { get; }
        public List<Item> Items = new List<Item>();
        public long Money = 1000;

        public Inventory(IEntity entity, InventoryType type = InventoryType.DEFAULT, uint id = 0)
        {
            Id = id;
            Type = type;
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

        public bool AddItem(Enums.Items id, uint amount, string metadata = "")
        {
            if (id == Enums.Items.Invalid) return false;
            return AddItem((uint)id, amount, metadata);
        }
        public bool AddItemOld(uint id, uint amount, string metadata = "")
        {
            Item itemWithLessStack = GetItemWithLessStack(id);
            if (itemWithLessStack != null)
            {
                uint nbrToAdd = itemWithLessStack.GetBaseItem().MaxStack - itemWithLessStack.Amount;
                amount -= nbrToAdd;
                itemWithLessStack.Amount += nbrToAdd;
            }

            Item item = new Item(this, BaseItem.GetBaseItem(id), amount);
            if (item.GetBaseItem().MaxStack < 1) item.Amount = amount;
            while (item.Amount > item.GetBaseItem().MaxStack && item.GetBaseItem().MaxStack > 0)
            {
                item.Amount = item.GetBaseItem().MaxStack;
                amount -= item.GetBaseItem().MaxStack;
                Items.Add(item);
                item = new Item(this, BaseItem.GetBaseItem(id), amount);
            }

            item.MetaData = metadata;
            Items.Add(item);
            return true;
        }
        public bool AddItem(uint id, uint amount, string metadata = "")
        {
            Item itemWithLessStack = GetItemWithLessStack(id);
            if (itemWithLessStack != null)
            {
                uint nbrToAdd = itemWithLessStack.GetBaseItem().MaxStack - itemWithLessStack.Amount;
                if (amount <= nbrToAdd) itemWithLessStack.Amount += amount;
                amount -= nbrToAdd;
                itemWithLessStack.Amount += nbrToAdd;
            }

            Item item = new Item(this, BaseItem.GetBaseItem(id), amount);
            if (item.GetBaseItem().MaxStack < 1) item.Amount = amount;
            while (item.Amount > item.GetBaseItem().MaxStack && item.GetBaseItem().MaxStack > 0)
            {
                item.Amount = item.GetBaseItem().MaxStack;
                amount -= item.GetBaseItem().MaxStack;
                Items.Add(item);
                item = new Item(this, BaseItem.GetBaseItem(id), amount);
            }

            item.MetaData = metadata;
            Items.Add(item);
            return true;
        }

        public Item GetItem(uint id)
        {
            Item item = null;
            foreach (Item it in Items)
            {
                if (it.GetBaseItem().Id == id)
                {
                    if (item == null) item = new Item(this, it.GetBaseItem(), it.Amount);
                    else item.Amount += it.Amount;
                }
            }

            return item;
        }

        public void RemoveItem(uint id, uint amount)
        {
            int slot = (int)id;
            if (amount < Items[slot].Amount && amount != 0)
            {
                Items[slot].Amount -= amount;
            }
            else
            {
                Items.RemoveAt(slot);
            }
            _ = SaveAsync();

        }

        public void TransferItem(Item item, Inventory inventory)
        {
            this.Items.Remove(item);
            inventory.Items.Add(item);
            item.SetInventory(inventory);
        }

        public Item GetItemWithLessStack(uint id)
        {
            Item currentItem = null;
            foreach (Item item in Items)
            {
                if (item.GetBaseItem().Id == id)
                {

                    if (currentItem == null || item.Amount < currentItem.Amount)
                    {
                        currentItem = item;
                    }

                }
            }
            return currentItem;
        }

        public Item GetItemWithBaseItemIdAndMetaData(uint id, string metadata)
        {
            foreach (Item item in Items)
            {
                if (item.GetBaseItem().Id == id && item.MetaData == metadata) return item;
            }

            return null;
        }


        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["inv_type"] = Type.ToString();
            data["inv_money"] = Money.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Id = uint.Parse(data["inv_id"]);
            Money = long.Parse(data["inv_money"]);
            Items = DatabaseElement.GetAllItems(this).ToList();
        }
        public void Clear()
        {
            List<Item> items = Items;
            Items = new List<Item>();
            foreach (Item item in items)
            {
                item.Delete();
            }
            Save();
        }
        public async Task ClearAsync()
        {
            List<Item> items = Items;
            Items = new List<Item>();
            foreach (Item item in items)
            {
                await item.DeleteAsync();
            }

            await SaveAsync();
        }


        public void Save()
        {
            long t = DateTime.Now.Ticks;
            foreach (Item item in Items)
            {
                item.Save();
            }
            DatabaseElement.Save(this);
            Alt.Log("Inventory Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }
        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            foreach (Item item in Items)
            {
                await item.SaveAsync();
            }
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Inventory Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }





    }
}
