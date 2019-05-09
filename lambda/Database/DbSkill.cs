using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Skills;

namespace Lambda.Database
{
    public class DbSkill : DbElement<Skill>
    {
        public DbSkill(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Skill skill)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["skl_type"] = skill.Type.ToString();
            data["skl_xp"] = skill.GetExperience().ToString();
            data["cha_id"] = skill.Player.Id.ToString();
            return data;
        }

        public override void SetData(Skill skill, Dictionary<string, string> data)
        {
            skill.AddExperience(long.Parse(data["skl_xp"]));
            skill.Type = Enum.Parse<Skill.SkillType>(data["skl_type"]);
        }
        public Skill[] GetAll(Player player)
        {
            List<Skill> items = new List<Skill>();
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "cha_id";
            where[index] = player.Id.ToString();
            List<Dictionary<string, string>> results = DbConnect.Select(TableName, where);
            foreach (Dictionary<string, string> result in results)
            {
                Skill item = new Skill();
                SetData(item, result);
                item.Id = uint.Parse(result[Prefix + "_id"]);
                item.Player = player;
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}
