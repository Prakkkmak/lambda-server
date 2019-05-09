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
    public class Chat
    {
        public void RegisterEvents()
        {
            Alt.OnClient("chatmessage", OnClientChatMessage);
        }

        public void OnClientChatMessage(IPlayer altPlayer, object[] args)
        {
            string msg = (string)args[0];
            if (string.IsNullOrWhiteSpace(msg)) return;
            if (altPlayer.GetData("player", out Player player))
            {
                if (msg[0] == '/')
                {
                    Alt.Log($"[chat:cmd] {player.Name}:{msg}");
                    InvokeCmd(player, msg);
                }
                else if (msg[0] == '!')
                {
                    Alt.Log($"[chat:cmd] {player.Name}:{msg}");
                    InvokeCmd(player, msg, false);
                }
                else
                {

                    Alt.Log($"[chat:msg] {player.Name}:{msg}");
                    SendInRange(player, 20, msg);
                }
            }
        }

        public CmdReturn InvokeCmd(Player player, string msg, bool returnable = true)
        {
            if (string.IsNullOrWhiteSpace(msg)) return default;
            string[] parameters = TextToArgs(msg);
            Command[] cmd = player.Game.GetCommands(parameters);
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
            if (returnable) player.SendMessage(cmdReturnText);
            return cmdReturn;
        }

        public string SendCmdReturn(CmdReturn cmdReturn)
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


        public void Send(Player player, string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;
            player.AltPlayer.Emit("chatmessage", null, msg);
        }

        public void SendInRange(Player player, int range, string msg, bool named = true)
        {
            foreach (Player player2 in player.Game.GetPlayers())
            {
                if (player.Position.Distance(player2.Position) < range)
                {
                    player2.AltPlayer.Emit("chatmessage", named ? player.Name : null, msg);
                }
            }

        }

        public string[] TextToArgs(string msg)
        {
            msg = msg.Replace("/", "");
            return msg.Split(" ");
        }

    }
}
