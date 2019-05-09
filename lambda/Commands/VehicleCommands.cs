using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
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
            player.Inventory.AddItem(1000, 1, vehicle.Lock.Code);
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

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Clef(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            if (vehicle.GetEngine())
            {
                vehicle.SetEngine(false);
                return new CmdReturn("Vous avez etteint le moteur");
            }
            else
            {
                vehicle.SetEngine(true);
                return new CmdReturn("Vous avez allumé le moteur");
            }


        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Fermer(Player player, object[] argv)
        {
            Vehicle vehicle = player.GetNearestVehicle(5);
            if (vehicle == null) return new CmdReturn("Veh pas trouvé");
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            vehicle.SetLock(VehicleLockState.Locked);
            return new CmdReturn("Vous avez fermé le véhicule");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Ouvrir(Player player, object[] argv)
        {
            Vehicle vehicle = player.GetNearestVehicle(5);
            if (vehicle == null) return new CmdReturn("Veh pas trouvé");
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            vehicle.SetLock(VehicleLockState.None);
            return new CmdReturn("Vous avez ouvert le véhicule");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Plaque")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Plaque(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            string text = argv.Aggregate("", (current, t) => current + ((string)t + " "));
            vehicle.SetPlate(text);
            return new CmdReturn("Vous avez changé la plaque du véhicule");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Cle_Obtenir(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            player.Inventory.AddItem(1000, 1, vehicle.Lock.Code);
            return new CmdReturn(vehicle.Lock.Code);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Vehicule_Proprietaire(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            vehicle.SetOwner(player);
            return new CmdReturn("Vous avez changé le propriétaire du véhicule");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Info(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            Player pla = player.Game.GetPlayerByDbId(vehicle.GetOwnerId());
            Alt.Log(vehicle.Rotation + "");
            if (pla == null) return new CmdReturn("Le proprio n'est pas co @" + vehicle.GetOwnerId());
            else return new CmdReturn($"[{vehicle.GetOwnerId()}]{pla.Name} est le proprietaire du véhicule. Serrure : {vehicle.Lock.Code}");
        }

    }
}
