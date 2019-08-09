using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class ChatCommands
    {
        [Permission("CIVIL_CHAT_OOC")]
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn OOC(Player player, object[] argv)
        {
            string str = string.Join(' ', argv);
            Alt.EmitAllClients("chatmessage", player.FullName, "((" + str + "))");
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_CHAT_DO")]
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Do(Player player, object[] argv)
        {
            string str = "{9b59b6}";
            str += string.Join(' ', argv);
            str += " ((" + player.FullName + "))";
            Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("CIVIL_CHAT_ME")]
        [Command(Command.CommandType.CHAT, 1)]
        [Syntax("Texte")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Me(Player player, object[] argv)
        {
            string str = "{9b59b6}";
            str += player.FullName + " ";
            str += string.Join(' ', argv);
            Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
