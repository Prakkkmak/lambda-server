using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;
using Lambda.Organizations;
using Lambda.Utils;

namespace Lambda.Commands
{
    class LeaderCommands
    {
        [Permission("LEADER_INVITE")]
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Rank))]
        public static CmdReturn Leader_Inviter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            if (!player.IsAllowedTo(org, "LEADER_INVITE")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            Request request = new Request(target, "Invitation", $"{player.FullName} veux vous vous inviter dans son organisation {org.Name}", player);
            request.AddAnswer("Accepter", () =>
            {
                player.SendMessage($"{target.FullName} a accepté votre demande");
                org.AddMember(target, rank);
            });
            request.AddAnswer("Refuser", () =>
            {
                player.SendMessage($"{target.FullName} a refusé votre demande");
            });
            request.Condition = () =>
            {
                return true;
            };
            target.SendRequest(request);
            return new CmdReturn($"Vous invité {target.FullName} à l'organisation {org.Name} au rang de {rank.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("LEADER_FIRE")]
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Membre")]
        [SyntaxType(typeof(Organization), typeof(Member))]
        public static CmdReturn Leader_Virer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Member member = (Member)argv[1];
            if (!player.IsAllowedTo(org, "LEADER_FIRE")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            org.RemoveMember(member);
            return new CmdReturn($"Vous avez viré quelqu'un de votre organisation.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("LEADER_PROMOTE")]
        [Command(Command.CommandType.ORGANIZATION, 4)]
        [Syntax("Organisation", "Membre", "Organisation", "Rang")]
        [SyntaxType(typeof(Organization), typeof(Member), typeof(Organization), typeof(Rank))]
        public static CmdReturn Leader_Promouvoir(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Member member = (Member)argv[1];
            if ((Organization)argv[2] != org) return new CmdReturn("Organisations différentes", CmdReturn.CmdReturnType.WARNING);
            Rank rank = (Rank)argv[3];
            if (!player.IsAllowedTo(org, "LEADER_PROMOTE")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            org.ChangeMemberRank(member, rank);
            return new CmdReturn($"Vous avez changé le rang de {member.Name}.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("LEADER_PROMOTE")]
        [Command(Command.CommandType.ORGANIZATION, 3)]
        [Syntax("Organisation", "Rang", "Salaire")]
        [SyntaxType(typeof(Organization), typeof(Rank), typeof(uint))]
        public static CmdReturn Leader_Salaire(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Rank rank = (Rank)argv[1];
            uint salary = (uint)argv[2];
            if (!player.IsAllowedTo(org, "LEADER_SALARY")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            rank.Salary = salary;
            return new CmdReturn($"Vous avez changé le salaire du rang {rank.Name} à {salary}$.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("LEADER_ACCOUNT")]
        [Command(Command.CommandType.ORGANIZATION, 1)]
        [Syntax("Organisation")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Leader_Compte(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            if (!player.IsAllowedTo(org, "LEADER_ACCOUNT")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            return new CmdReturn($"L'organisation a {org.BankMoney} en banque.", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
