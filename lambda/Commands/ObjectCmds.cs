using Lambda.Entity;
using Lambda.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class ObjectCmds
    {
        /// Print availible commands to the chat
        [Command(Command.CommandType.DEFAULT, "ID de l'objet")]
        public static CmdReturn Donner(Player player, string[] argv)
        {
            string charName = argv[1];
            Player[] players = player.Game.GetPlayers(charName);
            CmdReturn cmdReturn = CmdReturn.OnlyOnePlayer(players);
            if (cmdReturn.Type == CmdReturn.CmdReturnType.SUCCESS)
            {
                
                Player p = players[0];
                Request request = new Request(p, "Don", $"{player.Name} veux vous donner un objet", player);
                request.AddAnswer("Accepter", (sender, receiver) =>
                {
                    // Blah
                    sender.SendMessage("[TEST] Il a accepté");
                    receiver.SendMessage("[TEXT] Vous avez accepté");
                });
                    
               p.AddRequest(request);

                return new CmdReturn("Vous vous freeze un joueur.", CmdReturn.CmdReturnType.SUCCESS);
            }

            return new CmdReturn("blah", CmdReturn.CmdReturnType.SUCCESS);
        }

    }
}
