using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace Lambda.Entity
{
    public class Blip : IEntity
    {
        public short Dimension { get; set; } = 0;

        public Position Position { get; set; } = Position.Zero;

        public Rotation Rotation { get; set; } = Rotation.Zero;

        public Position GroundPosition
        {
            get {
                return new Position(Position.X, Position.Y, Position.Z - 1);
            }
            set {
                value = new Position(Position.X, Position.Y, Position.Z + 1);
            }
        }

        public Rgba Color = Rgba.Zero;

        public BlipType Type = BlipType.Area;

        public IBlip AltBlip;

        public Blip(Position position, short dim, BlipType type = BlipType.Object, float range = 0.5f, float height = 1, Rgba color = default)
        {
            Dimension = dim;
            Position = position;
            Color = color;
            DrawBlip();
            Blips.Add(this);

        }

        public void DrawBlip()
        {
            if (AltBlip != null) AltBlip.Remove();
            AltBlip = Alt.CreateBlip(Type, Position);
        }

        public static List<Blip> Blips = new List<Blip>();
    }
}
