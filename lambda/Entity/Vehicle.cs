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

        public Game Game { get; set; }

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

    }
}
