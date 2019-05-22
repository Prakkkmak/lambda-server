﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class BasicCommands
    {

        [Command(Command.CommandType.DEFAULT, 1)]
        [Syntax("Page")]
        [SyntaxType(typeof(int))]
        public static CmdReturn Aide(Player player, object[] argv)
        {
            int page = (int)argv[0];
            if (!(page < Enum.GetNames(typeof(Command.CommandType)).Length)) return new CmdReturn("Page inexistante");
            Command.CommandType type = (Command.CommandType)argv[0];
            Command[] commands = Command.GetCommands(type);
            int maxpage = Enum.GetValues(typeof(Command.CommandType)).Cast<int>().Max();
            string[] pages = new[]
            {
                "Basique",
                "Chat",
                "Inventaire",
                "Véhicule",
                "Organisation",
                "Zone",
                "Magasin",
                "Maison",
                "Banque",
                "Skin",
                "Admin",
                "Test"
            };
            string pagename = "Non défini";
            if (pages.Length > (int)argv[0])
            {
                pagename = pages[(int)argv[0]];
            }
            string text = $"Voici la liste des commandes ({pagename}) {(int)argv[0]}/{maxpage}:<br>";
            foreach (Command command in commands)
            {
                if (command.Status == Command.CommandStatus.NEW) text += "{2980b9}";
                if (command.Status == Command.CommandStatus.TOTEST) text += "{e67e22}";
                text += "/" + command.Name + ", ";
                if (command.Status != Command.CommandStatus.DEFAULT) text += "{ecf0f1}";
            }
            return new CmdReturn(text);
        }

        [Command(Command.CommandType.DEFAULT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Bug(Player player, object[] argv)
        {
            int minSize = 10;
            string text = "";
            string sender;
            if (player.Account != null)
            {
                sender = player.Account.Mail + " - " + player.FullName; //used for have the mail and the character name of the sender
            }
            else
            {
                sender = player.FullName;
            }
            for (int i = 0; i < argv.Length; i++)
            {
                text += (string)argv[i] + " ";
            }
            if (text.Length < minSize) return new CmdReturn("Votre bug doit être plus long ! ", CmdReturn.CmdReturnType.SYNTAX); // The minimum length of the report bugg
            Bug bug = new Bug(text, sender); // Create the bugg object
            bug.Send(); // Send the bugg
            return new CmdReturn("Vous avez envoyé votre bug !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT, 2)]
        [Syntax("Prenom", "nom")]
        [SyntaxType(typeof(string), typeof(string))]
        public static CmdReturn Personnage_Nom(Player player, object[] argv)
        {
            string firstName = (string)argv[0];
            string lastName = (string)argv[1];
            Regex regex = new Regex("[A-Z][a-z]+");
            if (!regex.Match(firstName).Success) return new CmdReturn("Votre prénom doit être de la forme : Babar");
            if (!regex.Match(lastName).Success) return new CmdReturn("Votre nom doit être de la forme : Jocombe");
            player.FirstName = firstName;
            player.LastName = lastName;
            return new CmdReturn("Vous avez changé de nom !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Liste(Player player, object[] argv)
        {
            string str = "";
            str += "Il y a " + Player.Players.Count + " joueurs sur le serveur. <br>";
            str = Player.Players.Aggregate(str, (current, p) => current + $"[{p.ServerId}]{p.FirstName} {p.LastName} <br>");
            return new CmdReturn(str);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Position(Player player, object[] argv)
        {
            Position pos = player.Position;
            CmdReturn cmdReturn = new CmdReturn($"Votre position ( X:{pos.X} | Y:{pos.Y} | Z:{pos.Z} ) Dim : {player.Dimension}");
            return cmdReturn;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Accepter(Player player, object[] argv)
        {
            Request request = player.GetRequest();
            if (request == null) return new CmdReturn("Vous n'avez pas de requete en attente");
            if (!request.Condition(request.Sender, player))
            {
                player.SetRequest(null);
                return CmdReturn.NotImplemented;
            }
            request.Answers[0].Action(request.Sender, player);
            player.SetRequest(null);
            return new CmdReturn("Vous avez accepté la demande !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Refuser(Player player, object[] argv)
        {
            Request request = player.GetRequest();
            if (request == null) return new CmdReturn("Vous n'avez pas de requete en attente");
            if (!request.Condition(request.Sender, player))
            {
                player.SetRequest(null);
                return CmdReturn.NotImplemented;
            }
            request.Answers[1].Action(request.Sender, player);
            player.SetRequest(null);
            return new CmdReturn("Vous avez refusé la demande !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn TP(Player player, object[] argv)
        {
            Location location = Area.GetDestination(player.Position, player.Dimension);
            //if (area == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            if (player.Vehicle != null) return new CmdReturn("");
            if (location.Equals(default(Location))) return new CmdReturn("");
            if (location.Position.Equals(new Position())) return new CmdReturn("Il n'y a pas d'interieur");
            player.GotoLocation(location);
            return new CmdReturn("Vous vous êtes téléporté", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Clear(Player player, object[] argv)
        {
            return new CmdReturn("<br><br><br><br><br><br><br><br><br><br><br><br>", CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}

