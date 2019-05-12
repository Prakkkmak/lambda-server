using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;
using Lambda.Entity;

namespace Lambda.Skills
{
    public class Skill : IDBElement
    {
        public enum SkillType
        {
            SKILL1,
            SKILL2,
            SKILL3
        }

        public uint Id { get; set; }


        public string Name;
        public SkillType Type;

        public Player Player;

        private long xp;


        public Skill()
        {
            Name = "Nom non défini";
            xp = 0;
        }
        public Skill(Player player, SkillType type) : this()
        {
            Player = player;
            Type = type;
        }

        public void AddExperience(long xp)
        {
            this.xp += xp;
        }

        public void RemoveExperience(long xp)
        {
            this.xp -= xp;
        }

        public void SetExperience(long xp)
        {
            this.xp = xp;
        }

        public long GetExperience()
        {
            return xp;
        }

        public int GetLevel()
        {
            return (int)(this.xp / 5);
        }
        public void SetLevel(int level)
        {
            this.xp = level * xp;
        }
        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["skl_type"] = Type.ToString();
            data["skl_xp"] = GetExperience().ToString();
            data["cha_id"] = Player.Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            AddExperience(long.Parse(data["skl_xp"]));
            Type = Enum.Parse<Skill.SkillType>(data["skl_type"]);
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Skill Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Skill Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
