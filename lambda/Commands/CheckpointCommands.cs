using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Housing;

namespace Lambda.Commands
{
    class CheckpointCommands
    {
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Action(Player player, object[] argv)
        {
            Checkpoint checkpoint = null;
            foreach (Checkpoint c in Checkpoint.Checkpoints)
            {

                if (c.Position.Distance(player.Position) < c.Range) checkpoint = c;
            }
            if (checkpoint == null) return new CmdReturn("Il n'y a rien ici", CmdReturn.CmdReturnType.WARNING);
            checkpoint.Action(player);
            return new CmdReturn("DEBUG: Action effectuée", CmdReturn.CmdReturnType.WARNING);
        }
    }
}
