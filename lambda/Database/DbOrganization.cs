using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Organizations;
using Lambda.Utils;

namespace Lambda.Database
{
    public class DbOrganization : DbElement<Organization>
    {
        public DbOrganization(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Organization organization)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["org_name"] = organization.Name;
            data["org_members"] = organization.GetMembersMetadata();
            return data;
        }

        public override void SetData(Organization organization, Dictionary<string, string> data)
        {
            organization.Name = data["org_name"];
            organization.SetMembers(data["org_members"]);
        }
    }
}
