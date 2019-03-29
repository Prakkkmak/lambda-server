using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Organization
{
    class Payday
    {
        private uint id;
        private DateTime date;
        private int amount;
        private uint characterId;
        public Payday()
        {
            this.id = 0;
            this.date = new DateTime();
            this.amount = 0;
            this.characterId = 0;
        }
    }
}
