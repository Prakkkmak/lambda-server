using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Database;

namespace Lambda.Organizations
{
    public class Rank : IDBElement
    {
        public uint Id { get; set; }
        public string Name;
        public uint Salary;
        public Organization Organization;

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


    }
}
