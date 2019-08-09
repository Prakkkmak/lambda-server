using Lambda.Buildings;
using Lambda.Entity;
using Lambda.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class BankCommands
    {
        [Permission("CIVIL_BANQUE_BALANCE")]
        [Command(Command.CommandType.BANK)]
        public static CmdReturn Banque_Balance(Player player, object[] argv)
        {
            Bank bank = player.GetBuildings<Bank>(Bank.Banks);
            if (bank == null) return new CmdReturn("Pas de banque ici", CmdReturn.CmdReturnType.WARNING);
            return new CmdReturn("Votre balance est de " + player.BankMoney + "$");
        }
        [Permission("CIVIL_BANQUE_DEPOSER")]
        [Command(Command.CommandType.BANK,1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Banque_Deposer(Player player, object[] argv)
        {
            uint amount = (uint)argv[0];
            Bank bank = player.GetBuildings<Bank>(Bank.Banks);
            if (bank == null) return new CmdReturn("Pas de banque ici", CmdReturn.CmdReturnType.WARNING);
            if (player.Inventory.Money < amount) return new CmdReturn("Vous n'avez pas assez d'argent", CmdReturn.CmdReturnType.WARNING);
            player.Inventory.Money -= amount;
            player.BankMoney += amount;
            return new CmdReturn("Votre nouvelle balance est de " + player.BankMoney + "$");
        }
        [Permission("CIVIL_BANQUE_RETIRER")]
        [Command(Command.CommandType.BANK, 1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Banque_Retirer(Player player, object[] argv)
        {
            uint amount = (uint)argv[0];
            Bank bank = player.GetBuildings<Bank>(Bank.Banks);
            if (bank == null) return new CmdReturn("Pas de banque ici", CmdReturn.CmdReturnType.WARNING);
            if (player.BankMoney < amount) return new CmdReturn("Vous n'avez pas assez d'argent sur votre compte", CmdReturn.CmdReturnType.WARNING);
            player.Inventory.Money += amount;
            player.BankMoney -= amount;
            return new CmdReturn("Votre nouvelle balance est de " + player.BankMoney + "$");
        }

    }
}
