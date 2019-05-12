using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public BaseItem()
        {

        }

        public Dictionary<string, string> GetData()
        {
            return null;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Id = uint.Parse(data["itd_id"]);
            //TODO améliorer le TryParse
            Enum.TryParse(data["itd_type"], out BaseItem.ItemType result);
            Type = result;
            Name = data["itd_name"];
            Description = data["itd_description"];
            Weight = int.Parse(data["itd_weight"]);
            MaxStack = uint.Parse(data["itd_maxstack"]);
        }

        public void Delete()
        {
            return;
        }

        public Task DeleteAsync()
        {
            return Task.Delay(0);
        }

        public Task SaveAsync()
        {
            return Task.Delay(0);
        }

        public void Save()
        {
            return;
        }
        public static void LoadBaseItems()
        {
            BaseItems = DatabaseElement.GetAllBaseItems().ToList();
        }
        public static BaseItem GetBaseItem(uint id)
        {
            return BaseItems.FirstOrDefault(baseitem => baseitem.Id == id);
        }
        public static List<BaseItem> BaseItems = new List<BaseItem>();
    }
}
