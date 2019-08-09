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
        [Permission("ADMINISTRATEUR_VEHICULE_CREER")]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Modele")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Creer(Player player, object[] argv)
        {
            if (!Enum.TryParse((string)argv[0], true, out VehicleModel model))
            {
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            }
            if (!Enum.IsDefined(typeof(VehicleModel), model)) return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            Position pos = PositionHelper.PositionInAngle(player.Position, player.Rotation, 2);
            Rotation rot = player.Rotation;
            rot.Yaw = -rot.Yaw;
            rot.Yaw -= (float)Math.PI / 2;
            while (rot.Yaw < -(float)Math.PI) rot.Yaw += 2 * (float)Math.PI;
            while (rot.Yaw > (float)Math.PI) rot.Yaw -= 2 * (float)Math.PI;
            if (rot.Yaw > 0) rot.Yaw -= (float)Math.PI;
            else if (rot.Yaw <= 0) rot.Yaw += (float)Math.PI;
            //rot.Yaw += (float)Math.PI;
            Vehicle vehicle = (Vehicle)Alt.CreateVehicle(model, pos, rot);
            Vehicle.AddVehicle(vehicle);
            //vehicle.Spawn();
            player.Inventory.AddItem(Enums.Items.CarKey, 1, vehicle.Lock.Code);
            return new CmdReturn("Vous avez fait apparaitre un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_VEHICULE_PARK")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Park(Player player, object[] argv)
        {
            Vehicle veh = (Vehicle)player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.Park();
            return new CmdReturn("Vous avez sauvegardé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_VEHICULE_GARER")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Garer(Player player, object[] argv)
        {
            Vehicle veh = (Vehicle)player.Vehicle;
            if (veh.OwnerId != player.Id) return new CmdReturn("Ce véhicule ne vous appartient pas.", CmdReturn.CmdReturnType.WARNING);
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.Park();
            return new CmdReturn("Vous avez garé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_VEHICULE_COULEUR")]
        [Command(Command.CommandType.VEHICLE, 3)]
        [Syntax("R", "G", "B")]
        [SyntaxType(typeof(byte), typeof(byte), typeof(byte))]
        public static CmdReturn Vehicule_Couleur(Player player, object[] argv)
        {
            Vehicle veh = (Vehicle)player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.PrimaryColorRgb = new Rgba((byte)argv[0], (byte)argv[1], (byte)argv[2], 255);
            return new CmdReturn("Vous avez changé la couleur de votre véhicule ! ", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_VEHICULE_COULEUR2")]
        [Command(Command.CommandType.VEHICLE, 3)]
        [Syntax("R", "G", "B")]
        [SyntaxType(typeof(byte), typeof(byte), typeof(byte))]
        public static CmdReturn Vehicule_Couleur_Secondaire(Player player, object[] argv)
        {
            Vehicle veh = (Vehicle)player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.SecondaryColorRgb = new Rgba((byte)argv[0], (byte)argv[1], (byte)argv[2], 255);
            return new CmdReturn("Vous avez changé la couleur de votre véhicule ! ", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_VEHICULE_SUPPRIMER")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Supprimer(Player player, object[] argv)
        {
            Vehicle veh = (Vehicle)player.Vehicle;
            if (veh == null) return CmdReturn.NotInVehicle;
            veh.Remove();
            return new CmdReturn("Vous avez supprimé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_VEHICULE_CLEF")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Clef(Player player, object[] argv)
        {
            Vehicle vehicle = (Vehicle)player.Vehicle;
            Alt.Log("");
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            if (vehicle.EngineOn)
            {
                vehicle.SetEngine(false);
                return new CmdReturn("Vous avez eteint le moteur");
            }
            else
            {
                vehicle.SetEngine(true);
                return new CmdReturn("Vous avez allumé le moteur");
            }


        }
        [Permission("CIVIL_VEHICULE_FERMER")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Fermer(Player player, object[] argv)
        {
            Vehicle vehicle = player.GetNearestVehicle(5);
            if (vehicle == null) return new CmdReturn("Veh pas trouvé");
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            vehicle.LockState = VehicleLockState.Locked;
            return new CmdReturn("Vous avez fermé le véhicule");
        }

        [Permission("CIVIL_VEHICULE_OUVRIR")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Ouvrir(Player player, object[] argv)
        {
            Vehicle vehicle = player.GetNearestVehicle(5);
            if (vehicle == null) return new CmdReturn("Veh pas trouvé");
            if (!player.HaveKeyOf(vehicle.Lock.Code)) return new CmdReturn("Vous n'avez pas les clés", CmdReturn.CmdReturnType.WARNING);
            vehicle.LockState = VehicleLockState.None;
            return new CmdReturn("Vous avez ouvert le véhicule");
        }
        [Permission("ADMINISTRATEUR_VEHICULE_PLAQUE")]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Plaque")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Plaque(Player player, object[] argv)
        {
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            string text = argv.Aggregate("", (current, t) => current + ((string)t + " "));
            vehicle.NumberplateText = text;
            return new CmdReturn("Vous avez changé la plaque du véhicule");
        }
        [Permission("ADMINISTRATEUR_VEHICULE_CLEF_OBTENIR")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Obtenir_Clef(Player player, object[] argv)
        {
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            player.Inventory.AddItem(Enums.Items.CarKey, 1, vehicle.Lock.Code);
            return new CmdReturn(vehicle.Lock.Code);
        }
        [Permission("ADMINISTRATEUR_VEHICULE_PROPRIETAIRE_MODIFIER")]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Vehicule_Proprietaire(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            vehicle.SetOwner(target);
            return new CmdReturn("Vous avez changé le propriétaire du véhicule.");
        }
        [Permission("MODERATEUR_VEHICULE_INFO")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Info(Player player, object[] argv)
        {
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            Player pla = Player.GetPlayerByDbId(vehicle.OwnerId);
            Alt.Log(vehicle.Rotation + "");
            if (pla == null) return new CmdReturn("Le proprio n'est pas co @" + vehicle.OwnerId);
            else return new CmdReturn($"[{vehicle.OwnerId}]{pla.FullName} est le proprietaire du véhicule. Serrure : {vehicle.Lock.Code}");
        }

        [Permission("ADMINISTRATEUR_VEHICULE_VENDRE")]
        [Command(Command.CommandType.VEHICLE, 1)]
        [Syntax("Prix")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Vehicule_Concession(Player player, object[] argv)
        {
            uint price = (uint)argv[0];
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            vehicle.SellPrice = price;
            return new CmdReturn("Vous avez mis en vente le véhicule à " + price + "$.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_VEHICULE_BUY")]
        [Command(Command.CommandType.VEHICLE)]
        public static CmdReturn Vehicule_Acheter(Player player, object[] argv)
        {
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (vehicle.SellPrice < 1) return new CmdReturn("Ce véhicule n'est pas en vente");
            if (player.Inventory.Money < vehicle.SellPrice) return CmdReturn.NoEnoughMoney;
            player.Inventory.Money -= vehicle.SellPrice;
            vehicle.SellPrice = 0;
            vehicle.SetOwner(player);
            vehicle.SetOwnerType(Vehicle.OwnerType.CHARACTER);
            return new CmdReturn("Vous avez acheté un véhicle !.", CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}
