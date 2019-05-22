using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
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
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle == null) return CmdReturn.NotInVehicle;
            if (!Enum.TryParse((string)argv[0], true, out VehicleLockState lockstate))
            {
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            }

            if (!Enum.IsDefined(typeof(VehicleLockState), lockstate))
                return new CmdReturn("State incorrect", CmdReturn.CmdReturnType.WARNING);
            vehicle.LockState = lockstate;
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
            player.Emit("playAnim", argv);
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
            player.Emit("playAnim", anim.Dictionary, anim.Animation);
            return new CmdReturn(anim.Dictionary + " " + anim.Animation);
        }
        [Permission("LEADER")]
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Leader_Test(Player player, object[] argv)
        {
            return new CmdReturn("Ca marche");
        }
        [Permission("TEST")]
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Truc_Marant(Player player, object[] argv)
        {
            if (player.IsInVehicle)
            {

            }
            return new CmdReturn("Ca marche");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("1", "2")]
        [SyntaxType(typeof(byte), typeof(byte))]
        public static CmdReturn Bite(Player player, object[] argv)
        {
            if (player.IsInVehicle)
            {
                player.Vehicle.ModKit = 1;
                player.Vehicle.SetMod((byte)argv[0], (byte)argv[1]);
            }
            return new CmdReturn("Ca marche");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Permission_Voir(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            return new CmdReturn(target.Permissions.ToString());
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Menotter(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            target.Emit("setHandcuff");
            target.SendMessage("Vous avez été menotté/démenotté");
            return new CmdReturn("Vous avez menotté/démenotté quelqu'un");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Nom")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Arme(Player player, object[] argv)
        {
            player.Emit("giveWeapon", (string)argv[0]);
            return new CmdReturn("Vous vous etes donné un objet");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("ipl")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Zone_Interieur_Ajouter_Ipl(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            area.InteriorLocation.Interior.AddIpl((string)argv[0]);
            return new CmdReturn("Vous avez ajouter ipl");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 3)]
        [Syntax("x", "y", "z")]
        [SyntaxType(typeof(float), typeof(float), typeof(float))]
        public static CmdReturn Zone_Interieur_Position(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            Position pos = area.InteriorLocation.Interior.Position;
            pos.X = (float)argv[0];
            pos.Y = (float)argv[1];
            pos.Z = (float)argv[2];
            area.InteriorLocation.Interior.Position = pos;
            return new CmdReturn("Vous avez changé pos");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Zone_Interieur_Activer(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            area.SetLocations(area.InteriorLocation.Interior, area.Dimension);
            return new CmdReturn("Vous avez changé pos");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 0)]
        public static CmdReturn Zone_Save(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            _ = area.SaveAsync();
            return new CmdReturn("Vous avez save la zone");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("1", "2")]
        [SyntaxType(typeof(uint), typeof(uint))]
        public static CmdReturn Cheveux(Player player, object[] argv)
        {
            player.Emit("setHairColor", (uint)argv[0], (uint)argv[1]);
            return new CmdReturn("Vous avez changé la couleur de vos cheveux");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 3)]
        [Syntax("Maman", "Papa", "Mix")]
        [SyntaxType(typeof(uint), typeof(uint), typeof(float))]
        public static CmdReturn Forme(Player player, object[] argv)
        {
            uint mother = (uint)argv[0];
            uint father = (uint)argv[1];
            float mix = (float)argv[2];
            player.Emit("setShape", mother, father, mix);
            return new CmdReturn("Vous avez changé la forme");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 3)]
        [Syntax("Maman", "Papa", "Mix")]
        [SyntaxType(typeof(uint), typeof(uint), typeof(float))]
        public static CmdReturn Peau(Player player, object[] argv)
        {
            uint mother = (uint)argv[0];
            uint father = (uint)argv[1];
            float mix = (float)argv[2];
            player.Emit("setSkin", mother, father, mix);
            return new CmdReturn("Vous avez changé la peau");
        }
    }
}


