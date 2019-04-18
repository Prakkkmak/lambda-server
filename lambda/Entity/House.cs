using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Entity
{
    class House : Area
    {
        private int rent;

        public Interior interior { get; set; }
        public uint interiorDim { get; set; }
        public House() : base(2, 1, AreaType.SHOP)
        {
            CheckpointTypeId = 10;
        }

    }
}
