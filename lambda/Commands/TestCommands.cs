using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Enums;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Skills;
using Lambda.Utils;

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
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Vehicules_Respawn(Player player, object[] argv)
        {
            foreach (Vehicle veh in Vehicle.Vehicles)
            {

                veh.Respawn();
            }
            return new CmdReturn("Les veh devraient avoir respawn");
        }

        [Command(Command.CommandType.TEST, 2)]
        [Syntax("slot", "xp")]
        [SyntaxType(typeof(int), typeof(int))]
        public static CmdReturn Xp(Player player, object[] argv)
        {
            Skill.SkillType type = (Skill.SkillType)argv[0];
            player.AddExperience(type, (int)argv[1]);
            return new CmdReturn("Vous avez xp");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("slot")]
        [SyntaxType(typeof(int))]
        public static CmdReturn Level(Player player, object[] argv)
        {
            Skill.SkillType type = (Skill.SkillType)argv[0];
            Skill skill = player.GetSkill(type);
            return new CmdReturn("Vous etes level " + skill.GetLevel());
        }
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
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("dict", "anim")]
        [SyntaxType(typeof(string), typeof(string))]
        public static CmdReturn specificanim(Player player, object[] argv)
        {
            player.AltPlayer.Emit("playAnim", argv);
            return new CmdReturn("anim?");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("numero")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn anim(Player player, object[] argv)
        {
            uint index = (uint)argv[0];
            if (Anim.Anims.Length <= index) return CmdReturn.InvalidParameters;
            Anim anim = Anim.Anims[index];
            player.AltPlayer.Emit("playAnim", anim.Dictionary, anim.Animation);
            return new CmdReturn(anim.Dictionary + " " + anim.Animation);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Organization", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Permissions))]
        public static CmdReturn Organisation_Permission_Ajouter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string perm = (string)argv[1];
            org.Permissions.Add(perm);
            _ = org.SaveAsync();
            return new CmdReturn("Vous avez ajouté une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Organization", "Permission")]
        [SyntaxType(typeof(Organization), typeof(Permissions))]
        public static CmdReturn Organisation_Permission_Supprimer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string perm = (string)argv[1];
            org.Permissions.Remove(perm);
            _ = org.SaveAsync();
            return new CmdReturn("Vous avez supprimé une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Organization")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Organisation_Permission_Voir(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            string str = "";
            foreach (string s in org.Permissions.Get())
            {
                str += s + " - ";
            }
            return new CmdReturn(str);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Organization", "Joueur")]
        [SyntaxType(typeof(Organization), typeof(Player))]
        public static CmdReturn Organisation_Leader_Creer(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Player player2 = (Player)argv[1];
            Rank rank = org.AddRank("Leader");
            rank.Permissions.Add("LEADER");
            org.AddMember(player2, rank);
            _ = org.SaveAsync();
            return new CmdReturn("Vous avez créé un leader");
        }
        [Permission("LEADER")]
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Organization", "Joueur")]
        [SyntaxType(typeof(Organization), typeof(Player))]
        public static CmdReturn Inviter(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            Player player2 = (Player)argv[1];
            Request request = new Request(player2, "Don", $"{player.Name} veux vous inviter dans son organisation", player);
            request.AddAnswer("Accepter", (sender, receiver) =>
            {
                org.AddMember(receiver);
                sender.SendMessage("Machin a accepter la demande");
            });
            request.AddAnswer("Refuser", (sender, receiver) =>
            {
                sender.SendMessage($"{receiver.Name} a refusé votre demande");
            });
            request.Condition = (sender, receiver) =>
            {
                return true;
            };
            player2.SendRequest(request);
            return new CmdReturn("Vous avez fait une leader");
        }
    }
}


