using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Lambda.Utils;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda.Commands
{
    class VehicleCommands
    {
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Modele")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule(Player player, object[] argv)
        {
            if (!Enum.TryParse((string)argv[0], true, out VehicleModel model))
            {
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            }
            if (!Enum.IsDefined(typeof(VehicleModel), model)) return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);

            Vehicle vehicle = new Vehicle(Vector3.Near(player.Position), model);
            player.Game.AddVehicle(vehicle);
            vehicle.Spawn();
            return new CmdReturn("Vous avez fait apparaitre un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Park(Player player, object[] argv)
        {
            Vehicle veh = player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.Park();
            player.Game.DbVehicle.Save(veh);
            return new CmdReturn("Vous avez sauvegardé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.VEHICLE, 3)]
        [Syntax("R", "G", "B")]
        [SyntaxType(typeof(byte), typeof(byte), typeof(byte))]
        public static CmdReturn Vehicule_Couleur(Player player, object[] argv)
        {
            Vehicle veh = player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.Color = new Rgba((byte)argv[0], (byte)argv[1], (byte)argv[2], 255);
            return new CmdReturn("Vous avez changé la couleur de votre véhicule ! ", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.VEHICLE, 3)]
        [Syntax("R", "G", "B")]
        [SyntaxType(typeof(byte), typeof(byte), typeof(byte))]
        public static CmdReturn Vehicule_Couleur_Secondaire(Player player, object[] argv)
        {
            Vehicle veh = player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.SecondaryColor = new Rgba((byte)argv[0], (byte)argv[1], (byte)argv[2], 255);
            return new CmdReturn("Vous avez changé la couleur de votre véhicule ! ", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Supprimer(Player player, object[] argv)
        {
            Vehicle veh = player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            player.Game.RemoveVehicle(veh);
            return new CmdReturn("Vous avez supprimé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}
