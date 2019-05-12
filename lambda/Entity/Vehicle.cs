using Lambda.Items;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class Vehicle : IEntity, IDBElement
    {
        public enum OwnerType
        {
            CHARACTER,
            ORGANIZATION
        }

        private ushort fuel;
        private OwnerType ownerType;
        private uint ownerId;
        private ushort rank; // The rank of acces to the AltVehicle
        private ushort hp;
        private Rgba color;


        public IVoiceChannel VoiceChannel { get; set; }

        public uint Id { get; set; }


        public Inventory Inventory;
        public IVehicle AltVehicle { get; set; }
        public VehicleModel Model { get; set; }
        public Position SpawnPosition { get; set; }
        public Rotation SpawnRotation { get; set; }

        public Lock Lock { get; set; }

        public Position Position
        {
            get => AltVehicle?.Position ?? SpawnPosition;
            set {
                if (AltVehicle != null) AltVehicle.Position = value;
            }
        }

        public Rotation Rotation
        {
            get => AltVehicle?.Rotation ?? SpawnRotation;
            set {
                if (AltVehicle != null) AltVehicle.Rotation = value;
            }
        }

        public Rgba Color
        {
            get => AltVehicle?.PrimaryColorRgb ?? new Rgba(0, 0, 0, 0);
            set {
                if (AltVehicle != null) AltVehicle.PrimaryColorRgb = value;
            }
        }



        public Rgba SecondaryColor
        {
            get => AltVehicle?.SecondaryColorRgb ?? new Rgba(0, 0, 0, 0);
            set {
                if (AltVehicle != null) AltVehicle.SecondaryColorRgb = value;
            }
        }

        public short Dimension { get; set; }

        public Vehicle()
        {
            this.SpawnPosition = new Position(0, 0, 0);
            this.SpawnRotation = new Rotation(0, 0, 0);
            this.Model = VehicleModel.Adder;
            this.Id = 0;
            this.fuel = 100;
            this.ownerType = OwnerType.CHARACTER;
            this.ownerId = 0;
            this.rank = 0;
            this.hp = 100;
            this.Inventory = new Inventory(this);
            this.VoiceChannel = Alt.CreateVoiceChannel(true, 10);
            this.Lock = new Lock(10, Lock.Complexity.NUMERICAL, Lock.Complexity.ALPHAMAJ);
        }

        public Vehicle(Position position) : this()
        {
            this.SpawnPosition = position;
        }

        public Vehicle(Position position, VehicleModel model) : this()
        {
            this.SpawnPosition = position;
            this.Model = model;
        }

        public void Respawn()
        {
            Rgba col1 = this.Color;
            Rgba col2 = this.SecondaryColor;
            AltVehicle.Remove();
            this.Spawn();
            this.Color = col1;
            this.SecondaryColor = col2;
        }
        public void Spawn()
        {
            this.AltVehicle = Alt.CreateVehicle(Model, SpawnPosition, SpawnRotation);
            this.AltVehicle.SetData("vehicle", this);
            this.Color = new Rgba(0, 0, 0, 255);
            this.SecondaryColor = new Rgba(0, 0, 0, 0);
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
            AltVehicle.EngineOn = status;
            if (status) AltVehicle.PetrolTankHealth = 100;
            else AltVehicle.PetrolTankHealth = 0;
        }
        public bool GetEngine()
        {
            return AltVehicle.EngineOn;
        }

        public void SetPlate(string text)
        {
            AltVehicle.NumberplateText = text;
        }

        public string GetPlate()
        {
            return AltVehicle.NumberplateText;

        }

        public void SetLock(VehicleLockState state)
        {
            AltVehicle.LockState = state;
        }

        public VehicleLockState GetLock()
        {
            return AltVehicle.LockState;
        }

        public uint GetOwnerId()
        {
            return ownerId;
        }

        public void SetOwnerId(uint id)
        {
            ownerId = id;
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
            SetOwnerId(id);
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
            data["veh_color_r"] = Color.R.ToString();
            data["veh_color_g"] = Color.G.ToString();
            data["veh_color_b"] = Color.B.ToString();
            data["veh_color2_r"] = SecondaryColor.R.ToString();
            data["veh_color2_g"] = SecondaryColor.G.ToString();
            data["veh_color2_b"] = SecondaryColor.B.ToString();
            data["veh_lock"] = Lock.Code;
            data["veh_plate"] = GetPlate();
            if (GetOwnerId() == 0) return data;
            if (GetOwnerType() == Vehicle.OwnerType.CHARACTER) data["cha_id"] = GetOwnerId().ToString();
            else if (GetOwnerType() == Vehicle.OwnerType.ORGANIZATION) data["org_id"] = GetOwnerId().ToString();
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
            Spawn();
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
            SetPlate(data["veh_plate"]);
            Color = color;
            SecondaryColor = secondaryColor;
            if (data.ContainsKey("cha_id"))
            {
                SetOwnerType(Vehicle.OwnerType.CHARACTER);
                SetOwnerId(uint.Parse(data["cha_id"]));
            }
            if (data.ContainsKey("org_id"))
            {
                SetOwnerType(Vehicle.OwnerType.ORGANIZATION);
                SetOwnerId(uint.Parse(data["org_id"]));
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

        public void Remove()
        {
            Vehicles.Remove(this);
            this.AltVehicle.Remove();
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
