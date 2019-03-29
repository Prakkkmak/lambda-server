using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Items
{
    public class BaseItem
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

        public BaseItem(Dictionary<string, string> datas)
        {
            Id = uint.Parse(datas["itd_id"]);
            Enum.TryParse(datas["itd_type"], out ItemType result);
            Type = result;
            Name = datas["itd_name"];
            Description = datas["itd_description"];
            Weight = int.Parse(datas["itd_weight"]);
            MaxStack = uint.Parse(datas["itd_maxstack"]);
            MetaDataDescription = new string[0];
            /*string metastadatastr = datas["itd_metadata"];
            MetaDataDescription = metastadatastr.Split(";");*/
        }

        public void AddToBaseItems()
        {
            Alt.Log($"Add object [{Id}]{Name} to base items");
            List<BaseItem> list = new List<BaseItem>();
            list.AddRange(BaseItems);
            list.Add(this);
            BaseItems = list.ToArray();
        }

        public static BaseItem GetBaseItem(uint id)
        {
            return BaseItems.FirstOrDefault(baseItem => baseItem.Id == id);
        }

        public static void LoadAllItems()
        {
            DBConnect dbConnect = DBConnect.DbConnect;
            List<Dictionary<string, string>> results = dbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                BaseItem item = new BaseItem(result);
                item.AddToBaseItems();
            }
        }



        public static string TableName = "t_itemdata_itd";
        public static BaseItem[] BaseItems = new BaseItem[0];
    }
}
