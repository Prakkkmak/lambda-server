using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Organizations;

namespace Lambda.Commands
{
    class OrganizationCmds
    {
        [Command(Command.CommandType.DEFAULT, "Nom")]
        public static CmdReturn AOrganisation_Creer(Player player, string[] argv)
        {
            Organization org = new Organization(argv[2]);
            player.Game.AddOrganization(org);
            player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez créé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT, "Nom")]
        public static CmdReturn AOrganisation_Supprimer(Player player, string[] argv)
        {
            if (!uint.TryParse(argv[2], out uint orgId)) return new CmdReturn("Tamere la pute je suis enervé sam arch pas", CmdReturn.CmdReturnType.WARNING);
            Organization org = player.Game.GetOrganization(orgId);
            player.Game.RemoveOrganization(org);
            //player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez supprimé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Liste(Player player, string[] argv)
        {
            string txt = "";
            foreach (Organization organization in player.Game.GetOrganizations())
            {
                txt += $"{organization.Id}: {organization.Name} <br>";
            }
            return new CmdReturn(txt, CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT, "Organisation", "Joueur")]
        public static CmdReturn AOrganisation_Ajouter(Player player, string[] argv)
        {
            if (!uint.TryParse(argv[2], out uint orgId)) return new CmdReturn("Tamere la pute je suis enervé sam arch pas", CmdReturn.CmdReturnType.WARNING);
            Organization org = player.Game.GetOrganization(orgId);
            string charName = argv[3];
            Player[] players = player.Game.GetPlayers(charName);
            CmdReturn cmdReturn = CmdReturn.OnlyOnePlayer(players);
            if (cmdReturn.Type != CmdReturn.CmdReturnType.SUCCESS) return cmdReturn;
            org.AddMember(player);
            player.Game.DbOrganization.Save(org);
            return new CmdReturn($"Vous avez ajouté {player.Name} à une organisation", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Rangs(Player player, string[] argv)
        {
            Organization[] organizations = player.GetOrganizations();
            if (organizations.Length < 1) return CmdReturn.NotInOrg;
            string str = "";
            foreach (Organization organization in organizations)
            {
                str = $"=> {organization.Name} : <br>";
                foreach (Rank rank in organization.GetRanks())
                {
                    str += $"{rank.Name} <br>";
                }
            }
            return new CmdReturn(str);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Membres(Player player, string[] argv)
        {
            Organization[] organizations = player.GetOrganizations();
            if (organizations.Length < 1) return CmdReturn.NotInOrg;

            string str = "";
            foreach (Organization organization in organizations)
            {
                str = $"=> {organization.Name} : <br>";
                foreach (Member member in organization.GetMembers())
                {
                    Player[] players = player.Game.GetPlayers(member.Id.ToString());
                    if (players.Length > 0)
                    {
                        str += $"{member.Id} {players[0].Name} (Online)<br>";
                    }
                    else
                    {
                        string name = player.Game.DbPlayer.Get(member.Id).Name;
                        str += $"{member.Id} {name} <br>";
                    }
                }
            }
            return new CmdReturn(str);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Rang_Creer(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Rang_Renommer(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Rang_Default(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Rang_Supprimer(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Inviter(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Proumevoir(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Organisation_Virer(Player player, string[] argv)
        {
            return CmdReturn.NotImplemented;
        }

    }


}
}
