using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class AreaCommands
    {
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.AREA)]
        public static CmdReturn Interieur_Liste(Player player, object[] argv)
        {
            Interior[] interiors = player.Game.GetInteriors();
            string txt = "Voici la liste des interieurs : <br>";
            foreach (Interior interior in interiors)
            {
                txt += interior.Id + " - " + interior.GetIPLs()[0] + "<br>";
            }
            return new CmdReturn(txt);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.AREA)]
        public static CmdReturn Interieur_Recharger(Player player, object[] argv)
        {
            player.Game.AddAllInteriors();
            return new CmdReturn("Vous avez rechargé les interieurs", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.AREA, 1)]
        [Syntax("Interieur")]
        [SyntaxType(typeof(Interior))]
        public static CmdReturn Interieur_Goto(Player player, object[] argv)
        {
            Interior interior = (Interior)argv[0];
            player.Goto(interior);
            return new CmdReturn("Vous vous etes téléporté dans un interieur", CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}
