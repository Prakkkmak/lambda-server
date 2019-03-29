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
    public class Player
    {

        private uint deathCount;
        private uint id; // The id in the database


        public ulong Money { get; set; }
        public short Food { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName}_{LastName}";
        public Account Account { get; set; }
        public Skin Skin { get; set; }
        public Inventory Inventory { get; set; }


        public IPlayer AltPlayer { get; }
        public ushort ServerId => AltPlayer.Id;
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
        public Rotation Rotation
        {
            get => AltPlayer.Rotation;
            set => AltPlayer.Rotation = value;
        }

        public bool IsDefaultPlayer => Account == null; // Check if the player have an account

        public Player(IPlayer altPlayer)
        {
            AltPlayer = altPlayer;
            altPlayer.SetData("player", this);
            deathCount = 0;
            id = 0;
            Money = 0;
            Food = 0;
            FirstName = "";
            LastName = "";
            Account = null;
            Skin = new Skin();
            Inventory = new Inventory();

        }

        public void Save()
        {
            if (id == 0)
            {
                id = (uint)Insert();
                OnlinePlayers.Add(this);
            }
            else
            {
                Update();
            }
            Skin.Save();
        }
        public bool Load()
        {
            Dictionary<string, string> datas = SelectByAccountId(Account.Id);
            if (datas.Count == 0) return false;
            id = uint.Parse(datas["cha_id"]);
            FirstName = datas["cha_firstname"];
            LastName = datas["cha_lastname"];
            Position position = new Position();
            position.X = float.Parse(datas["cha_position_x"]);
            position.Y = float.Parse(datas["cha_position_y"]);
            position.Z = float.Parse(datas["cha_position_z"]);
            Position = position;
            World = short.Parse(datas["cha_world"]);
            Money = ulong.Parse(datas["cha_money"]);
            Hp = ushort.Parse(datas["cha_hp"]);
            Food = short.Parse(datas["cha_food"]);
            deathCount = uint.Parse(datas["cha_deathcount"]);
            Skin.Id = uint.Parse(datas["ski_id"]);
            Skin.Load();
            Skin.SendSkin(this);
            OnlinePlayers.Add(this);
            return true;
        }

        public void Spawn(Position pos)
        {
            AltPlayer.Spawn(pos);
        }

        public override string ToString()
        {
            return $"[{id}]{Name}";
        }

        public void SendMessage(string msg)
        {
            Chat.Send(this, msg);
        }

        public static Player[] GetPlayers(string nameOrId)
        {
            List<Player> players = new List<Player>();
            foreach (Player onlinePlayer in OnlinePlayers)
            {
                if (onlinePlayer.ServerId.ToString().Equals(nameOrId)) players.Add(onlinePlayer);
                else if (onlinePlayer.Name.StartsWith(nameOrId)) players.Add(onlinePlayer);
            }

            return players.ToArray();
        }



        #region database

        private Dictionary<string, string> GetPlayerData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas["cha_firstname"] = FirstName;
            datas["cha_lastname"] = LastName;
            datas["cha_position_x"] = Position.X.ToString();
            datas["cha_position_Y"] = Position.Y.ToString();
            datas["cha_position_z"] = Position.Z.ToString();
            datas["cha_world"] = World.ToString();
            datas["cha_money"] = Money.ToString();
            datas["cha_hp"] = Hp.ToString();
            datas["cha_food"] = Food.ToString();
            datas["cha_deathcount"] = deathCount.ToString();
            datas["acc_id"] = Account.Id.ToString();
            datas["ski_id"] = Skin.Id.ToString();
            //datas["inv_id"] = Inventory.Id.ToString();
            //datas["baa_id"] = bankAccount.Id.ToString();
            //datas["lic_id"] = license.Id.ToString();
            return datas;
        }

        private long Insert()
        {
            Dictionary<string, string> datas = GetPlayerData();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Insert(TableName, datas);
        }

        private void Update()
        {
            Dictionary<string, string> datas = GetPlayerData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["cha_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            dbConnect.Update(TableName, datas, wheres);
        }

        public static Dictionary<string, string> Select(uint id)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["cha_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        public static Dictionary<string, string> SelectByAccountId(uint id)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["acc_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        #endregion


        public static string TableName = "t_character_cha";
        public static Position SpawnPosition = new Position(131.0769f, -1302.343f, 29.22925f);
        public static List<Player> OnlinePlayers = new List<Player>();
    }
}
