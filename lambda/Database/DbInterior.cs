using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Database
{
    public class DbInterior : DbElement<Interior>
    {
        public DbInterior(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Interior entity)
        {
            return new Dictionary<string, string>();
        }

        public override void SetData(Interior interior, Dictionary<string, string> data)
        {
            interior.Id = uint.Parse(data["int_id"]);
            interior.IPL = data["int_ipl"];
            interior.Position = GetPosition(data);
        }
    }
}
