using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Organizations;

namespace Lambda.Commands
{
    class OrganizationCmds
    {
        // Set a skin in a specific slot
        // /vetement 1 0
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organization_Creer(Player player, string[] argv)
        {
            Organization org = new Organization();
            player.Game.AddOrganization(org);
            player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez créé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
