using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Buildings;
using Lambda.Utils;

namespace Lambda.Commands
{
    class HouseCommands
    {
        [Permission("ADMINISTRATEUR_MAISON_CREER")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Maison_Creer(Player player, object[] argv)
        {
            House house = new House(player.Position);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_MAISON_INTERIEUR")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("IPL1,IPL2,...")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Maison_Interieur(Player player, object[] argv)
        {
            string ipls = (string)argv[0];
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            house.SetIpls(ipls);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez changé l'interieur de la maison.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_MAISON_POSITION")]
        [Command(Command.CommandType.ADMIN, 3)]
        [Syntax("x", "y", "z")]
        [SyntaxType(typeof(int), typeof(int), typeof(int))]
        public static CmdReturn Maison_Position(Player player, object[] argv)
        {
            Position pos = new Position((int)argv[0], (int)argv[1], (int)argv[2]);
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            house.SetInterior(pos);
            house.SetExterior(house.Checkpoint.Position);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez changé la position de la maison.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_MAISON_ACHETER")]
        [Command(Command.CommandType.HOUSE)]
        public static CmdReturn Maison_Info(Player player, object[] argv)
        {
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            if (house.Price > player.Inventory.Money) return CmdReturn.NoEnoughMoney;
            return new CmdReturn($"Proprio : {house.Owner} prix de vente: {house.Price}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_MAISON_VENDRE")]
        [Command(Command.CommandType.HOUSE,1)]
        [Syntax("Prix")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Maison_Vendre(Player player, object[] argv)
        {
            uint price = (uint)argv[0];
            Position pos = new Position((int)argv[0], (int)argv[1], (int)argv[2]);
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            if (house.Owner != player.Id) return new CmdReturn("Ce n'est pas votre maison");
            house.Price = price;
            _ = house.SaveAsync();
            return new CmdReturn($"Proprio : {house.Owner} prix de vente: {house.Price}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_MAISON_PRIX")]
        [Command(Command.CommandType.HOUSE, 1)]
        [Syntax("Prix")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Maison_Prix(Player player, object[] argv)
        {
            uint price = (uint)argv[0];
            Position pos = new Position((int)argv[0], (int)argv[1], (int)argv[2]);
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            house.Price = price;
            _ = house.SaveAsync();
            return new CmdReturn($"Proprio : {house.Owner} prix de vente: {house.Price}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_MAISON_ACHETER")]
        [Command(Command.CommandType.HOUSE)]
        public static CmdReturn Maison_Acheter(Player player, object[] argv)
        {
            Position pos = new Position((int)argv[0], (int)argv[1], (int)argv[2]);
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            if (house.Price > player.Inventory.Money) return CmdReturn.NoEnoughMoney;
            player.Inventory.Money -= house.Price;
            house.Price = 0;
            house.Owner = player.Id;
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez acehté la maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
