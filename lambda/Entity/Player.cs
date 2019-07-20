﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Async.Events;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using IVoiceChannel = AltV.Net.Elements.Entities.IVoiceChannel;
using Items;
using Lambda.Administration;
using Lambda.Clothing;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Buildings;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Skills;
using Lambda.Telephony;
using Lambda.Utils;
using MoreLinq;


namespace Lambda.Entity
{
    public class Player : AltV.Net.Elements.Entities.Player, IDBElement, IEntity
    {

        public Phone Phone;
        public List<Skill> Skills;
        public Permissions Permissions = new Permissions();

        public string License;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public ulong TimeOnline = 0;
        public ulong TotalTimeOnline = 0;


        public uint TicketLevel = 0;
        public new uint Id
        {
            get;
            set;
        }

        

        public long BankMoney;

        public short Food { get; set; }

        public ushort ServerId => base.Id;

        public Account Account { get; set; }

        public Inventory Inventory { get; set; }



        public Position FeetPosition => new Position(Position.X, Position.Y, Position.Z - 1);

        public Player PlayerSelected = null;
        public Vehicle VehicleSelected = null;

        public DateTime BanTime = default;

        public Skin Skin = new Skin();

        private uint deathCount;
        private uint id; // The id in the database
        


        private Request request;



        public Player(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {

            Skills = new List<Skill>();
            deathCount = 0;
            id = 0;
            Food = 0;
            FirstName = "";
            LastName = "";
            Account = null;
            Inventory = new Inventory(this);
            Inventory.Deposit(100000);
        }


        public void Spawn(Position pos)
        {
            //GotoLocation(new Location(pos, null, 0));
            Spawn(pos, 1000);
            Position = pos;
            Dimension = 0;
            Health = 100;
            
            Freeze(false);
        }

        public override string ToString()
        {
            return $"[{id}]{FullName}";
        }

        public void SendMessage(string msg)
        {
            Chat.Send(this, msg);
        }

        public void SendMessage(CmdReturn cmdReturn)
        {
            SendMessage(Chat.SendCmdReturn(cmdReturn));
        }

        public void Freeze(bool choice)
        {

            Emit("setFreeze", choice);
        }

        public void Goto(Position pos)
        {
            this.Position = pos;
        }
        public void Goto(Player player)
        {
            Position = player.Position;
            //GotoLocation(player.GetLocation());  // TODO
        }
        /*public void Goto(Interior interior)
         {
             this.GotoLocation(new Location(interior.Position, interior, (short)interior.Id));
         }*/

        public void SendRequest(Request r)
        {
            if (r.Answers.Count < 2) throw new Exception("Not enough answer");
            SendMessage(r.Text);
            SendMessage("Vous pouvez accepté ou refuser.");
            SetRequest(r);
            Emit("setInteraction", "test", r.Text, "Accepter", "Refuser");
        }

        public void SetRequest(Request r)
        {
            request = r;
        }

        public Request GetRequest()
        {
            return request;
        }


        public bool IsAllowedTo(string perm)
        {
            if (Permissions.Contains(perm)) return true;
            foreach (Rank rank in GetRanks())
            {
                if (rank.Permissions.Contains(perm)) return true;
            }
            return false;
        }

        public bool IsAllowedTo(Organization organization, string perm)
        {
            Member member = organization.GetMember(Id);
            return member != null && member.Rank.Permissions.Contains(perm);
        }

        /*public void LoadInterior(Interior interior)
        {
            foreach (string ipl in interior.GetIPLs())
            {
                Emit("loadIpl", ipl);
            }

        }

        public void UnloadInterior(Interior interior)
        {
            foreach (string ipl in interior.GetIPLs())
            {
                Emit("unloadIpl", ipl);
            }
        }*/
        public void LoadIpl(string ipl)
        {
            Emit("loadIpl", ipl);

        }
        public void LoadIpl(string[] ipls)
        {
            foreach (string ipl in ipls)
            {
                LoadIpl(ipl);
            }
        }
        public void LoadIpl(List<string> ipls)
        {
            foreach (string ipl in ipls)
            {
                LoadIpl(ipl);
            }
        }
        public void UnloadIpl(string ipl)
        {
            Emit("unloadIpl", ipl);

        }
        public void UnloadIpl(string[] ipls)
        {
            foreach (string ipl in ipls)
            {
                UnloadIpl(ipl);
            }
        }
        public void UnloadIpl(List<string> ipls)
        {
            foreach (string ipl in ipls)
            {
                UnloadIpl(ipl);
            }
        }

        public void Goto(Checkpoint checkpoint)
        {
            Dimension = checkpoint.Dimension;
            Position = checkpoint.Position;
        }

        public void Withdraw(long amount)
        {
            Inventory.Deposit(amount);
            BankMoney -= amount;
        }

        public void Deposit(long amount)
        {
            Inventory.Withdraw(amount);
            BankMoney += amount;
        }

        public long GetBankMoney()
        {
            return BankMoney;
        }
        public void SetBankMoney(long money)
        {
            BankMoney = money;
        }
        public T GetBuildings<T>(List<T> buildings) where T : Building
        {
            foreach (T b in buildings)
            {
                if (b.Checkpoint != null && b.Checkpoint.Position.Distance(Position) < b.Checkpoint.Range)
                {
                    return b;
                }
            }

            return null;
        }
        public House GetHouse()
        {
            foreach (House h in House.Houses)
            {
                if (h.InteriorCheckpoint != null && h.InteriorCheckpoint.Position.Distance(Position) < h.InteriorCheckpoint.Range)
                {
                    return h;
                }
                if (h.Checkpoint != null && h.Checkpoint.Position.Distance(Position) < h.Checkpoint.Range)
                {
                    return h;
                }
            }

            return null;
        }

        public Organization[] GetOrganizations()
        {
            List<Organization> organizations = new List<Organization>();
            foreach (Organization organization in Organization.Organizations)
            {
                if (organization.GetMember(Id) != null) organizations.Add(organization);
            }

            return organizations.ToArray();
        }

        public Rank[] GetRanks()
        {
            List<Rank> ranks = new List<Rank>();
            foreach (Organization organization in Organization.Organizations)
            {
                if (organization.GetMember(Id) != null) ranks.Add(organization.GetMember(Id).Rank);
            }

            return ranks.ToArray();
        }

        public Vehicle[] GetVehiclesInRange(int range)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            foreach (Vehicle vehicle in Lambda.Entity.Vehicle.Vehicles)
            {
                if (Position.Distance(vehicle.Position) < range) vehicles.Add(vehicle);
            }

            return vehicles.ToArray();
        }

        public new bool IsInVehicle
        {
            get { return Vehicle != null; }
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
            return Inventory.GetItemWithBaseItemIdAndMetaData((int)Enums.Items.CarKey, code) != null;
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



        public Player GetClosePlayerInDistance(int distance = 0)
        {
            Player closePlayer = null;
            Position targetPosition;
            foreach (Player player in Players)
            {
                if (player.Position.Distance(this.Position) < closePlayer.Position.Distance(this.Position) && player != this)
                {
                    closePlayer = player;
                }
            }

            if (closePlayer != null && distance <= 0) return closePlayer;
            if (closePlayer != null && closePlayer.Position.Distance(this.Position) < distance) return closePlayer;

            return null;
        }

        public Vehicle GetCloseVehicleInDistance(int distance = 0)
        {
            Vehicle closeVehicle = null;
            foreach (Vehicle vehicle in Lambda.Entity.Vehicle.Vehicles)
            {
                if (vehicle.Position.Distance(this.Position) < closeVehicle.Position.Distance(this.Position))
                {
                    closeVehicle = vehicle;
                }
            }

            if (closeVehicle != null && distance <= 0) return closeVehicle;
            if (closeVehicle != null && closeVehicle.Position.Distance(this.Position) < distance) return closeVehicle;

            return null;
        }

        public void GenerateContext()
        {
            List<Context> contexts = new List<Context>();
            if (PlayerSelected != null)
            {
                contexts.Add(new Context("Menotter", "police menotter"));
                contexts.Add(new Context("Menotter2", "police menotter"));
            }
            else
            {
                contexts.Add(new Context("Créer un véhicule", "vehicule sultan"));
                contexts.Add(new Context("Créer un véhicule2", "vehicule police"));
                contexts.Add(new Context("Déshabiller", "vet enl hab"));
            }

            string str = string.Join(',', contexts);
            Alt.Log("CONTEXTE" + str);
            Emit("setContextActions", "[" + str + "]");
        }


        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["cha_firstname"] = FirstName;
            data["cha_lastname"] = LastName;
            data["cha_position_x"] = Position.X.ToString();
            data["cha_position_Y"] = Position.Y.ToString();
            data["cha_position_z"] = Position.Z.ToString();
            data["cha_world"] = Dimension.ToString();
            data["cha_money"] = Inventory.Money.ToString();
            data["cha_hp"] = Health.ToString();
            data["cha_food"] = Food.ToString();
            data["cha_deathcount"] = "0";
            data["acc_id"] = Account.Id.ToString();
            data["cha_permissions"] = Permissions.ToString();
            data["inv_id"] = Inventory.Id.ToString();
            data["cha_bankaccount"] = GetBankMoney().ToString();
            data["cha_timeonline"] = TimeOnline.ToString();
            data["cha_totaltimeonline"] = TotalTimeOnline.ToString();
            data["cha_model"] = Skin.Model.ToString();
            data["cha_face"] = Skin.Face.ToString();
            data["cha_hairiness"] = Skin.Hairiness.ToString();
            data["cha_cosmetic"] = Skin.Cosmetic.ToString();
            data["cha_clothes"] = Skin.Clothes.ToString();
            if (Phone != null) data["pho_id"] = Phone.Id.ToString();
            return data;

        }

        public void SetData(Dictionary<string, string> data)
        {
            Id = uint.Parse(data["cha_id"]);
            FirstName = data["cha_firstname"];
            LastName = data["cha_lastname"];
            Inventory.Money = long.Parse(data["cha_money"]);
            SetBankMoney(long.Parse(data["cha_bankaccount"]));
            Food = short.Parse(data["cha_food"]);
            Position position = new Position();
            position.X = float.Parse(data["cha_position_x"]);
            position.Y = float.Parse(data["cha_position_y"]);
            position.Z = float.Parse(data["cha_position_z"]);
            Position = position;
            Dimension = short.Parse(data["cha_world"]);
            Health = ushort.Parse(data["cha_hp"]);
            if (!string.IsNullOrWhiteSpace(data["cha_permissions"]))
                Permissions.Set(data["cha_permissions"].Split(',').ToList());
            if (data["inv_id"] != null) DatabaseElement.Get<Inventory>(Inventory, uint.Parse(data["inv_id"]));
            else Inventory.Save();
            TimeOnline = ulong.Parse(data["cha_timeonline"]);
            TotalTimeOnline = ulong.Parse(data["cha_totaltimeonline"]);
            Skin.Model = Convert.ToUInt32(data["cha_model"]);
            if (data["pho_id"] != null) Phone = Phone.GetPhone(Convert.ToUInt32(data["pho_id"]));
            if (data["cha_face"].Split(',').Length == Skin.Face.ToString().Split(',').Length)
            {
                Skin.Face.Set(data["cha_face"]);
            }
            if (data["cha_hairiness"].Split(',').Length == Skin.Hairiness.ToString().Split(',').Length)
            {
                Skin.Hairiness.Set(data["cha_hairiness"]);
            }
            if (data["cha_cosmetic"].Split(',').Length == Skin.Cosmetic.ToString().Split(',').Length)
            {
                Skin.Cosmetic.Set(data["cha_cosmetic"]);
            }
            Alt.Log(data["cha_clothes"].Split(',').Length + " " + Skin.Clothes.ToString().Split(',').Length);
            Alt.Log(data["cha_clothes"]);
            Alt.Log(Skin.Clothes.ToString());
            if (data["cha_clothes"].Split(',').Length == Skin.Clothes.ToString().Split(',').Length)
            {
                Skin.Clothes.Set(data["cha_clothes"]);
            }
            Skin.Send(this);
            _ = SaveAsync();
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            Account.Save();
            Inventory.Save();
            foreach (Skill playerSkill in Skills)
            {
                playerSkill.Save();
            }
            DatabaseElement.Save(this);
            Alt.Log("Player Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            Players.Remove(this);
            VoiceChannel.RemovePlayer(this);
            Alt.EmitAllClients("chatmessage", $"{FullName} c'est déconnecté!");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await Account.SaveAsync();
            await Inventory.SaveAsync();
            foreach (Skill playerSkill in Skills)
            {
                await playerSkill.SaveAsync();
            }
            Alt.Log("Start save player");
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Player Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public static void AddPlayer(Player player)
        {
            Alt.Log("t1");
            Players.Add(player);
            Alt.Log("t2");
            VoiceChannel.AddPlayer(player);
            Alt.Log("t3");
            Alt.EmitAllClients("chatmessage", null, $"{player.FullName} s'est connecté!");
            Alt.Log("t4");
        }
        public static Player GetPlayerByDbId(uint id)
        {
            foreach (Player player in Players)
            {
                if (player.Id == id) return player;
            }

            return null;
        }

        public static Player[] GetPlayers(string nameOrId)
        {
            nameOrId.Replace('_', ' ');
            List<Player> players = new List<Player>();
            foreach (Player player in Players)
            {
                if (player.ServerId.ToString().Equals(nameOrId)) players.Add(player);
                else if (player.FullName.ToLower().StartsWith(nameOrId.ToLower())) players.Add(player);
            }
            return players.ToArray();
        }

        public static Player GetPlayer(uint databaseid)
        {
            foreach (Player player in Players)
            {
                if (player.Id == databaseid) return player;
            }

            return null;
        }

        public static IVoiceChannel VoiceChannel = Alt.CreateVoiceChannel(true, 15);
        public static List<Player> Players = new List<Player>();
    }
}
