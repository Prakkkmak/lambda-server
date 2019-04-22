using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Utils;

namespace Lambda.Entity
{
    public interface IEntity
    {
        Game Game { get; }
        short Dimension { get; set; }
        Position Position { get; set; }
        Rotation Rotation { get; set; }

    }
}
