using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Enums;
using Lambda.Database;
using Lambda.Entity;

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

        public int Use(Player player)
        {
            uint baseId = GetBaseItem().Id;
            switch (baseId)
            {
                case 50:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Clothes.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 51:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Hat.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 52:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Glasses.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 53:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Ears.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 54:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Watch.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 55:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Bracelet.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 56:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Clothes.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 57:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Hat.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 58:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Glasses.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 59:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Ears.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 60:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Watch.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case 61:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Bracelet.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                default:
                    return 1;
                    break;
            }

            return 0;
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["ite_amount"] = Amount.ToString();
            data["ite_metadata"] = MetaData;
            data["inv_id"] = GetInventory().Id.ToString();
            data["itd_id"] = GetBaseItem().Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Amount = uint.Parse(data["ite_amount"]);
            MetaData = data["ite_metadata"];
            SetBaseItem(BaseItem.GetBaseItem(uint.Parse(data["itd_id"])));
        }
        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Item Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public async Task DeleteAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.DeleteAsync(this);
            Alt.Log("Item deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Item Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }



    }
}
