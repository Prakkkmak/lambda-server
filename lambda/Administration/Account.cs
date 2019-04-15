using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lambda.Database;

namespace Lambda.Administration
{
    public class Account : IDBElement
    {
        public short note;
        public int admin;
        public uint hoursPlayed;

        public uint Id { get; set; }
        public string Mail { get; set; }
        public string License { get; set; }

        public Account()
        {
            Mail = "";
            Id = 0;
            note = 0;
            admin = 0;
        }

        public Account(string license) : this()
        {
            License = license;

        }

    }
}
