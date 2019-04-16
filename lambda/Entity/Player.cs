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

        public uint Id { get; set; }

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


        public Skin Skin => skin;

        public Inventory Inventory { get; set; }


        public IPlayer AltPlayer { get; }
        public ushort ServerId => AltPlayer.Id;
        public Game Game { get; }

        public short World
        {
            get => AltPlayer.Dimension;
            set => AltPlayer.Dimension = value;
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
        public Position FeetPosition => new Position(AltPlayer.Position.X, AltPlayer.Position.Y - 1, AltPlayer.Position.Z);

        public Rotation Rotation
        {
            get => AltPlayer.Rotation;
            set => AltPlayer.Rotation = value;
        }


        public Player()
        {
            Requests = new List<Request>();
            deathCount = 0;
            id = 0;
            Food = 0;
            FirstName = "";
            LastName = "";
            Account = null;
            skin = new Skin();
            Inventory = new Inventory();
            Inventory.Money = 10000;
        }

        public Player(IPlayer altPlayer, Game game) : this()
        {
            Game = game;
            AltPlayer = altPlayer;
            altPlayer.SetData("player", this);
           

        }



        public void SetSkin(Skin skin)
        {
            uint id = this.skin.Id;
            this.skin = skin;
            this.skin.Id = id;
            this.skin.SendSkin(this);
        }

        public void Spawn(Position pos)
        {
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

        public void AddRequest(Request r)
        {
            Requests.Add(r);
        }

        public Request[] GetRequests()
        {
            return Requests.ToArray();
        }


    }
}
