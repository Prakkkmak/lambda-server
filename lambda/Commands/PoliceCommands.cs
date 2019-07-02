using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class PoliceCommands
    {
        [Permission("POLICE_FINE")]
        [Command(Command.CommandType.POLICE, 1)]
        [Syntax("Montant")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Police_Amende(Player player, object[] argv)
        {
            Player target = player.PlayerSelected;
            if (target == null) return new CmdReturn("Vous ne selectionnez personne");
            uint amount = (uint)argv[0];
            Request request = new Request(target, "Don", $"{player.FullName} vous a donné une amende.", player);
            request.AddAnswer("Accepter", () =>
            {
                player.SendMessage($"{target.FullName} a accepté votre demande");
                target.Inventory.Withdraw(amount);
            });
            request.AddAnswer("Refuser", () =>
            {
                player.SendMessage($"{target.FullName} a refusé votre demande");
            });
            request.Condition = () => true;
            target.SendRequest(request);
            return new CmdReturn("Vous avez posé une amende à quelqu'un !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("POLICE_EQUIP")]
        [Command(Command.CommandType.POLICE)]
        public static CmdReturn Police_Duty(Player player, object[] argv)
        {
            if (player.Position.Distance(new Position(455, -990, 30)) > 4) return new CmdReturn("Vous n'etes pas aux casier");
            player.GiveWeapon(Weapons.Pistol, 100, false);
            player.GiveWeapon(Alt.Hash("WEAPON_NIGHTSTICK"), 1, true);
            player.GiveWeapon(Alt.Hash("WEAPON_FLASHLIGHT"), 1, false);
            player.GiveWeapon(Alt.Hash("WEAPON_STUNGUN"), 15, false);
            return new CmdReturn("Vous vous êtes équippé.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("POLICE_HANDCUFF")]
        [Command(Command.CommandType.POLICE)]
        public static CmdReturn Police_Menotter(Player player, object[] argv)
        {
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            target.Emit("setHandcuff");
            target.SendMessage("Vous avez été menotté/démenotté");
            return new CmdReturn("Vous avez menotté quelqu'un");
        }
    }
}
