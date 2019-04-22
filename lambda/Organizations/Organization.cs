using Items;
using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Database;

namespace Lambda.Organizations
{
    public class Organization : IDBElement
    {
        public uint Id { get; set; }
        public string Name;
        private List<Rank> ranks; // ranks in this organization

        public Organization()
        {
            Id = 0;
            Name = "";
            ranks = new List<Rank>();
        }

        public Organization(string name) : this()
        {
            this.Name = name;

        }


    }
}
