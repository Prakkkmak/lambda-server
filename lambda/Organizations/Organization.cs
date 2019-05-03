using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lambda.Database;
using Lambda.Entity;

namespace Lambda.Organizations
{
    public class Organization : IDBElement
    {
        public uint Id { get; set; }
        public string Name;
        public Rank DefaultRank { get; set; }
        private List<Rank> ranks; // ranks in this organization

        private List<Member> members;

        public Organization()
        {
            Id = 0;
            Name = "";
            ranks = new List<Rank>();
            members = new List<Member>();
            Rank r = new Rank(this);
            DefaultRank = r;
            ranks.Add(r);
        }

        public Organization(string name) : this()
        {
            this.Name = name;

        }

        public void AddMember(Player player)
        {
            if (members.Any(member => member.Id == player.Id)) return;
            members.Add(new Member(player.Id, DefaultRank));
        }
        public void AddMember(uint id, uint rankid)
        {
            if (members.Any(member => member.Id == id)) return;
            Rank rank = GetRank(rankid);
            if (rank == null) return;
            members.Add(new Member(id, rank));
        }

        public Member GetMember(uint id)
        {
            foreach (Member member in members)
            {
                if (member.Id == id) return member;
            }

            return null;
        }
        public Member[] GetMembers()
        {
            return members.ToArray();
        }

        public void AddRank(Rank rank)
        {
            ranks.Add(rank);
            rank.Organization = this;
        }
        public Rank AddRank(string name)
        {
            Rank r = new Rank(this, name);
            ranks.Add(r);
            return r;
        }

        public Rank GetRank(uint id)
        {
            foreach (Rank rank in ranks)
            {
                if (rank.Id == id) return rank;
            }

            return null;
        }
        public Rank GetRankByIndex(int id)
        {
            return ranks.Count < id ? null : ranks[id];
        }
        public Rank[] GetRanks(string name)
        {
            List<Rank> ranks = new List<Rank>();
            foreach (Rank rank in ranks)
            {
                if (ranks.IndexOf(rank).ToString().Equals(name) || rank.Name.Replace(" ", "_").Equals(name)) ranks.Add(rank);
            }
            return ranks.ToArray();

        }
        public void RemoveRank(Rank rank)
        {
            ranks.Remove(rank);
        }

        public Rank[] GetRanks()
        {
            return ranks.ToArray();
        }

        public void SetMembers(string metadata)
        {
            string[] datas = metadata.Split(",");
            foreach (string data in datas)
            {
                string[] result = data.Split(":");
                if (result.Length < 2) return;
                AddMember(uint.Parse(result[0]), uint.Parse(result[1]));
            }
        }
        public string GetMembersMetadata()
        {
            string str = "";
            foreach (Member member in members)
            {
                str += $"{member.Id}:{member.Rank.Id},";
            }

            return str;
        }
    }
}
