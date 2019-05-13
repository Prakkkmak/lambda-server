using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lambda.Entity;
using Lambda.Organizations;

namespace Lambda.Commands
{
    class AdminOrganizationCommands
    {
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Nom")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Admin_Organisation_Creer(Player player, object[] argv)
        {
            string str = argv.Aggregate("", (current, o) => current + (o + " "));
            Organization org = Organization.CreateOrganization(str);
            return new CmdReturn($"Vous avez créé l'organisation {org.Name} !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Nom")]
        [SyntaxType(typeof(Organization), typeof(string))]
        public static CmdReturn Admin_Organisation_Nom(Player player, object[] argv)
        {
            Organization organization = (Organization)argv[0];
            string str = "";
            for (int i = 1; i < argv.Length; i++)
            {
                str += argv[i] + " ";
            }
            organization.Rename(str);
            return new CmdReturn($"Vous avez renommé l'organisation en {str}!", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Supprimer(Player player, object[] argv)
        {
            Organization organization = (Organization)argv[0];
            organization.Remove();
            _ = organization.DeleteAsync();
            return new CmdReturn($"Vous avez supprimé l'organisation {organization.Name}!", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Nom")]
        [SyntaxType(typeof(Organization), typeof(string))]
        public static CmdReturn Admin_Organisation_Rang_Creer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = org.AddRank((string)argv[1]);
            return new CmdReturn($"Vous avez créé le rang {rank.Name}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Admin_Organisation_Rang_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            org.RemoveRank(rank);
            return new CmdReturn($"Vous avez supprimé le rang {rank.Name}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Nom")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(string))]
        public static CmdReturn Admin_Organisation_Rang_Renommer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            string str = "";
            for (int i = 2; i < argv.Length; i++)
            {
                str += argv[i] + " ";
            }
            rank.Rename(str);
            return new CmdReturn($"Vous avez renommé le rang en {rank.Name}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION)]
        public static CmdReturn Admin_Organisation_Liste(Player player, object[] argv)
        {
            string str = "Organisations existantes: <br>";
            foreach (Organization organization in Organization.Organizations)
            {
                str += organization.Name + "<br>";
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Membres(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string str = "Membres de l'organisation: <br>";
            foreach (Member member in org.GetMembers())
            {
                Player p = Player.GetPlayer(member.Id);
                if (p != null)
                {
                    str += $"[{p.ServerId}] {member.Name}<br>";
                }
                else
                {
                    str += $"{member.Name}";
                }
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Rangs(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string str = "Membres de l'organisation: <br>";
            foreach (Rank rank in org.GetRanks())
            {
                str += rank.Name + "<br>";
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Joueur")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(Player))]
        public static CmdReturn Admin_Organisation_Ajouter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            Player target = (Player)argv[2];
            org.AddMember(target, rank);
            return new CmdReturn($"Vous avez ajouté {target.Name} à l'organisation {org.Name} au rang de {rank.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
