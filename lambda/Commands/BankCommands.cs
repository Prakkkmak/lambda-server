using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{
    class BankCommands
    {
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.BANK)]
        public static CmdReturn Banque_Creer(Player player, object[] argv)
        {
            Area bank = new Area(2, 2, Area.AreaType.BANK);
            player.Game.AddArea(bank);
            bank.Spawn(player.FeetPosition);
            player.Game.DbArea.Save(bank);
            return new CmdReturn("Vous avez créé une banque !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.BANK, 1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(int))]
        public static CmdReturn Retirer(Player player, object[] argv)
        {
            int amount = (int)argv[0];
            Area bank = player.Game.GetArea(player.Position, Area.AreaType.BANK);
            if (bank == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            player.Withdraw(amount);
            return new CmdReturn("Vous avez retiré de l'argent !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.BANK, 1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(int))]
        public static CmdReturn Deposer(Player player, object[] argv)
        {
            int amount = (int)argv[0];
            Area bank = player.Game.GetArea(player.Position, Area.AreaType.BANK);
            if (bank == null) return new CmdReturn("Aucune zone a ete trouvé", CmdReturn.CmdReturnType.LOCATION);
            player.Deposit(amount);
            return new CmdReturn("Vous avez déposé de l'argent !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.BANK)]
        public static CmdReturn Balance(Player player, object[] argv)
        {
            return new CmdReturn($"Vous avez {player.GetBankMoney()}$ sur votre compte.");
        }
    }
}
