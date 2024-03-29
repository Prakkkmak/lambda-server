﻿using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Commands
{
    public class DefaultCmds
    {

        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Aide(Player player, string[] argv)
        {
            List<Command> commands = Command.Commands;
            string text = "Voici la liste des commandes : ";
            foreach (Command command in commands)
            {
                text += command.Name + " ";
            }
            return new CmdReturn(text, CmdReturn.CmdReturnType.SUCCESS);
        }

        /// List the players in game

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Liste(Player player, string[] argv)
        {
            string str = "";
            foreach (Player p in Player.OnlinePlayers)
            {
                str += $"[{p.ServerId}]{p.FirstName} {p.LastName} <br>";
            }
            CmdReturn cmdReturn = new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }

        // Print the inventory of the character in the chat

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Inventaire(Player player, string[] argv)
        {
            string str = $"Voici votre stockage";
            str +== "Voici le contenu de votre inventaire: <br>";

            foreach (Item inventoryItem in player.Inventory.Items)
            {
                str += inventoryItem.Base.Name + " " + "(" + inventoryItem.Amount + ")";
            }
            CmdReturn cmdReturn = new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }

        // Print the number of players online in the chat

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Players(Player player, string[] argv)
        {
            int nbr = Player.OnlinePlayers.Count;
            CmdReturn cmdReturn = new CmdReturn($"Il y a {nbr} joueurs sur le serveur", CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }

        //Print the pos of the character in the chat

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Pos(Player player, string[] argv)
        {
            Position pos = player.Position;
            CmdReturn cmdReturn = new CmdReturn($"Votre position ( X:{pos.X} | Y:{pos.Y} | Z:{pos.Z} )", CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }

        //Print the stats of the character in the chat
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Stats(Player player, string[] argv)
        {
            CmdReturn cmdReturn = new CmdReturn($"{player.Account.Mail} : [TODO]", CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Argent(Player player, string[] argv)
        {
            CmdReturn cmdReturn = new CmdReturn($"Vous avez {player.Inventory.Money} $]", CmdReturn.CmdReturnType.SUCCESS);
            return cmdReturn;
        }
    }
}
