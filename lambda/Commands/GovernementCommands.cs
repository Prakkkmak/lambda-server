using Lambda.Entity;
using Lambda.Organizations;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class GovernementCommands
    {
        [Permission("GOUVERNEMENT_BASE")]
        [Command(Command.CommandType.GOVERNEMENT , 1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Gouvernement_Base(Player player, object[] argv)
        {
            uint amount = (uint)argv[0];
            Organization.BaseIncome = amount;
            Chat.SendInRange(player,0, "Le revenu de base a été changé par le gouvernement, il est de " + amount + "$", false);
            return new CmdReturn("Le salaire de base a été changé", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
