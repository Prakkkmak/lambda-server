using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Housing;
using Lambda.Utils;

namespace Lambda.Commands
{
    class HouseCommands
    {
        [Command(Command.CommandType.HOUSE)]
        public static CmdReturn Maison_Creer(Player player, object[] argv)
        {
            House house = new House(player.Position);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.HOUSE, 1)]
        [Syntax("IPL1,IPL2,...")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Maison_Interieur(Player player, object[] argv)
        {
            string ipls = (string)argv[0];
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            house.SetIpls(ipls);
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.HOUSE, 3)]
        [Syntax("x", "y", "z")]
        [SyntaxType(typeof(int), typeof(int), typeof(int))]
        public static CmdReturn Maison_Position(Player player, object[] argv)
        {
            Position pos = new Position((int)argv[0], (int)argv[1], (int)argv[2]);
            House house = player.GetHouse();
            if (house == null) return new CmdReturn("Pas de maison ici", CmdReturn.CmdReturnType.WARNING);
            house.SetInterior(pos);
            house.SetExterior(house.Exterior.Position);
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
