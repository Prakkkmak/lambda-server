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
        public long Money = 0;

        public Player Player
        {
            get {
                foreach(Player player in Player.Players)
                {
                    if (player.Inventory == this) return player;
                }
                return null;
            }
        }

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
            if (Player != null) Player.SendMessage("Vous avez perdu " + amount + "$.");
            return true;
        }

        public bool AddMoney(long amount)
        {
            Money += amount;
            if (Player != null) Player.SendMessage("Vous avez reçu " + amount + "$.");
            return true;
        }
        public void AddItem(Enums.Items id, uint amount, string metadata = "")
        {
            AddItem((uint)id, amount, metadata);
        }
        public void AddItem(uint id, uint amount, string metadata = "")
        {
            Item item = new Item(BaseItem.GetBaseItem(id), amount);
            item.MetaData = metadata;
            AddItem(item);
        }

        public void AddItem(Item item)
        {
            if (Player != null) Player.SendMessage(item.Amount + " " + item.GetBaseItem().Name + " ont étés ajoutés à votre inventaire.");
            item.SetInventory(this);
            if(item.GetBaseItem().MaxStack == 1)
            {
                Items.Add(item);
            }
            else
            {
                foreach(Item it in Items)
                {
                    if(it.GetBaseItem().Id == item.GetBaseItem().Id)
                    {

                        it.Amount += item.Amount;
                        return;
                    }
                }
                Items.Add(item);
            }
        }
        public Item GetItem(Enums.Items item)
        {
            return GetItem((uint)item);
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
        public void RemoveItem(Enums.Items id, uint amount)
        {
            RemoveItem((uint)id, amount);

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

        public async Task RemoveItemAsync(uint id)
        {
            Item item = Items[(int)id];
            Items.Remove(item);
            await item.DeleteAsync();
            await SaveAsync();
        }
        public void RemoveItem(Item item)
        {
            Items.Remove(item);
            item.Delete();
            Save();

        }
        public async Task RemoveItemAsync(Item item)
        {
            Items.Remove(item);
            await item.DeleteAsync();
            await SaveAsync();
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

        public async Task DeleteAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.DeleteAsync(this);
            Alt.Log("Item deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }





    }
}
