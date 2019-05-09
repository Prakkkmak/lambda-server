using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Items;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Skills;
using Lambda.Utils;

namespace Lambda.Entity
{
    public class Player : IDBElement, IEntity
    {

        public List<Skill> Skills;
        public List<string> Permissions;

        public string license
        {
            get {
                if (AltPlayer != null)
                {
                    AltPlayer.GetData("license", out string result);
                    return result;
                }

                return "";
            }

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName}_{LastName}";

        public ulong TimeOnline = 0;
        public ulong TotalTimeOnline = 0;

        public uint Id
        {
            get;
            set;
        }

        public short Food { get; set; }
        public short Dimension
        {
            get => AltPlayer.Dimension;
            set {
                if (AltPlayer != null)
                {
                    AltPlayer.Dimension = value;
                }
            }
        }

        public ushort Hp
        {
            get => AltPlayer.Health;
            set {
                if (AltPlayer != null)
                {
                    AltPlayer.Health = value;
                }
            }


        }
        public ushort ServerId => AltPlayer.Id;

        public Account Account { get; set; }

        public Inventory Inventory { get; set; }

        public Interior Interior { get; set; }

        public Game Game { get; set; }

        public Vehicle Vehicle
        {
            get {
                if (!AltPlayer.IsInVehicle || AltPlayer.Vehicle == null) return null;
                AltPlayer.Vehicle.GetData("vehicle", out Vehicle vehicle);
                return vehicle;
            }
        }

        public IPlayer AltPlayer { get; }

        public Position Position
        {
            get => AltPlayer.Position;
            set {
                if (AltPlayer != null)
                {
                    AltPlayer.Position = value;
                }
            }

        }
        public Position FeetPosition => new Position(AltPlayer.Position.X, AltPlayer.Position.Y, AltPlayer.Position.Z - 1);

        public Rotation Rotation
        {
            get => AltPlayer.Rotation;
            set => AltPlayer.Rotation = value;
        }

        private uint deathCount;
        private uint id; // The id in the database

        private long bankMoney;

        private Skin skin;

        private Request request;

        public Player()
        {
            Skills = new List<Skill>();
            Permissions = new List<string>();
            deathCount = 0;
            id = 0;
            Food = 0;
            FirstName = "";
            LastName = "";
            Account = null;
            skin = new Skin();
            Inventory = new Inventory(this);
            Inventory.Deposit(100000);
        }

        public Player(IPlayer altPlayer, Game game) : this()
        {
            Game = game;
            AltPlayer = altPlayer;
            altPlayer.SetData("player", this);
            skin.Game = game;
            skin.SendModel(this);
            skin.SendSkin(this);
        }



        public void SetSkin(Skin skin)
        {
            this.skin = skin;
            //this.skin.Player = this;
            this.skin.SendSkin(this);
        }

        public Skin GetSkin()
        {
            return skin;
        }

        public void Spawn(Position pos)
        {
            GotoLocation(new Location(pos, null, 0));
            AltPlayer.Spawn(pos);
            Freeze(false);
        }

        public override string ToString()
        {
            return $"[{id}]{Name}";
        }

        public void SendMessage(string msg)
        {
            Game.Chat.Send(this, msg);
        }

        public void SendMessage(CmdReturn cmdReturn)
        {
            SendMessage(Game.Chat.SendCmdReturn(cmdReturn));
        }

        public void Freeze(bool choice)
        {

            AltPlayer.Emit("setfreeze", choice);
        }

        public void Goto(Position pos)
        {
            this.Position = pos;
        }
        public void Goto(Player player)
        {
            GotoLocation(player.GetLocation());
        }
        public void Goto(Interior interior)
        {
            this.GotoLocation(new Location(interior.Position, interior, (short)interior.Id));
        }

        public void SendRequest(Request r)
        {
            if (r.Answers.Count < 2) throw new Exception("Not enough answer");
            SendMessage(r.Text);
            SendMessage("Vous pouvez accepté ou refuser.");
            SetRequest(r);
        }

        public void SetRequest(Request r)
        {
            request = r;
        }

        public Request GetRequest()
        {
            return request;
        }

        public void AddPermission(string permission)
        {
            RemovePermission(permission);
            Permissions.Add(permission.ToUpper());
        }
        public void RemovePermission(string permission)
        {
            permission = permission.ToUpper();
            Permissions.RemoveAll(perm => perm.StartsWith(permission));
        }
        public bool PermissionExist(string permission)
        {
            foreach (string perm in Permissions)
            {

                if (permission.StartsWith(perm)) return true;
            }
            return false;
        }

        public void LoadInterior(Interior interior)
        {
            foreach (string ipl in interior.GetIPLs())
            {
                this.AltPlayer.Emit("loadIpl", ipl);
            }

        }
        public void UnloadInterior(Interior interior)
        {
            foreach (string ipl in interior.GetIPLs())
            {
                this.AltPlayer.Emit("unloadIpl", ipl);
            }
        }

        public Location GetLocation()
        {
            return new Location(Position, Interior, Dimension);
        }

        public void GotoLocation(Location location)
        {
            if (Interior != null) UnloadInterior(Interior);
            if (location.Interior != null)
            {
                LoadInterior(location.Interior);
                Interior = location.Interior.Clone();
            }
            Dimension = location.Dimension;
            Position = location.Position;
        }
        public void ExitInterior()
        {
            if (Interior == null) return;
            GotoLocation(new Location(Interior.Area.Position, null, 0));
        }

        public void Withdraw(long amount)
        {
            Inventory.Deposit(amount);
            bankMoney -= amount;
        }

        public void Deposit(long amount)
        {
            Inventory.Withdraw(amount);
            bankMoney += amount;
        }

        public long GetBankMoney()
        {
            return bankMoney;
        }
        public void SetBankMoney(long money)
        {
            bankMoney = money;
        }

        public Organization[] GetOrganizations()
        {
            List<Organization> organizations = new List<Organization>();
            foreach (Organization organization in Game.GetOrganizations())
            {
                if (organization.GetMember(Id) != null) organizations.Add(organization);
            }

            return organizations.ToArray();
        }

        public Vehicle[] GetVehiclesInRange(int range)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            foreach (Vehicle vehicle in Game.GetVehicles())
            {
                if (Position.Distance(vehicle.Position) < range) vehicles.Add(vehicle);
            }

            return vehicles.ToArray();
        }

        public Vehicle GetNearestVehicle()
        {
            Vehicle nearestVehicle = null;
            foreach (Vehicle vehicle in Game.GetVehicles())
            {
                if (nearestVehicle == null ||
                    Position.Distance(vehicle.Position) < Position.Distance(nearestVehicle.Position))
                {
                    nearestVehicle = vehicle;
                }
            }

            return nearestVehicle;
        }

        public Vehicle GetNearestVehicle(int range)
        {
            Vehicle nearestVehicle = null;
            foreach (Vehicle vehicle in GetVehiclesInRange(range))
            {
                if (nearestVehicle == null ||
                    Position.Distance(vehicle.Position) < Position.Distance(nearestVehicle.Position))
                {
                    nearestVehicle = vehicle;
                }
            }

            return nearestVehicle;
        }

        public bool HaveKeyOf(string code)
        {
            return Inventory.GetItemWithBaseItemIdAndMetaData(1000, code) != null;
        }

        public Skill GetSkill(Skill.SkillType type)
        {
            foreach (Skill skill in Skills)
            {
                if (type == skill.Type) return skill;
            }

            return null;
        }
        public void AddExperience(Skill.SkillType type, long xp)
        {
            Skill skill = GetSkill(type);
            if (skill == null)
            {
                skill = new Skill(this, type);
                Skills.Add(skill);
            }
            skill.AddExperience(xp);
        }

        public void RemoveExperience(Skill.SkillType type, long xp)
        {
            Skill skill = GetSkill(type);
            if (skill == null)
            {
                skill = new Skill(this, type);
                Skills.Add(skill);
            }
            skill.RemoveExperience(xp);
            if (skill.GetExperience() < 0)
            {
                Skills.Remove(skill);
            }
        }

    }
}
