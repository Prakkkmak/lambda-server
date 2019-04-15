using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Items;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Database
{
    public class DbPlayer : DbElement<Player>
    {
        public DbPlayer(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Player player)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["cha_firstname"] = player.FirstName;
            data["cha_lastname"] = player.LastName;
            data["cha_position_x"] = player.Position.X.ToString();
            data["cha_position_Y"] = player.Position.Y.ToString();
            data["cha_position_z"] = player.Position.Z.ToString();
            data["cha_world"] = player.World.ToString();
            data["cha_money"] = player.Inventory.Money.ToString();
            data["cha_hp"] = player.Hp.ToString();
            data["cha_food"] = player.Food.ToString();
            data["cha_deathcount"] = "0";
            data["acc_id"] = player.Account.Id.ToString();
            data["ski_id"] = player.Skin.Id.ToString();
            //data["inv_id"] = Inventory.Id.ToString();
            //data["baa_id"] = bankAccount.Id.ToString();
            //data["lic_id"] = license.Id.ToString();
            return data;

        }

        public override void SetData(Player player, Dictionary<string, string> data)
        {
            player.Id = uint.Parse(data["cha_id"]);
            player.FirstName = data["cha_firstname"];
            player.LastName = data["cha_lastname"];
            Position position = new Position();
            position.X = float.Parse(data["cha_position_x"]);
            position.Y = float.Parse(data["cha_position_y"]);
            position.Z = float.Parse(data["cha_position_z"]);
            player.Position = position;
            player.World = short.Parse(data["cha_world"]);
            player.Inventory.Money = ulong.Parse(data["cha_money"]);
            player.Hp = ushort.Parse(data["cha_hp"]);
            player.Food = short.Parse(data["cha_food"]);
            player.Skin.Id = uint.Parse(data["ski_id"]);
            //player.Skin.Load();
            //player.Skin.SendSkin(player);
        }
        public Player Get(Account account, Player player)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "acc_id";
            where[index] = account.Id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return default(Player);
            SetData(player, result);
            return player;
        }

    }
}
