using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace Lambda.Entity
{
    public class Checkpoint : IEntity
    {
        public short Dimension { get; set; }
        public Position Position { get; set; }
        public Rotation Rotation { get; set; }
        public uint Range = 0;
        public Action<Player> Action = null;

        public Checkpoint(Position position, short dim, Action<Player> action, CheckpointType type = CheckpointType.Cyclinder, uint range = 1, short height = 1, Rgba color = default)
        {
            Position = position;
            Dimension = dim;
            Range = range;
            Action = action;
            Alt.CreateCheckpoint(type, position, range, height, new Rgba(0, 0, 0, 255));
            Checkpoints.Add(this);
        }

        public static List<Checkpoint> Checkpoints = new List<Checkpoint>();

    }
}
