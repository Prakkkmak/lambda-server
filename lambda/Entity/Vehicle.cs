using Lambda.Items;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Lambda.Database;
using Lambda.Utils;

namespace Lambda.Entity
{
    public class Vehicle : IEntity
    {
        public enum OwnerType
        {
            CHARACTER,
            ORGANIZATION
        }


        private bool saved;
        private uint id;
        private VehicleModel model;
        private ushort fuel;
        private OwnerType ownerType;
        private uint ownerId;
        private ushort rank; // The rank of acces to the AltVehicle
        private ushort hp;
        public Inventory Inventory;
        public readonly IVehicle AltVehicle;

        public Position SpawnPosition { get; set; }
        public Rotation SpawnRotation { get; set; }

        public Position Position
        {
            get => AltVehicle.Position;
            set => AltVehicle.Position = value;
        }

        public Rotation Rotation
        {
            get => AltVehicle.Rotation;
            set => AltVehicle.Rotation = value;
        }
        public short World { get; set; }

        public Rgba Color
        {
            get => AltVehicle.PrimaryColorRgb;
            set => AltVehicle.PrimaryColorRgb = value;
        }

        public Rgba SecondaryColor
        {
            get => Rgba.Zero;
            set => AltVehicle.SecondaryColorRgb = value;
        }

        public int Hp
        {
            get => AltVehicle.EngineHealth;
            set => AltVehicle.EngineHealth = value;
        }

        public Vehicle(Position position, VehicleModel model = VehicleModel.Adder)
        {
            SpawnPosition = position;
            SpawnRotation = new Rotation(0, 0, 0);
            this.model = model;
            AltVehicle = Spawn();
            AltVehicle.SetData("vehicle", this);

            this.id = 0;
            this.Color = new Rgba(241, 196, 15, 255);
            this.SecondaryColor = new Rgba(199, 199, 199, 255);
            this.fuel = 100;
            this.ownerType = OwnerType.CHARACTER;
            this.ownerId = 0;
            this.rank = 0;
            this.hp = 100;
            this.Inventory = new Inventory();
        }

        public Vehicle(Dictionary<string, string> datas)
        {
            this.saved = true;
            this.id = uint.Parse(datas["veh_id"]);
            string modelName = datas["veh_model"];
            Position position = new Position();
            position.X = float.Parse(datas["veh_position_x"]);
            position.Y = float.Parse(datas["veh_position_y"]);
            position.Z = float.Parse(datas["veh_position_z"]);
            Rotation rotation = new Rotation();
            rotation.pitch = float.Parse(datas["veh_rotation_p"]);
            rotation.roll = float.Parse(datas["veh_rotation_r"]);
            rotation.yaw = float.Parse(datas["veh_rotation_y"]);
            SpawnPosition = position;
            SpawnRotation = rotation;
            bool result = Enum.TryParse(modelName, true, out VehicleModel model);
            this.model = model;
            if (result) AltVehicle = Spawn();
            AltVehicle.SetData("vehicle", this);
            Rgba color = new Rgba();
            color.r = byte.Parse(datas["veh_color_r"]);
            color.g = byte.Parse(datas["veh_color_g"]);
            color.b = byte.Parse(datas["veh_color_b"]);
            Color = color;
            Rotation = rotation;
            fuel = ushort.Parse(datas["veh_fuel"]);
            rank = ushort.Parse(datas["veh_rank"]);
            Hp = int.Parse(datas["veh_hp"]);
            this.Inventory = new Inventory();

        }

        public IVehicle Spawn()
        {
            return Alt.CreateVehicle(model, SpawnPosition, SpawnRotation.pitch);
        }

        public void Repair()
        {
            AltVehicle.BodyHealth = 100;
            AltVehicle.EngineHealth = 100;
            AltVehicle.BodyAdditionalHealth = 100;
            AltVehicle.PetrolTankHealth = 100;
            hp = 100;
            AltVehicle.DamageData = "AA==";
            AltVehicle.HealthData = "DwAi";
        }

        public void SetColor(byte r, byte g, byte b, byte a = 255)
        {
            Rgba color = new Rgba(r, g, b, a);
            Color = color;
        }

        public void Park()
        {
            SpawnPosition = Position;
            Update();
        }

        private Dictionary<string, string> GetVehicleData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["veh_model"] = model.ToString();
            datas["veh_position_x"] = SpawnPosition.X.ToString();
            datas["veh_position_y"] = SpawnPosition.Y.ToString();
            datas["veh_position_z"] = Position.Z.ToString();
            datas["veh_rotation_r"] = SpawnRotation.roll.ToString();
            datas["veh_rotation_p"] = SpawnRotation.pitch.ToString();
            datas["veh_rotation_y"] = SpawnRotation.yaw.ToString();
            datas["veh_color_r"] = Color.r.ToString();
            datas["veh_color_g"] = Color.g.ToString();
            datas["veh_color_b"] = Color.b.ToString();
            datas["veh_fuel"] = fuel.ToString();
            datas["veh_rank"] = rank.ToString();
            datas["veh_hp"] = Hp.ToString();
            return datas;
        }

        public void Insert()
        {
            Dictionary<string, string> datas = GetVehicleData();
            DBConnect dbConnect = DBConnect.DbConnect;
            saved = true;
            id = (uint)dbConnect.Insert(TableName, datas);
        }

        public void Update()
        {
            Alt.Log("Update d'un véhicule en cours ...");
            if (!saved)
            {
                Alt.Log("Vehicule non  sauvegardé ; démarage du insert.");
                Insert();
                return;
            }
            Dictionary<string, string> datas = GetVehicleData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["veh_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            Alt.Log("Update a performer ...");
            dbConnect.Update(TableName, datas, wheres);
        }

        public void Delete()
        {
            if (saved)
            {
                Dictionary<string, string> wheres = new Dictionary<string, string>();
                wheres["veh_id"] = id.ToString();
                DBConnect dbConnect = DBConnect.DbConnect;
                dbConnect.Delete(TableName, wheres);
            }

            Vehicles.Remove(this);
            AltVehicle.SetData("vehicule", null);
            AltVehicle.Remove();
        }

        public static Dictionary<string, string> Select(int id, DBConnect dbConnect = null)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["veh_id"] = id.ToString();
            dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        public static void UpdateAll()
        {
            foreach (Vehicle veh in Vehicles)
            {
                if (veh.saved) veh.Update();

            }
        }

        public static void LoadAllVehicles()
        {
            DBConnect dbConnect = DBConnect.DbConnect;
            List<Dictionary<string, string>> results = dbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                Vehicle veh = new Vehicle(result);
                Vehicles.Add(veh);
            }
        }

        public static List<Vehicle> Vehicles = new List<Vehicle>();
        public static string TableName = "t_vehicle_veh";
    }
}
