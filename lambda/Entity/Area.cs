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
        public short World { get; set; }

        public Area(float radius, float height, AreaType type = AreaType.NORMAL)
        {
            Inventory = new Inventory();
            Radius = radius;
            Height = height;
            Type = type;


        }
        public Area(Dictionary<string, string> datas)
        {
            Inventory = new Inventory();
            Radius = uint.Parse(datas["are_radius"]);
            Id = uint.Parse(datas["are_id"]);
            Type = AreaType.NORMAL;
            Height = 1;
            CheckpointTypeId = 1;

        }

        public void Spawn(int checkpointId, Position position, Rgba color)
        {
            AltCheckpoint = Alt.CreateCheckpoint((byte)checkpointId, position, Radius, 2, color);
        }

        public virtual string GetMetaData()
        {
            return "";
        }

        public void Save()
        {
            if (Id == 0)
            {
                Id = (uint)Insert();
            }
            else
            {
                Update();
            }
        }


        public static List<Area> GetAreaInPos(Position position)
        {
            List<Area> areas = new List<Area>();
            foreach (Area area in Areas)
            {
                float distance = PositionHelper.Distance(position, area.Position);
                if (distance < area.Radius)
                {
                    areas.Add(area);
                }
            }

            return areas;
        }
        public static List<Area> GetAreaInPos(Position position, AreaType areaType)
        {
            List<Area> areas = new List<Area>();
            foreach (Area area in Areas)
            {
                if (area.Type != areaType) continue;
                float distance = PositionHelper.Distance(position, area.Position);
                if (distance < area.Radius)
                {
                    areas.Add(area);
                }
            }

            return areas;
        }

        public static void LoadAll()
        {
            List<Dictionary<string, string>> datasList = SelectAll();
            foreach (Dictionary<string, string> datas in datasList)
            {
                Position position;
                position.X = float.Parse(datas["are_position_x"]);
                position.Y = float.Parse(datas["are_position_y"]);
                position.Z = float.Parse(datas["are_position_z"]);
                Rgba color = new Rgba(0, 255, 0, 255);
                Area area = null;
                if (datas["are_type"] == "NORMAL")
                {
                    area = new Area(datas);
                }
                if (datas["are_type"] == "SHOP")
                {
                    area = new Shop(datas);
                }

                if (area == null) continue;
                area.Spawn(area.CheckpointTypeId, position, color);
                Areas.Add(area);


            }
        }

        #region database

        private Dictionary<string, string> GetObjectData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["are_type"] = Type.ToString();
            datas["are_position_x"] = Position.X.ToString();
            datas["are_position_y"] = Position.Y.ToString();
            datas["are_position_z"] = Position.Z.ToString();
            datas["are_radius"] = Radius.ToString();
            datas["are_metadata"] = GetMetaData();
            //datas["are_checkpoint"] = Type.ToString();
            return datas;
        }


        public void Delete()
        {

            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["are_id"] = Id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            dbConnect.Delete(TableName, wheres);
            AltCheckpoint.Remove();
            Areas.Remove(this);
        }

        public long Insert()
        {
            Dictionary<string, string> datas = GetObjectData();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Insert(TableName, datas);
        }

        public long Update()
        {
            Dictionary<string, string> datas = GetObjectData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["are_id"] = Id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Update(TableName, datas, wheres);
        }

        public static Dictionary<string, string> Select(int id)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["are_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        public static List<Dictionary<string, string>> SelectAll()
        {
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Select(TableName, new Dictionary<string, string>());
        }




        public static string TableName = "t_area_are";

        #endregion


        public static List<Area> Areas = new List<Area>();

    }
}
