using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Database
{
    public interface IDBElement
    {
        uint Id { get; set; }
        //void Load(Dictionary<string, string> data);

    }
}
