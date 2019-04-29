using Lambda.Entity;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Items;

namespace Lambda.Commands
{
    class ObjectCmds
    {
        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT, "ID du joueur", "Id du objet", "amount")]
        public static CmdReturn Donner(Player player, string[] argv)
        {
            string charName = argv[1];
            if (!uint.TryParse(argv[2], out uint objid)) return CmdReturn.InvalidParameters;
            if (!uint.TryParse(argv[3], out uint amount)) return CmdReturn.InvalidParameters;
            Player[] players = player.Game.GetPlayers(charName);
            if (player.Inventory.GetItem(objid) == null) return new CmdReturn("Vous n'avez pas assez d'objet", CmdReturn.CmdReturnType.WARNING);

            if (player.Inventory.GetItem(objid).Amount < amount) return new CmdReturn("Vous n'avez pas assez d'objet", CmdReturn.CmdReturnType.WARNING);


            CmdReturn cmdReturn = CmdReturn.OnlyOnePlayer(players);
            if (cmdReturn.Type == CmdReturn.CmdReturnType.SUCCESS)
            {

                Player p = players[0];
                if (p == player) return new CmdReturn("Vous ne pouvez pas vous ciblier vous meme", CmdReturn.CmdReturnType.WARNING);
                if (p.GetRequest() != null) return CmdReturn.RequestBusy;
                if (player.Inventory.GetItem(objid) != null)
                {
                    if (player.Inventory.GetItem(objid).Amount < amount)
                    {
                        return new CmdReturn("Vous n'avez pas assez d'objets", CmdReturn.CmdReturnType.WARNING);
                    }
                }
                Request request = new Request(p, "Don", $"{player.Name} veux vous donner un objet", player);
                request.AddAnswer("Accepter", (sender, receiver) =>
                {
                    // Blah
                    sender.SendMessage($"{receiver.Name} a accepté votre demande");

                    sender.Inventory.RemoveItem(objid, amount);
                    receiver.Inventory.AddItem(objid, amount);
                });
                request.AddAnswer("Refuser", (sender, receiver) =>
                {
                    // Blah
                    sender.SendMessage($"{receiver.Name} a refusé votre demande");

                });
                request.Condition = (sender, receiver) =>
                {
                    if (sender.Position.Distance(receiver.Position) > 3)
                    {
                        sender.SendMessage($"Vous être trop loin");
                        receiver.SendMessage($"Vous être trop loin");
                        return false;
                    }
                    return true;
                };
                p.SendRequest(request);
                return new CmdReturn("Vous avez fait une demande.", CmdReturn.CmdReturnType.SUCCESS);
            }

            return new CmdReturn("blah", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Accepter(Player player, string[] argv)
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
        public static CmdReturn Refuser(Player player, string[] argv)
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

        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Objets(Player player, string[] argv)
        {
            string str = "";
            foreach (BaseItem item in player.Game.GetBaseItems())
            {
                str += $"[{item.Id}]{item.Name} ";
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}
