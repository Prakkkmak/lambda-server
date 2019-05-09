using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net.Data;
using Items;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Skills;

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
            data["cha_world"] = player.Dimension.ToString();
            data["cha_money"] = player.Inventory.Money.ToString();
            data["cha_hp"] = player.Hp.ToString();
            data["cha_food"] = player.Food.ToString();
            data["cha_deathcount"] = "0";
            data["acc_id"] = player.Account.Id.ToString();
            data["ski_id"] = player.GetSkin().Id.ToString();
            data["cha_permissions"] = string.Join(",", player.Permissions);
            data["inv_id"] = player.Inventory.Id.ToString();
            data["cha_bankaccount"] = player.GetBankMoney().ToString();
            data["cha_timeonline"] = player.TimeOnline.ToString();
            data["cha_totaltimeonline"] = player.TotalTimeOnline.ToString();
            //data["baa_id"] = bankAccount.Id.ToString();
            //data["lic_id"] = license.Id.ToString();
            return data;

        }

        public override void SetData(Player player, Dictionary<string, string> data)
        {
            player.Id = uint.Parse(data["cha_id"]);
            player.FirstName = data["cha_firstname"];
            player.LastName = data["cha_lastname"];
            player.Inventory.Money = long.Parse(data["cha_money"]);
            player.SetBankMoney(long.Parse(data["cha_bankaccount"]));
            player.Food = short.Parse(data["cha_food"]);
            if (player.AltPlayer == null) return;
            Position position = new Position();
            position.X = float.Parse(data["cha_position_x"]);
            position.Y = float.Parse(data["cha_position_y"]);
            position.Z = float.Parse(data["cha_position_z"]);
            player.Position = position;
            player.Dimension = short.Parse(data["cha_world"]);
            player.Hp = ushort.Parse(data["cha_hp"]);
            player.GetSkin().Id = uint.Parse(data["ski_id"]);
            if (!string.IsNullOrWhiteSpace(data["cha_permissions"]))
                player.Permissions = data["cha_permissions"].Split(',').ToList();
            if (data["inv_id"] != null) player.Game.DbInventory.Get(uint.Parse(data["inv_id"]), player.Inventory);
            else player.Game.DbInventory.Save(player.Inventory);
            Game.DbSkin.Get(player.GetSkin().Id, player.GetSkin());
            player.TimeOnline = ulong.Parse(data["cha_timeonline"]);
            player.TotalTimeOnline = ulong.Parse(data["cha_totaltimeonline"]);

        }
        public Player Get(Account account, Player player)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            string index = "acc_id";
            where[index] = account.Id.ToString();
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return default(Player);
            SetData(player, result);
            player.Skills = Game.DbSkill.GetAll(player).ToList();
            return player;
        }
        public override void Save(Player player)
        {
            Game.DbAccount.Save(player.Account);
            Game.DbSkin.Save(player.GetSkin());
            Game.DbInventory.Save(player.Inventory);
            foreach (Skill playerSkill in player.Skills)
            {
                Game.DbSkill.Save(playerSkill);
            }
            base.Save(player);
        }


    }
}
