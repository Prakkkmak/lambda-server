using Lambda.Items;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Lambda.Database;
using Lambda.Utils;

namespace Lambda.Entity
{
    public class Vehicle : AltV.Net.Elements.Entities.Vehicle, IEntity, IDBElement
    {
        public enum OwnerType
        {
            CHARACTER,
            ORGANIZATION
        }

        public uint Id { get; set; }

        public ushort Fuel = 0;
        public OwnerType ownerType = OwnerType.CHARACTER;
        public uint OwnerId = 0;

        public IVoiceChannel VoiceChannel { get; set; }

        public Inventory Inventory;
        public Lock Lock { get; set; }

        //public IVehicle AltVehicle { get; set; }
        public VehicleModel Model { get; set; } = VehicleModel.Adder;
        public Position SpawnPosition { get; set; } = Position.Zero;
        public Rotation SpawnRotation { get; set; } = Rotation.Zero;

        public Vehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            this.Inventory = new Inventory(this);
            this.VoiceChannel = Alt.CreateVoiceChannel(true, 10);
            this.Lock = new Lock(10, Lock.Complexity.NUMERICAL, Lock.Complexity.ALPHAMAJ);
        }

        public void Respawn()
        {
            Position = SpawnPosition;
        }

        public void Repair()
        {
            BodyHealth = 100;
            EngineHealth = 100;
            BodyAdditionalHealth = 100;
            PetrolTankHealth = 100;
            DamageData = "AA==";
            HealthData = "DwAi";
        }

        public void Park()
        {
            SpawnPosition = Position;
            Save();
        }

        public void Park(Rotation rot)
        {
            SpawnPosition = Position;
            SpawnRotation = rot;
        }

        public void SetEngine(bool status)
        {
            EngineOn = status;
            if (status) PetrolTankHealth = 100;
            else PetrolTankHealth = 0;
        }

        public OwnerType GetOwnerType()
        {
            return ownerType;
        }

        public void SetOwnerType(OwnerType owner)
        {
            ownerType = owner;
        }

        public void SetOwner(uint id, OwnerType owner = OwnerType.CHARACTER)
        {
            OwnerId = id;
            SetOwnerType(owner);
        }
        public void SetOwner(Player player)
        {
            SetOwner(player.Id);
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["veh_model"] = Model.ToString();
            data["veh_position_x"] = SpawnPosition.X.ToString();
            data["veh_position_y"] = SpawnPosition.Y.ToString();
            data["veh_position_z"] = SpawnPosition.Z.ToString();
            data["veh_rotation_r"] = SpawnRotation.Roll.ToString();
            data["veh_rotation_p"] = SpawnRotation.Pitch.ToString();
            data["veh_rotation_y"] = SpawnRotation.Yaw.ToString();
            data["veh_color_r"] = PrimaryColorRgb.R.ToString();
            data["veh_color_g"] = PrimaryColorRgb.G.ToString();
            data["veh_color_b"] = PrimaryColorRgb.B.ToString();
            data["veh_color2_r"] = SecondaryColorRgb.R.ToString();
            data["veh_color2_g"] = SecondaryColorRgb.G.ToString();
            data["veh_color2_b"] = SecondaryColorRgb.B.ToString();
            data["veh_lock"] = Lock.Code;
            data["veh_plate"] = NumberplateText;
            if (OwnerId == 0) return data;
            if (GetOwnerType() == Vehicle.OwnerType.CHARACTER) data["cha_id"] = OwnerId.ToString();
            else if (GetOwnerType() == Vehicle.OwnerType.ORGANIZATION) data["org_id"] = OwnerId.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Model = (VehicleModel)Enum.Parse(typeof(VehicleModel), data["veh_model"]);
            Position position = new Position();
            position.X = float.Parse(data["veh_position_x"]);
            position.Y = float.Parse(data["veh_position_y"]);
            position.Z = float.Parse(data["veh_position_z"]);
            SpawnPosition = position;
            Lock.Code = data["veh_lock"];
            //Spawn();
            Rotation rotation = new Rotation();
            rotation.Roll = float.Parse(data["veh_rotation_r"]);
            rotation.Pitch = float.Parse(data["veh_rotation_p"]);
            rotation.Yaw = float.Parse(data["veh_rotation_y"]);
            Rotation = rotation;
            Rgba color = new Color();
            color.R = byte.Parse(data["veh_color_r"]);
            color.G = byte.Parse(data["veh_color_g"]);
            color.B = byte.Parse(data["veh_color_b"]);
            Rgba secondaryColor = new Color();
            secondaryColor.R = byte.Parse(data["veh_color2_r"]);
            secondaryColor.G = byte.Parse(data["veh_color2_g"]);
            secondaryColor.B = byte.Parse(data["veh_color2_b"]);
            NumberplateText = data["veh_plate"];
            PrimaryColorRgb = color;
            SecondaryColorRgb = secondaryColor;
            if (data.ContainsKey("cha_id"))
            {
                SetOwnerType(Vehicle.OwnerType.CHARACTER);
                OwnerId = uint.Parse(data["cha_id"]);
            }
            if (data.ContainsKey("org_id"))
            {
                SetOwnerType(Vehicle.OwnerType.ORGANIZATION);
                OwnerId = uint.Parse(data["org_id"]);
            }
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Vehicle Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            DatabaseElement.Delete(this);
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Vehicle Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public static void AddVehicle(Vehicle vehicle)
        {
            Vehicles.Add(vehicle);
        }

        public static void LoadVehicles()
        {
            Vehicles.AddRange(DatabaseElement.GetAllVehicles());
        }


        public static List<Vehicle> Vehicles = new List<Vehicle>();
    }
}
