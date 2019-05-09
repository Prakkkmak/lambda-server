﻿using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using Lambda.Entity;

namespace Lambda.Commands
{
    class ChatCommands
    {
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn OOC(Player player, object[] argv)
        {
            string str = string.Join(' ', argv);
            Alt.EmitAllClients("chatmessage", player.Name, str);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Do(Player player, object[] argv)
        {
            string str = "{9b59b6}";
            str += string.Join(' ', argv);
            str += " ((" + player.Name + "))";
            player.Game.Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Me(Player player, object[] argv)
        {
            string str = "{9b59b6}";
            str += player.Name + " ";
            str += string.Join(' ', argv);
            player.Game.Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}