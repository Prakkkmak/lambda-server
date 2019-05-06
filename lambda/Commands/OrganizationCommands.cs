using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Organizations;

namespace Lambda.Commands
{
    class OrganizationCommands
    {
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Nom")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Organisation_Creer(Player player, object[] argv)
        {
            Organization org = new Organization((string)argv[0]);
            player.Game.AddOrganization(org);
            player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez créé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Organisation_Supprimer(Player player, object[] argv)
        {
            player.Game.RemoveOrganization((Organization)argv[0]);

            return new CmdReturn("Vous avez supprimé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Liste(Player player, object[] argv)
        {
            string txt = "Voici la liste des organisations: ";
            foreach (Organization organization in player.Game.GetOrganizations())
            {
                txt += $"{organization.Id}: {organization.Name} <br>";
            }

            return new CmdReturn(txt);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Joueur")]
        [SyntaxType(typeof(Organization), typeof(Player))]
        public static CmdReturn Organisation_Ajouter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Player target = (Player)argv[1];
            org.AddMember(target);
            player.Game.DbOrganization.Save(org);
            target.SendMessage($"Vous avez été ajouté a l'organisation {org.Name}.");
            return new CmdReturn($"Vous avez ajouté {target.Name} à une organisation", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Rangs(Player player, object[] argv)
        {
            Organization[] organizations = player.GetOrganizations();
            if (organizations.Length < 1) return CmdReturn.NotInOrg;
            string str = "";
            foreach (Organization organization in organizations)
            {
                str = $"=> {organization.Name} : <br>";
                int i = 0;
                foreach (Rank rank in organization.GetRanks())
                {
                    str += $"{i++} - {rank.Name} <br>";
                }
            }

            return new CmdReturn(str);

        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Membres(Player player, object[] argv)
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
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Nom")]
        [SyntaxType(typeof(Organization), typeof(string))]
        public static CmdReturn Organisation_Rang_Creer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            if (org.GetMember(player.Id) == null) return CmdReturn.NotInOrg;
            player.Game.DbRank.Save(org.AddRank((string)argv[1]));
            return new CmdReturn("Vous avez rajouté un rang", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Nom")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(string))]
        public static CmdReturn Organisation_Rang_Renommer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            string name = (string)argv[2];
            if (rank == null) return new CmdReturn("Ce rang n existe pas", CmdReturn.CmdReturnType.WARNING);
            rank.Name = name;
            player.Game.DbRank.Save(rank);
            return new CmdReturn("Vous avez renommé un rang !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Organisation_Rang_Default(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            org.DefaultRank = rank;
            player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez changé le rang par défaut!", CmdReturn.CmdReturnType.SUCCESS);

        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Organisation_Rang_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            org.RemoveRank(rank);
            player.Game.DbOrganization.Save(org);
            return new CmdReturn("Vous avez changé le rang par défaut!", CmdReturn.CmdReturnType.SUCCESS);

        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Inviter(Player player, object[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Promouvoir(Player player, object[] argv)
        {
            return CmdReturn.NotImplemented;
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Virer(Player player, object[] argv)
        {
            return CmdReturn.NotImplemented;
        }
    }
}
