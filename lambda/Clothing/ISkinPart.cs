using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    interface ISkinPart
    {
        void Set(string str);
        void Send(Player player);
        string ToString();
    }
}
