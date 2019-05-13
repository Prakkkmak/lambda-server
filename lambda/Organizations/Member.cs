using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Organizations
{
    public class Member
    {
        public uint Id;
        public string Name = "";
        public Rank Rank;

        public Member(uint id, Rank rank)
        {
            Id = id;
            Rank = rank;
        }


    }
}
