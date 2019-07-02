using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Telephony
{
    public struct Message : IDBElement
    {
        public uint Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }

        public Phone Phone { get; set; }

        public Message(uint id, Phone phone, string from, string to, string body)
        {
            Id = id;
            Phone = phone;
            From = from;
            To = to;
            Body = body;
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["pme_from"] = From;
            data["pme_to"] = To;
            data["pme_body"] = Body;
            data["pho_id"] = Phone.Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            From = data["pme_from"];
            To = data["pme_to"];
            Body = data["pme_body"];
            Phone = Phone.GetPhone(Convert.ToUInt32(data["pho_id"]));
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Message Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Delete(this);
            Alt.Log("Message Deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Message Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
