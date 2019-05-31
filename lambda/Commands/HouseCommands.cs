﻿using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class HouseCommands
    {
        [Command(Command.CommandType.HOUSE)]
        public static CmdReturn Maison_Creer(Player player, object[] argv)
        {
            House house = new House();
            Area.AddArea(house);
            house.Spawn(player.FeetPosition);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.HOUSE, 1)]
        [Syntax("Interieur")]
        [SyntaxType(typeof(Interior))]
        public static CmdReturn Maison_Interieur(Player player, object[] argv)
        {
            Interior interior = (Interior)argv[0];
            House house = (House)Area.GetArea(player.FeetPosition, Area.AreaType.HOUSE);
            if (house == null) return new CmdReturn("Aucune maison n a ete trouvée", CmdReturn.CmdReturnType.LOCATION);
            house.SetLocations(interior, (short)house.Id);
            _ = house.SaveAsync();
            return new CmdReturn("Vous avez changé l'interieur!", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
