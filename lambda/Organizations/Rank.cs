using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Database;

namespace Lambda.Organizations
{
    class Rank : IDBElement
    {
        public uint Id { get; set; }
        public string Name;
        private Dictionary<uint, string> members; // The list of the members Id;

        public Rank()
        {
            Id = 0;
            Name = "";
            members = new Dictionary<uint, string>();
        }


    }
}
