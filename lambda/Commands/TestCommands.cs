using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Enums;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Commands
{

    class TestCommands
    {
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Modele")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Modele(Player player, object[] argv)
        {
            if (!Enum.TryParse((string)argv[0], true, out PedModel model))
            {
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            if (!Enum.IsDefined(typeof(PedModel), model))
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);

            //int nbr = Convert.ToInt32((string)argv[0], 16);
            //player.AltPlayer.Model = (uint)Convert.ToInt32(model.ToString(), 16);
            player.AltPlayer.Model = (uint)model;
            player.GetSkin().Model = (string)argv[0];
            return CmdReturn.Success;
        }



        [Command(Command.CommandType.TEST, 1)]
        [Syntax("LockState")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Lockstate(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (!Enum.TryParse((string)argv[0], true, out VehicleLockState lockstate))
            {
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            if (!Enum.IsDefined(typeof(VehicleLockState), lockstate))
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            vehicle.SetLock(lockstate);
            return new CmdReturn("Vous avez changé letat du lock");
        }


        [Command(Command.CommandType.TEST)]
        public static CmdReturn vingtsept(Player player, object[] argv)
        {
            return new CmdReturn("C 2 sECR3t http://zzefds.fr");
        }
    }
}


