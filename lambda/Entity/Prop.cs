using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;

namespace Lambda.Entity
{
    public class Prop : IEntity
    {
        public short Dimension { get; set; }
        public Position Position { get; set; }
        public Rotation Rotation { get; set; }

        public string Name = "";

        public uint Serial = 0;
        //public int MoveFrequency = 1000; // None
        public int SyncFrequency = 0; // None

        public Prop(Position pos, Rotation rot, short dim, string name)
        {
            Position = pos;
            Rotation = rot;
            Dimension = dim;
            Name = name;
            GenerateSerial();
            Props.Add(this);
            foreach (Player player in Player.Players)
            {
                player.Emit("syncProp", GetIdentity(), Name, Position, Rotation);
            }
        }
        public void GenerateSerial()
        {
            uint maxSerial = 0;
            foreach (Prop prop in Props)
            {
                if (prop.Name.Equals(Name))
                {
                    if (prop.Serial > maxSerial) maxSerial = prop.Serial;
                }
            }
            Serial = maxSerial + 1;
        }
        public string GetIdentity()
        {
            return Name + ":" + Serial;
        }
        public virtual void Update()
        {
            //
        }

        public static void SyncProps()
        {
            PropSyncTic++;
            if (PropSyncTic > long.MaxValue - 1) PropSyncTic = 0;
            foreach (Prop prop in Props)
            {
                if ((prop.SyncFrequency > 0 && PropSyncTic % prop.SyncFrequency == 0) || prop.SyncFrequency == 0) 
                {
                    prop.Update();
                    if (prop.SyncFrequency == 0) prop.SyncFrequency = -1;
                    foreach (Player player in Player.Players)
                    {
                        player.Emit("syncProp", prop.GetIdentity(), prop.Name, prop.Position, prop.Rotation);
                    }
                }
            }
        }

        public static long PropSyncTic = 0;

        public static List<Prop> Props = new List<Prop>();
    }
}
