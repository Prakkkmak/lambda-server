using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Administration;
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

        public Permissions Permissions = new Permissions();

        public Organization()
        {
            Id = 0;
            Name = "";
            ranks = new List<Rank>();
            members = new List<Member>();
            Rank r = new Rank(this);
            Permissions.Add("LEADER");
            DefaultRank = r;
            ranks.Add(r);
        }

        public Organization(string name) : this()
        {
            this.Name = name;

        }

        public Member AddMember(Player player)
        {
            if (members.Any(member => member.Id == player.Id)) return null;
            Member m = new Member(player.Id, DefaultRank);
            members.Add(m);
            return m;
        }
        public Member AddMember(Player player, Rank rank)
        {
            if (members.Any(member => member.Id == player.Id)) return null;
            Member m = new Member(player.Id, rank);
            members.Add(m);
            return m;
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
            List<Rank> rs = new List<Rank>();
            foreach (Rank rank in this.ranks)
            {
                if (ranks.IndexOf(rank).ToString().Equals(name) || rank.Name.Replace(" ", "_").Equals(name)) rs.Add(rank);
            }
            return rs.ToArray();

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

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["org_name"] = Name;
            data["org_members"] = GetMembersMetadata();
            data["org_permissions"] = Permissions.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Name = data["org_name"];
            SetMembers(data["org_members"]);
            Permissions.Set(data["org_permissions"]);
        }


        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            foreach (Rank rank in GetRanks())
            {
                DatabaseElement.Save(rank);
            }
            Alt.Log("Organization Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {

            DatabaseElement.Delete(this);
        }

        public void Remove()
        {
            Organizations.Remove(this);
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            foreach (Rank rank in GetRanks())
            {
                await rank.SaveAsync();
            }
            Alt.Log("Organization Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");

        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }


        public static void LoadOrganizations()
        {
            Organizations = DatabaseElement.GetAllOrganizations().ToList();
            foreach (Organization organization in Organizations)
            {
                Rank[] ranks = DatabaseElement.GetAllRanks(organization);
                foreach (Rank rank in ranks)
                {
                    organization.AddRank(rank);
                }
            }
        }
        public static void AddOrganization(Organization org)
        {
            Organizations.Add(org);
        }
        public static Organization[] GetOrganizations(string name)
        {
            List<Organization> orgs = new List<Organization>();
            foreach (Organization organization in Organizations)
            {
                if (organization.Id.ToString().Equals(name) || organization.Name.Replace(" ", "_").Equals(name)) orgs.Add(organization);
            }
            return orgs.ToArray();
        }

        public static List<Organization> Organizations = new List<Organization>();
    }
}
