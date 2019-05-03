using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class HouseCommands
    {
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.HOUSE)]
        public static CmdReturn Maison_Creer(Player player, object[] argv)
        {
            House house = new House();
            player.Game.AddArea(house);
            house.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(house);
            return new CmdReturn("Vous avez créé une maison !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.HOUSE, 1)]
        [Syntax("Interieur")]
        [SyntaxType(typeof(Interior))]
        public static CmdReturn Maison_Interieur(Player player, object[] argv)
        {
            Interior interior = (Interior)argv[0];
            House house = (House)player.Game.GetArea(player.FeetPosition, Area.AreaType.HOUSE);
            if (house == null) return new CmdReturn("Aucune maison n a ete trouvée", CmdReturn.CmdReturnType.LOCATION);
            player.Game.DbArea.Save(house);
            house.SetLocations(interior, (short)house.Id);

            return new CmdReturn("Vous avez changé l'interieur!", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
