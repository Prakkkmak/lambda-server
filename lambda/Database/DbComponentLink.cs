using Lambda.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Database
{
    public class DbComponentLink : DbElement<ComponentLink>
    {
        public DbComponentLink(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }
        public override Dictionary<string, string> GetData(ComponentLink componentLink)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            int i = 0;
            data["lin_valid"] = ((uint)componentLink.Validity).ToString();
            data["lin_from"] = componentLink.Link.From.ToString();
            data["lin_to"] = componentLink.Link.To.ToString();
            data["lin_fromdrawable"] = componentLink.DrawableA.ToString();
            data["lin_todrawable"] = componentLink.DrawableB.ToString();
            return data;
        }

        public override void SetData(ComponentLink componentLink, Dictionary<string, string> data)
        {
            componentLink.Id = uint.Parse(data["lin_id"]);
            componentLink.Link = new Link(uint.Parse(data["lin_from"]), uint.Parse(data["lin_to"]));
            componentLink.DrawableA = uint.Parse(data["lin_fromdrawable"]);
            componentLink.DrawableB = uint.Parse(data["lin_todrawable"]);
            componentLink.Validity = (ComponentLink.Valid)int.Parse(data["lin_valid"]);
        }
    }
}
