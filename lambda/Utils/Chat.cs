using System;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Entity;
using Player = Lambda.Entity.Player;

namespace Lambda.Utils
{
    public static class Chat
    {
        public static void RegisterEvents()
        {
            Alt.OnClient("chatmessage", OnClientChatMessage);
        }

        public static void OnClientChatMessage(IPlayer altPlayer, object[] args)
        {
            string msg = (string)args[0];
            Alt.Log(msg);
            if (string.IsNullOrWhiteSpace(msg)) return;
            Player player = (Player)altPlayer;
            if (player != null)
            {
                if (msg[0] == '/')
                {
                    Alt.Log($"[chat:cmd] {player.FullName}:{msg}");
                    InvokeCmd(player, msg);
                }
                else if (msg[0] == '!')
                {
                    Alt.Log($"[chat:cmd] {player.FullName}:{msg}");
                    InvokeCmd(player, msg, false);
                }
                else
                {

                    Alt.Log($"[chat:msg] {player.FullName}:{msg}");
                    SendInRange(player, 20, msg);
                }
            }
        }

        public static CmdReturn InvokeCmd(Player player, string msg, bool returnable = true)
        {
            if (string.IsNullOrWhiteSpace(msg)) return default;
            string[] parameters = TextToArgs(msg);
            Command[] cmd = Command.GetCommands(parameters);
            CmdReturn cmdReturn = new CmdReturn();
            string commandstring = "";
            foreach (Command command in cmd)
            {
                commandstring += command.Syntax() + "<br>";
            }
            if (cmd.Length > 1) cmdReturn = new CmdReturn("Plusieurs commandes ont le même nom. <br> " + commandstring, CmdReturn.CmdReturnType.SYNTAX);
            if (cmd.Length < 1) cmdReturn = new CmdReturn("Commande introuvable.", CmdReturn.CmdReturnType.SYNTAX);
            if (cmd.Length == 1) cmdReturn = cmd[0].Execute(player, parameters);
            string cmdReturnText = string.Copy(SendCmdReturn(cmdReturn));
            if (returnable && !string.IsNullOrWhiteSpace(cmdReturnText)) player.SendMessage(cmdReturnText);
            return cmdReturn;
        }

        public static string SendCmdReturn(CmdReturn cmdReturn)
        {
            switch (cmdReturn.Type)
            {
                case CmdReturn.CmdReturnType.WARNING:
                    return "{e67e22}" + cmdReturn.Text;
                case CmdReturn.CmdReturnType.SUCCESS:
                    return "{2ecc71}" + cmdReturn.Text;
                case CmdReturn.CmdReturnType.ERROR:
                    return "{c0392b}" + cmdReturn.Text;
                case CmdReturn.CmdReturnType.SYNTAX:
                    return "{f1c40f}" + cmdReturn.Text;
                case CmdReturn.CmdReturnType.LOCATION:
                    return "{e67e22}" + cmdReturn.Text;
                case CmdReturn.CmdReturnType.DEFAULT:
                    return cmdReturn.Text;
                case CmdReturn.CmdReturnType.NOTIMPLEMENTED:
                    return cmdReturn.Text;
                default:
                    return cmdReturn.Text;

            }
        }


        public static void Send(Player player, string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;
            player.Emit("chatmessage", null, msg);
        }

        public static void SendInRange(Player player, int range, string msg, bool named = true)
        {
            foreach (Player player2 in Player.Players)
            {

                if (player.Position.Distance(player2.Position) < range || range == 0)
                {
                    player2.Emit("chatmessage", named ? player.FullName : null, msg);
                }
            }

        }

        public static string[] TextToArgs(string msg)
        {
            msg = msg.Replace("/", "");
            return msg.Split(" ");
        }

    }
}
