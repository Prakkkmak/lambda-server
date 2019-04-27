using Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Lambda.Database;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Entity
{
    public class Area : IEntity, IDBElement
    {
        public enum OwnerType
        {
            CHARACTER,
            ORGANIZATION
        }
        public enum AreaType
        {
            NORMAL,
            SHOP,
            HOUSE,
            BANK
        }




        public Inventory Inventory { get; set; }
        private OwnerType ownerType;
        private uint ownerId;

        //public Interior Interior { get; set; }

        public uint Id { get; set; }
        public AreaType Type { get; set; }
        public ICheckpoint AltCheckpoint { get; set; }

        public float Radius { get; set; }
        public float Height { get; set; }
        public byte CheckpointTypeId { get; set; }

        public Location InteriorLocation;
        public Location ExteriorLocation;

        public Position SpawnPosition;

        public Position Position
        {
            get => AltCheckpoint?.Position ?? SpawnPosition;
            set {
                if (AltCheckpoint != null) AltCheckpoint.Position = value;
            }
        }
        public Rotation Rotation { get; set; }
        public Game Game { get; set; }
        public short Dimension { get; set; }

        public Area()
        {
            //Spawn(0, new Position(0, 0, 0), new Rgba(0, 0, 0, 255));
        }

        public Area(float radius, float height, AreaType type = AreaType.NORMAL) : this()
        {
            Inventory = new Inventory(this);
            Radius = radius;
            Height = height;
            Type = type;


        }

        public void Spawn(Position position)
        {
            AltCheckpoint = Alt.CreateCheckpoint((byte)this.CheckpointTypeId, position, Radius, 2, new Rgba(0, 0, 0, 255));
        }

        public virtual string GetMetaData()
        {
            return "";
        }
        public virtual void SetMetaData(string metadata)
        {

        }

        public void SetLocations(Interior interior, short dim)
        {
            Location interiorLocation = new Location(interior.Position, interior, dim);
            Location exteriorLocation = new Location(Position, null, 0);
            SetInteriorLocation(interiorLocation);
            SetExteriorLocation(exteriorLocation);
        }

        public void SetInteriorLocation(Location interior)
        {
            InteriorLocation = interior;


        }
        public void SetExteriorLocation(Location interior)
        {
            ExteriorLocation = interior;

        }



    }
}
