using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Commands
{
    class ShopCmds
    {
        // Set a skin in a specific slot
        // /vetement 1 0
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Magasin_Creer(Player player, string[] argv)
        {
            Shop shop = new Shop();
            player.Game.AddArea(shop);
            shop.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez créé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Maison_Creer(Player player, string[] argv)
        {
            House house = new House();
            player.Game.AddArea(house);
            house.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(house);

            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT, "ID")]
        public static CmdReturn Maison_Interieur(Player player, string[] argv)
        {
            House house = (House)player.Game.GetArea(player.Position, Area.AreaType.HOUSE);
            if (house == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[2], out uint interiorid)) return new CmdReturn("Parametre incorrect", CmdReturn.CmdReturnType.WARNING);
            Interior interior = player.Game.GetInterior(interiorid);
            if (interior == null) return new CmdReturn("Aucun interieur trouvé", CmdReturn.CmdReturnType.WARNING);
            //Location location = new Location(interior.Position, interior, 10);
            player.Game.DbArea.Save(house);
            house.SetLocations(interior, (short)house.Id);

            return new CmdReturn("Vous avez changé l'interieur!", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn TP(Player player, string[] argv)
        {
            Location location = player.Game.GetDestination(player.Position, player.Dimension);
            //if (area == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (player.AltPlayer.Vehicle != null) return new CmdReturn("");
            if (location.Equals(default(Location))) return new CmdReturn("");
            player.GotoLocation(location);
            return new CmdReturn("Vous vous êtes téléporté", CmdReturn.CmdReturnType.SUCCESS);
        }

        // Set a skin in a specific slot
        // /vetement 1 0
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Banque_Creer(Player player, string[] argv)
        {
            Area bank = new Area(2, 2, Area.AreaType.BANK);
            player.Game.AddArea(bank);
            bank.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(bank);
            return new CmdReturn("Vous avez créé une banque !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "amount")]
        public static CmdReturn Retirer(Player player, string[] argv)
        {

            Area shop = player.Game.GetArea(player.Position, Area.AreaType.BANK);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[1], out uint amount)) return new CmdReturn("Montant Incorrect", CmdReturn.CmdReturnType.WARNING);
            player.Withdraw(amount);
            return new CmdReturn("Vous avez retiré de l'argent !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "amount")]
        public static CmdReturn Deposer(Player player, string[] argv)
        {

            Area shop = player.Game.GetArea(player.Position, Area.AreaType.BANK);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[1], out uint amount)) return new CmdReturn("Montant Incorrect", CmdReturn.CmdReturnType.WARNING);
            player.Deposit(amount);
            return new CmdReturn("Vous avez déposé de l'argent !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Balance(Player player, string[] argv)
        {
            return new CmdReturn($"Vous avez {player.GetBankMoney()}$ sur votre compte.");
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Interieur_Liste(Player player, string[] argv)
        {
            Interior[] interiors = player.Game.GetInteriors();
            string txt = "Voici la liste des interieurs : <br>";
            foreach (Interior interior in interiors)
            {
                txt += interior.Id + " - " + interior.GetIPLs()[0] + "<br>";
            }
            return new CmdReturn(txt, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Interieur_Recharger(Player player, string[] argv)
        {
            player.Game.AddAllInteriors();
            return new CmdReturn("tonton", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT, "ID")]
        public static CmdReturn Interieur_Goto(Player player, string[] argv)
        {
            if (!uint.TryParse(argv[2], out uint result)) return new CmdReturn("Syntaxe incorrecte", CmdReturn.CmdReturnType.SYNTAX);
            Interior interior = player.Game.GetInterior(result);
            if (interior == null) return new CmdReturn("Cet interrieur n'existe pas", CmdReturn.CmdReturnType.WARNING);
            player.Goto(interior);
            return new CmdReturn("Vous vous etes téléporté dans un interieur", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Magasin_Supprimer(Player player, string[] argv)
        {

            Shop shop = (Shop)player.Game.GetArea(player.Position, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            player.Game.RemoveArea(shop);
            return new CmdReturn("Vous avez supprimé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "id", "prix")]
        public static CmdReturn Magasin_Ajouter_Vente(Player player, string[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.Position, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[3], out uint id)) return new CmdReturn("Veuillez rentrer un id valide", CmdReturn.CmdReturnType.SYNTAX);
            if (!int.TryParse(argv[4], out int price)) return new CmdReturn("Veuillez entrer un prix valide", CmdReturn.CmdReturnType.SYNTAX);
            if (price < 0) return new CmdReturn("Veuillez entrer un prix valide", CmdReturn.CmdReturnType.SYNTAX);
            if (player.Game.GetBaseItem(id) == null) return new CmdReturn("Cet objet n'existe pas !", CmdReturn.CmdReturnType.WARNING);
            shop.AddSell(id, price);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez ajouté une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "id")]
        public static CmdReturn Magasin_Enlever_Vente(Player player, string[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.Position, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[3], out uint id)) return new CmdReturn("Veuillez rentrer un id valide", CmdReturn.CmdReturnType.SYNTAX);
            if (player.Game.GetBaseItem(id) == null) return new CmdReturn("Cet objet n'existe pas !", CmdReturn.CmdReturnType.WARNING);
            if (shop.GetSell(id).Equals(default(Sell))) return new CmdReturn("Cet objet n'est pas en vente !", CmdReturn.CmdReturnType.WARNING);
            shop.RemoveSell(id);
            player.Game.DbArea.Save(shop);
            return new CmdReturn("Vous avez supprimé une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Magasin_Inspecter(Player player, string[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.Position, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            string sellsString = "";
            foreach (Sell sell in shop.Sells)
            {
                sellsString += $"[{sell.ItemId}]{sell.Price}$<br>";
            }

            if (sellsString.Length < 1) sellsString = "Le magasin est vide";
            return new CmdReturn(sellsString, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "Id", "Nombre")]
        public static CmdReturn Magasin_Acheter(Player player, string[] argv)
        {
            Shop shop = (Shop)player.Game.GetArea(player.Position, Area.AreaType.SHOP);
            if (shop == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (!uint.TryParse(argv[2], out uint id)) return new CmdReturn("Id Incorrect", CmdReturn.CmdReturnType.WARNING);
            if (!int.TryParse(argv[3], out int amount)) return new CmdReturn("Prix invalide", CmdReturn.CmdReturnType.WARNING);
            CmdReturn returnType = shop.Sell(id, (uint)amount, player.Inventory);
            if (returnType.Type != CmdReturn.CmdReturnType.SUCCESS) return returnType;
            return new CmdReturn("Vous avez acheté des objets !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
