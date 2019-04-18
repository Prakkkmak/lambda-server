using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Items;

namespace Lambda.Database
{
    public class DbBaseItem : DbElement<BaseItem>
    {
        public DbBaseItem(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(BaseItem entity)
        {
            return new Dictionary<string, string>();
        }

        public override void SetData(BaseItem baseItem, Dictionary<string, string> data)
        {
            baseItem.Id = uint.Parse(data["itd_id"]);
            //TODO améliorer le TryParse
            Enum.TryParse(data["itd_type"], out BaseItem.ItemType result);
            baseItem.Type = result;
            baseItem.Name = data["itd_name"];
            baseItem.Description = data["itd_description"];
            baseItem.Weight = int.Parse(data["itd_weight"]);
            baseItem.MaxStack = uint.Parse(data["itd_maxstack"]);
            baseItem.MetaDataDescription = new string[0]; //TODO
        }
    }
}
