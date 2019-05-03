using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{

    class TestCommands
    {
        [Command(Command.CommandType.DEFAULT, 1)]
        [Syntax("Modele")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Model(Player player, object[] argv)
        {
            int nbr = Convert.ToInt32((string)argv[0], 16);
            player.AltPlayer.Model = (uint)nbr;
            return CmdReturn.Success;
        }

    }
}
