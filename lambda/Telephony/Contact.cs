using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Telephony
{
    public struct Contact : IDBElement
    {
        public uint Id { get; set; }
        public Phone Phone { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }


        public Contact(uint id, Phone phone, string name, string phoneNumber)
        {
            Id = id;
            Phone = phone;
            Name = name;
            PhoneNumber = phoneNumber;
        }


        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["pco_number"] = PhoneNumber;
            data["pco_name"] = Name;
            data["pho_id"] = Phone.Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            PhoneNumber = data["pco_number"];
            Name = data["pco_name"];
            Phone = Phone.GetPhone(Convert.ToUInt32(data["pho_id"]));
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Contact Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Delete(this);
            Alt.Log("Contact Deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Contact Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
