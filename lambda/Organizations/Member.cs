using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Organizations
{
    public class Member : IDBElement
    {
        public uint Id { get; set; }
        public uint PlayerId = 0;
        public Rank Rank;
        public string Name;

        public Member()
        {
        }

        public Member(uint id, Rank rank)
        {
            PlayerId = id;
            Rank = rank;
        }



        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["ran_id"] = Rank.Id.ToString();
            data["cha_id"] = PlayerId.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            PlayerId = uint.Parse(data["cha_id"]);
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Member Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Member Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task DeleteAsync()
        {
            await DatabaseElement.DeleteAsync(this);
        }
    }
}
