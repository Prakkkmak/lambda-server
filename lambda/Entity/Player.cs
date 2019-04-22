using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Items;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Entity
{
    public class Player : IDBElement, IEntity
    {

        private uint deathCount;
        private uint id; // The id in the database
        private Skin skin;
        private List<Request> Requests;
        private long bankMoney;


        public uint Id { get; set; }

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

        public Account Account { get; set; }

        public short Food { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName}_{LastName}";
        public Inventory Inventory { get; set; }
        public Interior Interior { get; set; }

        public IPlayer AltPlayer { get; }
        public ushort ServerId => AltPlayer.Id;
        public Game Game { get; }



        public short Dimension
        {
            get => AltPlayer.Dimension;
            set {
                AltPlayer.Dimension = value;
                //Game.VoiceChannel.RemovePlayer(AltPlayer);
                //Game.VoiceChannel.AddPlayer(AltPlayer);
            }
        }
        public ushort Hp
        {
            get => AltPlayer.Health;
            set => AltPlayer.Health = value;
        }
        public Position Position
        {
            get => AltPlayer.Position;
            set => AltPlayer.Position = value;
        }
        public Position FeetPosition => new Position(AltPlayer.Position.X, AltPlayer.Position.Y, AltPlayer.Position.Z - 1);

        public Rotation Rotation
        {
            get => AltPlayer.Rotation;
            set => AltPlayer.Rotation = value;
        }


        public Player()
        {
            Permissions = new List<string>();
            Requests = new List<Request>();
            deathCount = 0;
            id = 0;
            Food = 0;
            FirstName = "";
            LastName = "";
            Account = null;
            skin = new Skin { Player = this };
            Inventory = new Inventory(this);
            Inventory.Deposit(100000);


        }

        public Player(IPlayer altPlayer, Game game) : this()
        {
            Game = game;
            AltPlayer = altPlayer;
            altPlayer.SetData("player", this);


        }



        public void SetSkin(Skin skin)
        {
            this.skin = skin;
            this.skin.Player = this;
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

        public void Freeze(bool choice)
        {
            AltPlayer.Emit(choice ? "freeze" : "unfreeze");
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

        public void AddRequest(Request r)
        {
            Requests.Add(r);
        }

        public Request[] GetRequests()
        {
            return Requests.ToArray();
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
                this.AltPlayer.Emit("loadipl", ipl);
            }

        }
        public void UnloadInterior(Interior interior)
        {
            foreach (string ipl in interior.GetIPLs())
            {
                this.AltPlayer.Emit("unloadipl", ipl);
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


    }
}
