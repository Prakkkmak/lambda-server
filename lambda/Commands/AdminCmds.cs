using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Items;
using Lambda.Administration;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Player = Lambda.Entity.Player;
using Vector3 = Lambda.Utils.Vector3;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda.Commands
{
    class AdminCmds
    {
        /// Create a checkpoint at the character position.
        /// /checkpoint creer 1 5
        [Command(Command.CommandType.ADMIN, "type", "rayon")]
        public static CmdReturn Checkpoint_Creer(Player player, string[] argv)
        {
            if (!int.TryParse(argv[2], out int type)) return new CmdReturn("Le type doit être un nombre !", CmdReturn.CmdReturnType.SYNTAX);
            if (!int.TryParse(argv[3], out int radius)) return new CmdReturn("Le rayon doit être un nombre !", CmdReturn.CmdReturnType.SYNTAX);
            Area area = new Area(type, player.Position, radius, 1, Rgba.Zero); // Create the area
            Area.Areas.Add(area); // Add the area to the list of areas
            return new CmdReturn("Vous avez créé un checkpoint !", CmdReturn.CmdReturnType.SUCCESS);
        }
        /// Give an item to the character
        /// /give 0 5
        [Command(Command.CommandType.ADMIN, "id", "nombre")]
        public static CmdReturn Give(Player player, string[] argv)
        {
            if (!uint.TryParse(argv[1], out uint id))
            {
                return new CmdReturn("Veuillez entrer un id valide", CmdReturn.CmdReturnType.SYNTAX);
            }

            if (!uint.TryParse(argv[1], out uint amount))
            {
                return new CmdReturn("Veuillez entrer un nombre valide", CmdReturn.CmdReturnType.SYNTAX);
            }
            if (BaseItem.GetBaseItem(id) == null) return new CmdReturn("Cet objet n'existe pas", CmdReturn.CmdReturnType.WARNING);
            player.Inventory.AddItem(id, amount);
            return new CmdReturn("Vous vous êtes donné des objets", CmdReturn.CmdReturnType.SUCCESS);
        }

        /// Give set of weapons to the  character

        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Armes(Player player, string[] argv)
        {
            player.AltPlayer.Emit("giveAllWeapons");
            return new CmdReturn("Vous vous êtes donnés des armes", CmdReturn.CmdReturnType.SUCCESS);
        }
        /// Teleport the player who perform the command to another character
        /// /goto John_Smith
        [Command(Command.CommandType.ADMIN, "joueur")]
        public static CmdReturn Goto(Player player, string[] argv)
        {
            if (argv.Length < 2) throw new ArgumentException("Cmd Goto need 1 params", nameof(argv));
            string charName = argv[1];
            Player[] players = Player.GetPlayers(charName);
            CmdReturn cmdReturn = CmdReturn.OnlyOnePlayer(players);
            return cmdReturn.Type == CmdReturn.CmdReturnType.SUCCESS ? new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS) : cmdReturn;
        }
        /// Respawn the character
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Respawn(Player player, string[] argv)
        {
            if (argv.Length < 1) throw new ArgumentException("Cmd Goto need 1 params", nameof(argv));
            player.Spawn(Player.SpawnPosition);
            player.Hp = 100;
            return new CmdReturn("Vous vous avez respawn!", CmdReturn.CmdReturnType.SUCCESS);
        }

        /// Spawn a veh with the specified model
        /// /vehicule Sultan

        [Command(Command.CommandType.ADMIN, "model")]
        public static CmdReturn Vehicule(Player player, string[] argv)
        {
            if (!Enum.TryParse(argv[1], true, out VehicleModel model) && Enum.IsDefined(typeof(VehicleModel), argv[1])) // If the model is not the name
            {
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            Vehicle vehicle = new Vehicle(Vector3.Near(player.Position), model);
            Vehicle.Vehicles.Add(vehicle);
            return new CmdReturn("Vous avez fait apparaitre un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

        /// Save the spawn point of the current vehicle

        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Vehicule_Park(Player player, string[] argv)
        {

            if (player.AltPlayer != null)
            {

                if (player.AltPlayer.IsInVehicle)
                {

                    player.AltPlayer.Vehicle.GetData("AltVehicle", out Vehicle vehicle);
                    if (vehicle != null)
                    {

                        vehicle.Park();
                    }
                    else
                    {

                        return CmdReturn.NotExceptedError;
                    }
                }
                else
                {
                    Alt.Log(CmdReturn.NotInVehicle.Text);
                    Alt.Log(CmdReturn.NotInVehicle + "");
                    return CmdReturn.NotInVehicle;
                }
            }
            else
            {
                return CmdReturn.NoPlayerAttached;
            }
            return new CmdReturn("Vous avez sauvegardé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

        /// Repair the current vehicle

        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Vehicule_Reparer(Player player, string[] argv)
        {
            IVehicle AltVehicle = player.AltPlayer.Vehicle;

            if (AltVehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);

            AltVehicle.GetData("vehicle", out Vehicle veh);

            if (veh != null)
            {
                veh.Repair();
                return new CmdReturn("Vous avez réparé votre véhicule ! ", CmdReturn.CmdReturnType.SUCCESS);
            }
            else
            {
                return CmdReturn.NotInVehicle;
            }
        }

        /// Delete the current vehicule from the database and from the game

        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Vehicule_Supprimer(Player player, string[] argv)
        {
            Alt.Log("Commande Vehicule Park");
            if (player.AltPlayer != null)
            {
                Alt.Log("Le joueur existe");
                if (player.AltPlayer.IsInVehicle)
                {
                    Alt.Log("Le joueur est dans un véhicule");
                    player.AltPlayer.Vehicle.GetData("AltVehicle", out Vehicle vehicle);
                    if (vehicle != null)
                    {
                        Alt.Log("Delete du veh");
                        vehicle.Delete();
                        return new CmdReturn("Vous avez supprimé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
                    }
                    else
                    {

                        return CmdReturn.NotExceptedError;
                    }
                }
                else
                {
                    Alt.Log(CmdReturn.NotInVehicle.Text);
                    Alt.Log(CmdReturn.NotInVehicle + "");
                    return CmdReturn.NotInVehicle;
                }
            }
            else
            {
                return CmdReturn.NoPlayerAttached;
            }
            return new CmdReturn("Vous avez sauvegardé un véhicule", CmdReturn.CmdReturnType.SUCCESS);
        }

        //todo
        /*
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Vetement_Sauvegarder_Bon(Player player, string[] argv)
        {
            player.Skin.Valid = true;
            player.Skin.Insert();
            return new CmdReturn("Vous avez sauvegardé un bon skin", CmdReturn.CmdReturnType.SUCCESS);
        }

        //todo

        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Vetement_Sauvegarder_Mauvais(Player player, string[] argv)
        {
            player.Skin.Valid = false;
            player.Skin.Insert();
            return new CmdReturn("Vous avez sauvegardé un mauvais skin", CmdReturn.CmdReturnType.SUCCESS);
        }*/
    }
}
