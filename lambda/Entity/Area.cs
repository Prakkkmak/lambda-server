using Items;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Lambda.Database;
using Lambda.Utils;

namespace Lambda.Entity
{
    class Area : IEntity
    {
        public enum OwnerType
        {
            CHARACTER,
            ORGANIZATION
        }
        public enum AreaType
        {
            NORMAL,
        }

        private uint id;
        private AreaType type;
        private uint radius;
        private Items.Inventory inventory;
        private OwnerType ownerType;
        private uint ownerId;
        public ICheckpoint AltCheckpoint;

        public byte CheckpointType
        {
            get => AltCheckpoint.CheckpointType;
        }

        public Position Position
        {
            get => AltCheckpoint.Position;
            set => AltCheckpoint.Position = value;
        }
        public Rotation Rotation { get; set; }
        public short World { get; set; }

        public Area(int typeId, Position position, float radius, float height, Rgba color)
        {
            CheckpointType type = (CheckpointType)typeId;
            AltCheckpoint = Alt.CreateCheckpoint(type, position, radius, height, new Rgba(241, 196, 15, 255));

        }

        private Dictionary<string, string> GetObjectData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["are_type"] = type.ToString();
            datas["are_position_x"] = Position.X.ToString();
            datas["are_position_y"] = Position.Y.ToString();
            datas["are_position_z"] = Position.Z.ToString();
            datas["are_radius"] = radius.ToString();
            datas["are_checkpoint"] = CheckpointType.ToString();
            return datas;
        }
        public long Insert()
        {
            Alt.Log("Insert d'une zone en cours ...");
            Dictionary<string, string> datas = GetObjectData();
            Alt.Log("datas créés ...");
            DBConnect dbConnect = DBConnect.DbConnect;
            Alt.Log("dbconnect ...");
            Alt.Log("Insert a performer ...");
            return dbConnect.Insert(TableName, datas);
        }

        public static string TableName = "t_area_are";
        public static List<Area> Areas = new List<Area>();

    }
}
