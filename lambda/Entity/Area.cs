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
        }




        public Inventory Inventory { get; set; }
        private OwnerType ownerType;
        private uint ownerId;

        public uint Id { get; set; }
        public AreaType Type { get; set; }
        public ICheckpoint AltCheckpoint { get; set; }

        public float Radius { get; set; }
        public float Height { get; set; }
        public byte CheckpointTypeId { get; set; }

        public Position Position
        {
            get => AltCheckpoint.Position;
            set => AltCheckpoint.Position = value;
        }
        public Rotation Rotation { get; set; }
        public Game Game { get; }
        public short World { get; set; }

        public Area()
        {
            //
        }

        public Area(float radius, float height, AreaType type = AreaType.NORMAL)
        {
            Inventory = new Inventory();
            Radius = radius;
            Height = height;
            Type = type;


        }

        public void Spawn(int checkpointId, Position position, Rgba color)
        {
            AltCheckpoint = Alt.CreateCheckpoint((byte)checkpointId, position, Radius, 2, color);
        }

        public virtual string GetMetaData()
        {
            return "";
        }
        public virtual void SetMetaData(string metadata)
        {

        }




    }
}
