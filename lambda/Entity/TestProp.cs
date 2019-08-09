using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda.Entity
{
    class TestProp : Prop
    {
        public TestProp(Position pos, Rotation rot, short dim, string name) : base(pos, rot, dim, name)
        {
            SyncFrequency = 1;
        }

        public override void Update()
        {
            base.Update();
            Position newPos = this.Position;
            newPos.X = this.Position.X + 0.1f;
            this.Position = newPos;
        }
    }
}
