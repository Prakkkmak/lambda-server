using Lambda.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Database
{
    public class DbLink : DbElement<Link>
    {
        public DbLink(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }
        public override Dictionary<string, string> GetData(Link link)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["lin_valid"] = link.Type.ToString();
            data["lin_from"] = link.Component1.Item1.ToString();
            data["lin_to"] = link.Component2.Item1.ToString();
            data["lin_fromdrawable"] = link.Component1.Item2.ToString();
            data["lin_todrawable"] = link.Component2.Item2.ToString();
            return data;
        }

        public override void SetData(Link link, Dictionary<string, string> data)
        {
            //componentLink.Id = uint.Parse(data["lin_id"]);
            link.Component1 = new Tuple<byte, ushort>(byte.Parse(data["lin_from"]), ushort.Parse(data["lin_fromdrawable"]));
            link.Component2 = new Tuple<byte, ushort>(byte.Parse(data["lin_to"]), ushort.Parse(data["lin_todrawable"]));
            link.Type = (Link.LinkType)int.Parse(data["lin_valid"]);
        }
    }
}
