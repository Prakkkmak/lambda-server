using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Enums;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Skills;

namespace Lambda.Commands
{

    class TestCommands
    {
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Modele")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Modele(Player player, object[] argv)
        {
            if (!Enum.TryParse((string)argv[0], true, out PedModel model))
            {
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            if (!Enum.IsDefined(typeof(PedModel), model))
                return new CmdReturn("Modele incorrect", CmdReturn.CmdReturnType.WARNING);

            player.GetSkin().Model = model;
            player.GetSkin().SendModel(player);
            return CmdReturn.Success;
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Temps(Player player, object[] argv)
        {

            return new CmdReturn($"{player.TotalTimeOnline} , {player.TimeOnline}");
        }


        [Command(Command.CommandType.TEST, 1)]
        [Syntax("LockState")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Lockstate(Player player, object[] argv)
        {
            Vehicle vehicle = player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (!Enum.TryParse((string)argv[0], true, out VehicleLockState lockstate))
            {
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            if (!Enum.IsDefined(typeof(VehicleLockState), lockstate))
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            vehicle.SetLock(lockstate);
            return new CmdReturn("Vous avez changé letat du lock");
        }

        [Permission("TEST")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn testperm(Player player, object[] argv)
        {
            return new CmdReturn("oui");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Payday(Player player, object[] argv)
        {
            if (player.TimeOnline < 5) return new CmdReturn("Pas assez co");
            player.TimeOnline = 0;
            player.SetBankMoney(player.GetBankMoney() + 10000);
            return new CmdReturn("Vous avez recu le payday");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Temps")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Ajouter_Temps(Player player, object[] argv)
        {
            player.TimeOnline += (uint)argv[0];
            return new CmdReturn("Vous avez ajouté du temps de jeu");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn vingtsept(Player player, object[] argv)
        {
            return new CmdReturn("C 2 sECR3t 0+0+4+23");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Vehicules_Respawn(Player player, object[] argv)
        {
            foreach (Vehicle veh in player.Game.GetVehicles())
            {

                veh.Respawn();
            }
            return new CmdReturn("Les veh devraient avoir respawn");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("slot", "xp")]
        [SyntaxType(typeof(int), typeof(int))]
        public static CmdReturn Xp(Player player, object[] argv)
        {
            Skill.SkillType type = (Skill.SkillType)argv[0];
            player.AddExperience(type, (int)argv[1]);
            return new CmdReturn("Vous avez xp");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("slot")]
        [SyntaxType(typeof(int))]
        public static CmdReturn Level(Player player, object[] argv)
        {
            Skill.SkillType type = (Skill.SkillType)argv[0];
            Skill skill = player.GetSkill(type);
            return new CmdReturn("Vous etes level " + skill.GetLevel());
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Levels(Player player, object[] argv)
        {
            string txt = "";
            foreach (Skill playerSkill in player.Skills)
            {
                txt += "" + playerSkill.GetLevel() + "<br>";
            }
            return new CmdReturn(txt);
        }
    }
}


