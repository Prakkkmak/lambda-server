using System;
using System.Collections.Generic;
using System.Text;
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

    }
}
