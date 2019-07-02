using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Database;

namespace Lambda.Administration
{
    public class Account : IDBElement
    {
        public short note = 0;
        public int admin = 0;
        public uint hoursPlayed = 0;

        public uint Id { get; set; }
        public string Mail = "";
        public string License = "";

        public DateTime BanTime = default;

        public Account(string license)
        {
            Id = 0;
            License = license;
        }

        public void Ban(int hours, string reason)
        {
            DateTime date = DateTime.Now;
            date = date.AddHours(hours);
            BanTime = date;
            _ = SaveAsync();

        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["acc_mail"] = Mail;
            data["acc_hoursplayed"] = hoursPlayed.ToString();
            data["acc_note"] = note.ToString();
            data["acc_admin"] = admin.ToString();
            data["acc_license"] = License;
            if (BanTime != default)
                data["acc_ban"] = BanTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Id = uint.Parse(data["acc_id"]);
            Mail = data["acc_mail"];
            hoursPlayed = uint.Parse(data["acc_hoursplayed"]);
            note = short.Parse(data["acc_note"]);
            admin = short.Parse(data["acc_admin"]);
            License = data["acc_license"];
            if (!string.IsNullOrEmpty(data["acc_ban"])) BanTime = Convert.ToDateTime(data["acc_ban"]);
        }



        public async void Save()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Account Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Account Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
