using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Items;

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
            Area.Areas.Add(shop);
            shop.Spawn(shop.CheckpointTypeId, player.FeetPosition, new Rgba(255, 255, 255, 255));
            shop.Save();
            return new CmdReturn("Vous avez créé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Magasin_Supprimer(Player player, string[] argv)
        {
            List<Shop> shops = Area.GetAreaInPos(player.Position, Area.AreaType.SHOP).OfType<Shop>().ToList();
            if (shops.Count < 1) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            Shop shop = shops[0];
            shop.Delete();
            return new CmdReturn("Vous avez supprimé un magasin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "id", "prix")]
        public static CmdReturn Magasin_Ajouter_Vente(Player player, string[] argv)
        {
            List<Shop> shops = Area.GetAreaInPos(player.Position, Area.AreaType.SHOP).OfType<Shop>().ToList();
            if (shops.Count < 1) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            Shop shop = shops[0];
            if (!uint.TryParse(argv[3], out uint id)) return new CmdReturn("Veuillez rentrer un id valide", CmdReturn.CmdReturnType.SYNTAX);
            if (!int.TryParse(argv[4], out int price)) return new CmdReturn("Veuillez entrer un prix valide", CmdReturn.CmdReturnType.SYNTAX);
            if (BaseItem.GetBaseItem(id) == null) return new CmdReturn("Cet objet n'existe pas !", CmdReturn.CmdReturnType.WARNING);
            shop.AddSell(id, price);
            shop.Save();
            return new CmdReturn("Vous avez ajouté une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, "id")]
        public static CmdReturn Magasin_Enlever_Vente(Player player, string[] argv)
        {
            List<Shop> shops = Area.GetAreaInPos(player.Position, Area.AreaType.SHOP).OfType<Shop>().ToList();
            if (shops.Count < 1) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            Shop shop = shops[0];
            if (!uint.TryParse(argv[3], out uint id)) return new CmdReturn("Veuillez rentrer un id valide", CmdReturn.CmdReturnType.SYNTAX);
            if (BaseItem.GetBaseItem(id) == null) return new CmdReturn("Cet objet n'existe pas !", CmdReturn.CmdReturnType.WARNING);
            shop.RemoveSell(id);
            shop.Save();
            return new CmdReturn("Vous avez supprimé une vente !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Magasin_Inspecter(Player player, string[] argv)
        {
            List<Shop> shops = Area.GetAreaInPos(player.Position, Area.AreaType.SHOP).OfType<Shop>().ToList();
            if (shops.Count < 1) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            Shop shop = shops[0];
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
            List<Shop> shops = Area.GetAreaInPos(player.Position, Area.AreaType.SHOP).OfType<Shop>().ToList();
            if (shops.Count < 1) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            Shop shop = shops[0];
            if (!uint.TryParse(argv[2], out uint id)) return new CmdReturn("Id Incorrect", CmdReturn.CmdReturnType.WARNING);
            if (!int.TryParse(argv[3], out int amount)) return new CmdReturn("Prix invalide", CmdReturn.CmdReturnType.WARNING);
            CmdReturn returnType = shop.Sell(id, (uint)amount, player.Inventory);
            if (returnType.Type != CmdReturn.CmdReturnType.SUCCESS) return returnType;
            return new CmdReturn("Vous avez acheté un objet !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
