using Items;
using Lambda.Organization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Organization
{
    class Organization
    {
        private uint id;
        private string name;
        private BankAccount bankAccount;
        private List<Rank> ranks; // ranks in this organization
    }
}
