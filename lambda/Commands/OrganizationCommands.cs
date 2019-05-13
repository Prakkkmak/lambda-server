using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Database;
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
            Organization.AddOrganization(org);
            org.Save();
            return new CmdReturn("Vous avez créé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Organisation_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            org.Remove();
            return new CmdReturn("Vous avez supprimé une organisation !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Organisation_Liste(Player player, object[] argv)
        {
            string txt = "Voici la liste des organisations: ";
            foreach (Organization organization in Organization.Organizations)
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
            //org.AddMember(target);
            org.Save();
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
                    Player player2 = Player.GetPlayer(member.Id);
                    if (player2 != null)
                    {
                        str += $"{member.Rank.Name} - {player2.ServerId} {player2.Name} (Online)<br>";
                    }
                    else
                    {
                        //string name = DatabaseElement.Get<Player>(member.Id)["cha_name"];
                        //str += $"{member.Id} {name} <br>";
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
            Rank rank = org.AddRank((string)argv[1]);
            _ = rank.SaveAsync();
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
            _ = rank.SaveAsync();
            return new CmdReturn("Vous avez renommé un rang !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Organisation_Rang_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            org.RemoveRank(rank);
            _ = org.SaveAsync();
            return new CmdReturn("Vous avez supprimé un rang!", CmdReturn.CmdReturnType.SUCCESS);
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
