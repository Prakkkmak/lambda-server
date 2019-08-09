using System;
using System.Linq;
using System.Text.RegularExpressions;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class BasicCommands
    {
        [Permission("CIVIL_AIDE")]
        [Command(Command.CommandType.DEFAULT, 1)]
        [Syntax("Page")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Aide(Player player, object[] argv)
        {
            int page = (int)(uint)argv[0];
            if (!(page < Enum.GetNames(typeof(Command.CommandType)).Length)) return new CmdReturn("Page inexistante");
            Command.CommandType type = (Command.CommandType)page;
            Command[] commands = Command.GetCommands(type);
            int maxpage = Enum.GetValues(typeof(Command.CommandType)).Cast<int>().Max();
            string[] pages = new[]
            {
                "Basique",
                "Chat",
                "Inventaire",
                "Véhicule",
                "Organisation",
                "Maison",
                "Banque",
                "Skin",
                "Administration",
                "Police",
                "Gouvernement",
                "Santé",
                "Instructeur",
                "Taxi",
                "Text",
            };
            string pagename = "Non défini";
            if (pages.Length > page)
            {
                pagename = pages[page];
            }
            string text = $"Voici la liste des commandes ({pagename}) {page}/{maxpage}:<br>";
            string commandSyntaxes = "";
            foreach (Command command in commands)
            {
                if (!player.IsAllowedTo(command.Permission)) continue;
                if (command.Status == Command.CommandStatus.NEW) text += "{2980b9}";
                if (command.Status == Command.CommandStatus.TOTEST) text += "{e67e22}";
                commandSyntaxes += "/" + command.Name + ", ";
                if (command.Status != Command.CommandStatus.DEFAULT) text += "{ecf0f1}";
            }
            if (string.IsNullOrWhiteSpace(commandSyntaxes))
            {
                return new CmdReturn("Vous n'avez aucune permission dans cette partie.");
            }
            text += commandSyntaxes;
            return new CmdReturn(text);
        }
        [Permission("CIVIL_BUG")]
        [Command(Command.CommandType.DEFAULT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Feedback(Player player, object[] argv)
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
            bug.SendAsync(); // Send the bugg
            return new CmdReturn("Vous avez envoyé votre bug !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_PERSONNAGE_NOM")]
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
        [Permission("CIVIL_LISTE")]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Liste(Player player, object[] argv)
        {
            string str = "";
            str += "Il y a " + Player.Players.Count + " joueurs sur le serveur. <br>";
            str = Player.Players.Aggregate(str, (current, p) => current + $"[{p.ServerId}]{p.FirstName} {p.LastName} <br>");
            return new CmdReturn(str);
        }
        [Permission("CIVIL_POSITION")]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Position(Player player, object[] argv)
        {
            Position pos = player.Position;
            CmdReturn cmdReturn = new CmdReturn($"Votre position ( X:{pos.X} | Y:{pos.Y} | Z:{pos.Z} ) Dim : {player.Dimension}");
            return cmdReturn;
        }
        [Permission("CIVIL_ACCEPTER")]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Accepter(Player player, object[] argv)
        {
            Request request = player.GetRequest();
            if (request == null) return new CmdReturn("Vous n'avez pas de requete en attente");
            if (!request.Condition())
            {
                player.SetRequest(null);
                return CmdReturn.NotImplemented;
            }
            request.Answers[0].Action();
            player.SetRequest(null);
            return new CmdReturn("Vous avez accepté la demande !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_REFUSER")]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Refuser(Player player, object[] argv)
        {
            Request request = player.GetRequest();
            if (request == null) return new CmdReturn("Vous n'avez pas de requete en attente");
            if (!request.Condition())
            {
                player.SetRequest(null);
                return CmdReturn.NotImplemented;
            }
            request.Answers[1].Action();
            player.SetRequest(null);
            return new CmdReturn("Vous avez refusé la demande !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_CLEAR")]
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Clear(Player player, object[] argv)
        {
            return new CmdReturn("<br><br><br><br><br><br><br><br><br><br><br><br>", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_PAYDAY_INFO")]
        [Command(Command.CommandType.BANK)]
        public static CmdReturn Payday_Info(Player player, object[] argv)
        {
            return new CmdReturn("Vous avez joué " + player.TimeOnline + " depuis le dernier payday du " + player.Payday.Date);
        }

    }
}

