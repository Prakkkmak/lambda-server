using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{
    class DoctorCommands
    {
        [Permission("DOCTOR_HEAL")]
        [Command(Command.CommandType.POLICE)]
        public static CmdReturn Hopital_Soigner(Player player, object[] argv)
        {
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            target.Health = 100;
            target.SendMessage("Vous avez été soigné");
            return new CmdReturn("Vous avez soigné quelqu'un");
        }
    }
}
