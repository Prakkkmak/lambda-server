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

        public string Name = "";

        public Permissions Permissions = new Permissions();

        private List<Rank> ranks = new List<Rank>();
        private List<Member> members = new List<Member>();



        public Organization(string name)
        {
            this.Name = name;
        }

        public void Rename(string name)
        {
            this.Name = name;
            _ = this.SaveAsync();
        }

        public Member AddMember(Player player, Rank rank)
        {
            if (members.Any(member => member.Id == player.Id)) return null;
            Member m = new Member(player.Id, rank);
            m.Name = player.Name;
            members.Add(m);
            return m;
        }
        public Member AddMember(uint id, uint rankid)
        {
            if (members.Any(member => member.Id == id)) return null;
            Rank rank = GetRank(rankid);
            if (rank == null) return null;
            Member m = new Member(id, rank);
            members.Add(m);
            return m;
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
            _ = r.SaveAsync();
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
            _ = rank.DeleteAsync();
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
                Member member = AddMember(uint.Parse(result[0]), uint.Parse(result[1]));
                if (member != null) member.Name = DatabaseElement.Get<Player>(member.Id)["cha_firstname"] + " " +
                                DatabaseElement.Get<Player>(member.Id)["cha_lastname"];
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

        public async Task DeleteAsync()
        {
            foreach (Rank rank in GetRanks())
            {
                await rank.DeleteAsync();
            }
            _ = DatabaseElement.DeleteAsync(this);
        }

        public static Organization CreateOrganization(string name)
        {
            Organization o = new Organization(name);
            Organizations.Add(o);
            _ = o.SaveAsync();
            return o;
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
