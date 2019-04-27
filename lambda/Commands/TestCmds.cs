using Lambda.Administration;
using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{
    public class TestCmds
    {
        [Command(Command.CommandType.ADMIN, "ipl")]
        public static CmdReturn Ipl(Player player, string[] argv)
        {
            player.AltPlayer.Emit("loadipl", argv[1]);
            return new CmdReturn("Vous avez chargé un ipl", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN, "ipl")]
        public static CmdReturn Iplr(Player player, string[] argv)
        {
            player.AltPlayer.Emit("unloadipl", argv[1]);
            return new CmdReturn("Vous avez chargé un ipl", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
