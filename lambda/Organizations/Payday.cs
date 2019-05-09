using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Organizations
{
    class Payday
    {
        public Dictionary<string, long> sources;

        public Payday()
        {
            sources = new Dictionary<string, long>();
        }

        public void AddSource(string label, long amount)
        {
            sources.Add(label, amount);
        }


    }
}
