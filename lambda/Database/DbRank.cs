using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Organizations;

namespace Lambda.Database
{
    public class DbRank : DbElement<Rank>
    {
        public DbRank(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Rank rank)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data["ran_id"] = vehicle.Model.ToString();
            data["ran_name"] = rank.Name;
            data["ran_salary"] = rank.Salary.ToString();
            data["ran_default"] = (rank.IsDefault() ? 1 : 0).ToString();
            data["org_id"] = rank.Organization.Id.ToString();
            return data;
        }

        public override void SetData(Rank rank, Dictionary<string, string> data)
        {
            rank.Name = data["ran_name"];
            rank.Salary = uint.Parse(data["ran_salary"]);
        }

        public Rank[] GetAll(Organization organization)
        {
            List<Rank> ranks = new List<Rank>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            where["org_id"] = organization.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(TableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Rank rank = new Rank();
                SetData(rank, result);
                rank.Id = uint.Parse(result[Prefix + "_id"]);
                ranks.Add(rank);
                //rank.Organization = organization;
                if (result[Prefix + "_default"] == "1")
                {
                    organization.DefaultRank = rank;
                }
            }

            return ranks.ToArray();
        }
    }
}
