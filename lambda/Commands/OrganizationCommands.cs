using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Organizations;
using Lambda.Utils;

namespace Lambda.Commands
{
    class OrganizationCommands
    {
        [Permission("ORGANISATION_FACTURE")]
        [Command(Command.CommandType.ORGANIZATION, 2)]
        [Syntax("Organisation", "Prix")]
        [SyntaxType(typeof(Organization), typeof(uint))]
        public static CmdReturn Facture(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            uint price = (uint)argv[1];
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            if (!player.IsAllowedTo(org, "ORGANIZATION_FACTURE")) return new CmdReturn("Vous n'etes pas autorisés a faire cela");
            Request request = new Request(target, "Facture", $"Un joueur vous facture {price} pour l'organisation {org.Name}", player);
            request.AddAnswer("Accepter", () =>
            {
                player.SendMessage($"{target.FullName} a payé la facture!");
                org.BankMoney += (int)price;
                target.BankMoney -= price;
            });
            request.AddAnswer("Refuser", () =>
            {
                player.SendMessage($"{target.FullName} a refusé votre facture:");
            });
            request.Condition = () =>
            {
                return true;
            };
            target.SendRequest(request);
            return new CmdReturn($"Vous avez envoyé une facture à machin", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
