using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
using Lambda.Entity;

namespace Lambda.Commands
{
    class ChatCmds
    {
        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT, "texte")]
        public static CmdReturn OOC(Player player, string[] argv)
        {
            argv[0] = "";
            string str = string.Join(' ', argv);
            Alt.EmitAllClients("chatmessage", player.Name, str);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }

        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT, "texte")]
        public static CmdReturn Desc(Player player, string[] argv)
        {
            argv[0] = "";
            string str = "{9b59b6}";
            str += string.Join(' ', argv);
            str += "((" + player.Name + "))";
            player.Game.Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT, "texte")]
        public static CmdReturn Me(Player player, string[] argv)
        {
            argv[0] = "";
            string str = "{9b59b6}";
            str += player.Name;
            str += " " + string.Join(' ', argv);
            player.Game.Chat.SendInRange(player, 20, str, false);
            return new CmdReturn("", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
