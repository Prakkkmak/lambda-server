using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
using Lambda.Administration;
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
            str = str.Substring(0, str.Length - 1);
            if (Organization.Organizations.Find((o) => o.Name.Equals(str)) != null)
            {
                return new CmdReturn($"Cette organisation existe déjà !", CmdReturn.CmdReturnType.WARNING);
            }
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
            str = str.Substring(0, str.Length - 1);
            if (Organization.Organizations.Find((o) => o.Name.Equals(str)) != null)
            {
                return new CmdReturn($"Cette organisation existe déjà !", CmdReturn.CmdReturnType.WARNING);
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
            string str = "";
            for (int i = 1; i < argv.Length; i++)
            {
                str += argv[i] + " ";
            }
            str = str.Substring(0, str.Length - 1);
            if (org.GetRanks().ToList().Find((o) => o.Name.Equals(str)) != null)
            {
                return new CmdReturn($"Ce rang existe déjà !", CmdReturn.CmdReturnType.WARNING);
            }
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
            foreach (Member member in org.GetMembers())
            {
                if (member.Rank == rank) return new CmdReturn($"{member.Name} occupe ce rang, veuillez le changer de rang", CmdReturn.CmdReturnType.WARNING);
            }
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
            str = str.Substring(0, str.Length - 1);
            if (org.GetRanks().ToList().Find((o) => o.Name.Equals(str)) != null)
            {
                return new CmdReturn($"Ce rang existe déjà !", CmdReturn.CmdReturnType.WARNING);
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
                str += organization.Id + "-" + organization.Name + "<br>";
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Membres_Liste(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string str = "Membres de l'organisation: <br>";
            foreach (Member member in org.GetMembers())
            {
                Player p = Player.GetPlayer(member.PlayerId);
                if (p != null)
                {
                    str += $"[{p.ServerId}] {p.FullName} ({member.Rank.Name})<br>";
                }
                else
                {
                    str += $"[Offline]{member.PlayerId} ({member.Rank.Name})<br>";
                }
            }
            return new CmdReturn(str, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Rangs_Liste(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string str = "Rangs de l'organisation: <br>";
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
            if (org.GetMember(target.Id) != null) return new CmdReturn($"{target.FullName} est déjà d'ans l'organisation", CmdReturn.CmdReturnType.WARNING);
            org.AddMember(target, rank);
            target.SendMessage("Vous avez été invité à l'organisation " + org.Name + " au rang de " + rank.Name + ".");
            return new CmdReturn($"Vous avez ajouté {target.FullName} à l'organisation {org.Name} au rang de {rank.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 4)]
        [Syntax("Organisation", "Membre", "Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Member), typeof(Organization), typeof(Rank))]
        public static CmdReturn Admin_Organisation_Rang_Nom(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Member member = (Member)argv[1];
            if ((Organization)argv[2] != org) return new CmdReturn("Organisations différentes", CmdReturn.CmdReturnType.WARNING);
            Rank rank = (Rank)argv[3];
            org.ChangeMemberRank(member, rank);
            return new CmdReturn($"Vous avez changé le rang de {member.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Salaire")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(uint))]
        public static CmdReturn Admin_Organisation_Salaire(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            uint salary = (uint)argv[2];
            rank.ChangeSalary(salary);
            return new CmdReturn($"Vous avez changé le salaire de {rank.Name} à {salary}$.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Permissions))]
        public static CmdReturn Admin_Organisation_Permission_Ajouter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string perm = (string)argv[1];
            org.Permissions.Add(perm);
            _ = org.SaveAsync(); //TODO deplacer le save
            return new CmdReturn($"Vous avez ajouté la permission {perm} à {org.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Admin_Organisation_Permission_Voir(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            return new CmdReturn($"{org.Permissions.ToString()}", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Permissions))]
        public static CmdReturn Admin_Organisation_Permission_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string perm = (string)argv[1];
            org.Permissions.Remove(perm);
            _ = org.SaveAsync(); //TODO deplacer le save
            return new CmdReturn($"Vous avez supprimé la permission {perm} à {org.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(Permissions))]
        public static CmdReturn Admin_Organisation_Permission_Rang_Ajouter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            string perm = (string)argv[2];
            rank.Permissions.Add(perm);
            _ = rank.SaveAsync(); //TODO deplacer le save
            return new CmdReturn($"Vous avez ajouté la permission {perm} à {rank.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(Permissions))]
        public static CmdReturn Admin_Organisation_Permission_Rang_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            string perm = (string)argv[2];
            rank.Permissions.Remove(perm);
            _ = rank.SaveAsync(); //TODO deplacer le save
            return new CmdReturn($"Vous avez supprimé la permission {perm} à {rank.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Membre")]
        [SyntaxType(typeof(Organization), typeof(Member))]
        public static CmdReturn Admin_Organisation_Retirer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Member member = (Member)argv[1];
            org.RemoveMember(member);
            return new CmdReturn($"Vous avez viré quelqu'un d'une organisation.", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Admin_Organisation_Permission_Rang_Voir(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            return new CmdReturn($"{rank.Permissions}", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
