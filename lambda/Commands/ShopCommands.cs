using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Elements.Entities;
using Lambda.Entity;
using Lambda.Items;
using Player = Lambda.Entity.Player;

namespace Lambda.Commands
{
    class ShopCommands
    {
        [Command(Command.CommandType.SHOP)]
        public static CmdReturn Magasin_Creer(Player player, object[] argv)
        {
            Shop shop = new Shop();
            player.Game.AddArea(shop);
            shop.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez créé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.SHOP)]
        public static CmdReturn Magasin_Supprimer(Player player, object[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.FeetPosition, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            player.Game.RemoveArea(shop);
            return new CmdReturn("Vous avez supprimé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.SHOP, 2)]
        [Syntax("Objet", "Prix")]
        [SyntaxType(typeof(BaseItem), typeof(int))]
        public static CmdReturn Magasin_Ajouter_Vente(Player player, object[] argv)
        {
            BaseItem baseItem = (BaseItem)argv[0];
            int price = (int)argv[1];
            Shop shop = (Shop)player.Game.GetArea(player.FeetPosition, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (price < 0) return new CmdReturn("Veuillez entrer un prix valide", CmdReturn.CmdReturnType.SYNTAX);
            shop.AddSell(baseItem.Id, price);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez ajouté une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.SHOP, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(BaseItem))]
        public static CmdReturn Magasin_Enlever_Vente(Player player, object[] argv)
        {
            BaseItem baseItem = (BaseItem)argv[0];
            Shop shop = (Shop)player.Game.GetArea(player.FeetPosition, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (shop.GetSell(baseItem.Id).Equals(default(Sell))) return new CmdReturn("Cet objet n'est pas en vente !", CmdReturn.CmdReturnType.WARNING);
            shop.RemoveSell(baseItem.Id);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez supprimé une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.SHOP)]
        public static CmdReturn Magasin_Inspecter(Player player, object[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.FeetPosition, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            string sellsString = "";
            foreach (Sell sell in shop.Sells)
            {
                sellsString += $"[{sell.ItemId}]{sell.Price}$<br>";
            }

            if (sellsString.Length < 1) sellsString = "Le magasin est vide";
            return new CmdReturn(sellsString);
        }
        [Command(Command.CommandType.SHOP, 2)]
        [Syntax("Objet", "Montant")]
        [SyntaxType(typeof(BaseItem), typeof(uint))]
        public static CmdReturn Magasin_Acheter(Player player, object[] argv)
        {
            BaseItem baseItem = (BaseItem)argv[0];
            uint amount = (uint)argv[1];
            Shop shop = (Shop)player.Game.GetArea(player.FeetPosition, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            CmdReturn returnType = shop.Sell(baseItem.Id, amount, player.Inventory);
            if (returnType.Type != CmdReturn.CmdReturnType.SUCCESS) return returnType;
            return new CmdReturn("Vous avez acheté des objets !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
