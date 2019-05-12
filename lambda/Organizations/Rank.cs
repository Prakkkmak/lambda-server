using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using Lambda.Administration;
using Lambda.Database;

namespace Lambda.Organizations
{
    public class Rank : IDBElement
    {
        public uint Id { get; set; }

        public string Name;
        public uint Salary;
        public Organization Organization;
        public Permissions Permissions = new Permissions();

        public Rank()
        {
            Id = 0;
            Name = "Defaut";
        }

        public Rank(Organization org) : this()
        {
            Organization = org;
        }

        public Rank(Organization org, string name) : this(org)
        {
            Name = name;
        }

        public bool IsDefault()
        {
            return Organization.DefaultRank == this;
        }


        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data["ran_id"] = vehicle.Model.ToString();
            data["ran_name"] = Name;
            data["ran_salary"] = Salary.ToString();
            data["ran_default"] = (IsDefault() ? 1 : 0).ToString();
            data["org_id"] = Organization.Id.ToString();
            data["ran_permissions"] = Permissions.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Name = data["ran_name"];
            Salary = uint.Parse(data["ran_salary"]);
            Permissions.Set(data["ran_permissions"]);
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Rank Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Rank Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
