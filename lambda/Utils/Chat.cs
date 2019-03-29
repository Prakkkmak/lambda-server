using System;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Entity;
using Player = Lambda.Entity.Player;

namespace Lambda.Utils
{
    public class Chat
    {
        public static void RegisterEvents()
        {
            Alt.OnClient("chatmessage", OnClientChatMessage);
        }

        public static void OnClientChatMessage(IPlayer altPlayer, object[] args)
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
                else
                {
                    Alt.Log($"[chat:msg] {player.Name}:{msg}");
                    Console.WriteLine($"[chat:msg CONSOLE] {player.Name}:{msg}");
                    player.SendMessage("Test de la lettre => Ã© Ã¯ Ã¨");
                    player.SendMessage($"{player.Name} : {msg}");
                }
            }
            else
            {
                //throw new Exception("Can't access to <Account> of player");
            }
        }

        public static CmdReturn InvokeCmd(Player player, string msg)
        {
            string[] parameters = TextToArgs(msg);
            Command[] cmd = Command.GetCommands(parameters);
            CmdReturn cmdReturn = new CmdReturn();
            string commandstring = "";
            foreach (Command command in cmd)
            {
                commandstring += command.Syntax().Text + "<br>";
            }
            if (cmd.Length > 1) cmdReturn = new CmdReturn("Plusieurs commandes ont le même nom. <br> " + commandstring, CmdReturn.CmdReturnType.SYNTAX);
            if (cmd.Length < 1) cmdReturn = new CmdReturn("Commande introuvable.", CmdReturn.CmdReturnType.SYNTAX);
            if (cmd.Length == 1) cmdReturn = cmd[0].Execute(player, parameters);
            string cmdReturnText = string.Copy(SendCmdReturn(cmdReturn));
            player.SendMessage(cmdReturnText);
            return cmdReturn;
        }

        public static string SendCmdReturn(CmdReturn cmdReturn)
        {
            Alt.Log("Cmd return du send cmd return " + cmdReturn.Text);
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
            msg = msg.Replace("é", "Ã©");
            msg = msg.Replace("è", "Ã¨");
            msg = msg.Replace("à", "Ã");
            msg = msg.Replace("ê", "Ãª");
            msg = msg.Replace("ç", "Ã§"); ;
            player.AltPlayer.Emit("chatmessage", null, msg);
        }

        public static string[] TextToArgs(string msg)
        {
            msg = msg.Replace("/", "");
            return msg.Split(" ");
        }

    }
}
