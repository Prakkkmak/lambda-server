using Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
        public IBlip AltBlip { get; set; }

        public float Radius { get; set; }
        public float Height { get; set; }
        public byte CheckpointTypeId { get; set; }
        public ushort BlipTypeId { get; set; }

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
            if (type == AreaType.BANK)
            {
                BlipTypeId = 0;

            }

        }

        public void Spawn(Position position)
        {
            AltCheckpoint = Alt.CreateCheckpoint((byte)this.CheckpointTypeId, position, Radius, 2, new Rgba(0, 0, 0, 255));
            //AltBlip = Alt.CreateBlip(4, position);
            //AltBlip.Sprite = BlipTypeId;
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


        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["are_type"] = Type.ToString();
            data["are_position_x"] = Position.X.ToString();
            data["are_position_y"] = Position.Y.ToString();
            data["are_position_z"] = Position.Z.ToString();
            data["are_radius"] = Radius.ToString();
            data["are_metadata"] = GetMetaData();
            if (InteriorLocation.Equals(default(Location))) data["are_interior"] = "0";
            else data["are_interior"] = InteriorLocation.Interior.Id.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Enum.TryParse(data["are_type"], out Area.AreaType type);
            Type = type;
            Position position = new Position();
            position.X = float.Parse(data["are_position_x"]);
            position.Y = float.Parse(data["are_position_y"]);
            position.Z = float.Parse(data["are_position_z"]);
            Radius = float.Parse(data["are_radius"]);
            Spawn(position);
            SetMetaData(data["are_metadata"]);
            Interior interior = Interior.GetInterior(uint.Parse(data["are_interior"]));
            if (interior != null) SetLocations(interior, short.Parse(data["are_id"]));
        }

        public void Remove()
        {
            Areas.Remove(this);
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Area Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Area Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }


        public static void AddArea(Area area)
        {
            Areas.Add(area);
        }
        public static Area GetArea(Position position)
        {
            foreach (Area area in Areas)
            {
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }

        public static Area GetArea(Position position, Area.AreaType type)
        {
            foreach (Area area in Areas)
            {
                if (area.Type != type) continue;
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }

        public static Area GetArea(Player player, Area.AreaType type)
        {
            return GetArea(player.FeetPosition, type);
        }


        public static Location GetDestination(Position position, short dimension)
        {
            foreach (Area area in Areas)
            {
                if (area.InteriorLocation.Position.Distance(position) < area.Radius && area.Id == dimension)
                {
                    return area.ExteriorLocation;
                }
                if (area.ExteriorLocation.Position.Distance(position) < area.Radius && area.Dimension == dimension)
                {
                    return area.InteriorLocation;
                }
            }

            return default;
        }
        public void RemoveArea(Area area)
        {
            Areas.Remove(area);
            area.AltCheckpoint.Remove();
        }
        public static void LoadAreas()
        {
            Areas.AddRange(DatabaseElement.GetAllAreas());
        }

        public static List<Area> Areas = new List<Area>();
    }
}
