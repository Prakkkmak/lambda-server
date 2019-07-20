using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class InstructorCommands
    {

        [Permission("INSTRUCTEUR_DELIVRER_CONDUIRE")]
        [Command(Command.CommandType.INSTRUCTOR, 2)]
        [Syntax("Prénom", "Nom")]
        [SyntaxType(typeof(string), typeof(string))]
        public static CmdReturn Instructeur_Delivrer_Conduire(Player player, object[] argv)
        {
            string firstName = (string)argv[0];
            string lastName = (string)argv[1];
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            target.Inventory.AddItem(Enums.Items.DrivingLicense, 1, firstName + " " + lastName);
            target.SendMessage("Vous avez reçu un permis de conduire");
            return new CmdReturn("Vous avez délivré un permis de conduire");
        }
    }
}
