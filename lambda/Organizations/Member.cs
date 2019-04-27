using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Organizations
{
    public class Member
    {
        public uint Id;
        public Rank Rank;

        public Member()
        {

        }

        public Member(uint id, Rank rank)
        {
            Id = id;
            Rank = rank;
        }


    }
}
