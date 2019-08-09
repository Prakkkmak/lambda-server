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
        private Inventory inventory = null;
        private BaseItem baseItem = null;


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
        public Item(BaseItem baseItem, uint amount) : this()
        {
            this.baseItem = baseItem;
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
            Enums.Items baseItem = (Enums.Items)GetBaseItem().Id;
            switch (baseItem)
            {
                case Enums.Items.ClothesMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Clothes.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.HatMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Hat.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.GlassesMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Glasses.Set(MetaData);
                    Alt.Log("Metadataz " + MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.EarsMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Ears.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.WatchMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Watch.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.BraceletMale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeFemale01) return 1;
                    player.Skin.Accessory.Bracelet.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.ClothesFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Clothes.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.HatFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Hat.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.GlassesFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Glasses.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.EarsFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Ears.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.WatchFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Watch.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.BraceletFemale:
                    if (player.Skin.Model == (uint)PedModel.FreemodeMale01) return 1;
                    player.Skin.Accessory.Bracelet.Set(MetaData);
                    player.Skin.Send(player, false);
                    _ = inventory.RemoveItemAsync(this);
                    break;
                case Enums.Items.DrivingLicense:
                    if (player.PlayerSelected == null) break;
                    player.PlayerSelected.SendMessage("Voici le permis de conduire de: " + MetaData);
                    break;
                case Enums.Items.BikeLicense:
                    if (player.PlayerSelected == null) break;
                    player.PlayerSelected.SendMessage("Voici le permis moto de: " + MetaData);
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
            DatabaseElement.Save(this);
        }

        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public async Task DeleteAsync()
        {
            await DatabaseElement.DeleteAsync(this);
        }

        public async Task SaveAsync()
        {
            await DatabaseElement.SaveAsync(this);
        }



    }
}
