using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Quests;
using Lambda.Skills;
using Lambda.Utils;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda.Commands
{

    class TestCommands
    {
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Licenses(Player player, object[] argv)
        {
            return new CmdReturn(player.Licenses.ToString());
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Slot", "Valeur")]
        [SyntaxType(typeof(int), typeof(uint))]
        public static CmdReturn SetLicense(Player player, object[] argv)
        {
            int license = (int)argv[0];
            uint value = (uint)argv[1];
            if (license < 0 || license > player.Licenses.Values.Length) return new CmdReturn("Slot incorrect");
            player.Licenses.Values[license] = value;
            return new CmdReturn(player.Licenses.ToString());
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn License_Info(Player player, object[] argv)
        {
            string str = "";
            if (player.IsInCheckpoint("farm"))
            {
                str += "Licence 1 : 100$<br>";
                str += "Licence 2 : 1 200$<br>";
                str += "Licence 3 : 9 800$<br>";
                str += "Licence 4 : 45 800$<br>";
            }
            if (player.IsInCheckpoint("fish"))
            {
                str += "Licence 1 : 1 200$<br>";
                str += "Licence 2 : 12 000$<br>";
                str += "Licence 3 : 35 000$<br>";
                str += "Licence 4 : 230 000$<br>";
            }
                if (str.Equals("")) str = "Aucune licence n'est disponible ici";
            return new CmdReturn(str);
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn License_Acheter(Player player, object[] argv)
        {
            if (player.IsInCheckpoint("farm"))
            {
                uint value = player.Licenses.GetValue(Enums.LicenseType.Farming);
                if(value == 0 && player.Inventory.Money >= 100)
                {
                    player.Licenses.SetValue(Enums.LicenseType.Farming, 1);
                    player.Inventory.Withdraw(100);
                    return new CmdReturn("Vous avez acheté la licence de niveau 1 en fermier pour 100$ !");
                }
            }

            return new CmdReturn(player.Licenses.ToString());
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Fermier_Job_Tomattes(Player player, object[] argv)
        {
            if (player.Licenses.GetValue(Enums.LicenseType.Farming) < 1)
                return new CmdReturn("Vous n'avez pas la license adéquate.");
            player.StartQuest("farming_tomato_01");
            return new CmdReturn("");
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST,2)]
        [Syntax("Joueur", "Quete")]
        [SyntaxType(typeof(Player), typeof(string))]
        public static CmdReturn Quete_Demarrer(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            string quest = (string)argv[1];
            if (player.Quest != null) return new CmdReturn("Il y a déjà une quete en cours", CmdReturn.CmdReturnType.WARNING);
            player.StartQuest(quest);
            if(player.Quest == null) return new CmdReturn("Aucune quete trouvée", CmdReturn.CmdReturnType.WARNING);
            return new CmdReturn("Votre quête est lancée!");
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST,1)]
        [Syntax("Quete")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Quete_Chercher(Player player, object[] argv)
        {
            string st = (string)argv[0];
            string str = "";
            foreach(Quest quest in Quest.Quests)
            {
                if (quest.Name.StartsWith(st)) str += quest.Name + "  ";
            }
            if (str.Length == 0) str = "Aucune quete trouvée;";
            return new CmdReturn(str);
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Quete_Inspecter(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            if (target.Quest == null) return new CmdReturn("Ce joueur n'a aucune quete en cours");
            else return new CmdReturn("Quete en cours : " + target.Quest.Name);
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Quete_Completer(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            if (target.Quest == null) return new CmdReturn("Ce joueur n'a aucune quete en cours");
            else
            {
                target.Quest.Complete(target);
                return new CmdReturn("Vous avez complété votre quête.");
            }
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("a")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Prop(Player player, object[] argv)
        {
            string name = (string)argv[0];
            Prop prop = new TestProp(player.Position, player.Rotation, player.Dimension, name);
            
            return new CmdReturn("Prop placé");
        }
        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("a", "temps")]
        [SyntaxType(typeof(string), typeof(uint))]
        public static CmdReturn Fermier_Planter(Player player, object[] argv)
        {
            string name = (string)argv[0];
            uint sec = (uint)argv[1];
            Prop prop = new PlantProp(player.Position, player.Rotation, player.Dimension, name, player.Position.Z - 4, player.Position.Z - 2, (int)sec);
            return new CmdReturn("Prop planté");
        }


        [Permission("TESTEUR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Fermier_Recolter(Player player, object[] argv)
        {
            PlantProp plant = player.GetClosePlant(3);
            if (plant == null) return new CmdReturn("Il n'y a aucune plante ici");
            Item harvest = plant.Harvrest();
            if (harvest == null) return new CmdReturn("La plante n'est pas assez mure.");
            player.Inventory.AddItem(harvest);
            player.AddExperience(Skill.SkillType.FARMING, 5);
            if (player.Quest != null) if(player.Quest.Condition(player))player.Quest.Complete(player);
            return new CmdReturn("Vous avez récolté la plante.");
        }
        /*
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

            player.Skin.Model = (uint)model;
            player.Skin.Send(player);
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
            if (type == default) return new CmdReturn("Cette competence n'existe pas");
            Skill skill = player.GetSkill(type);
            if (skill == null) return new CmdReturn("Vous n'avez pas la competence");
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
            Alt.Log("pr3");
            Alt.Log(player.IsInVehicle + "");
            Alt.Log("pr32");
            if (!player.IsInVehicle) return new CmdReturn("Vous n'etes pas dans un véhicule");
            Alt.Log("pr31");
            byte category = (byte)argv[0];
            Alt.Log("ici0");
            byte value = (byte)argv[1];
            Alt.Log("ici");
            player.Vehicle.ModKit = 1;
            Alt.Log("ici2");
            player.Vehicle.SetMod(category, value);
            Alt.Log("ici3");
            return new CmdReturn("Ca marche");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn kouki(Player player, object[] argv)
        {
            Alt.Log("pr3");
            Alt.Log(player.IsInVehicle + "");
            Alt.Log("pr32");
            return new CmdReturn("ok");
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
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("1", "2")]
        [SyntaxType(typeof(uint), typeof(uint))]
        public static CmdReturn Cheveux(Player player, object[] argv)
        {
            player.Emit("setHairColor", (uint)argv[0], (uint)argv[1]);
            return new CmdReturn("Vous avez changé la couleur de vos cheveux");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("1")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Yeux(Player player, object[] argv)
        {
            player.Emit("setEyeColor", (uint)argv[0]);
            return new CmdReturn("Vous avez changé la couleur de vos yeux");
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

        [Command(Command.CommandType.TEST)]
        public static CmdReturn front_pos(Player player, object[] argv)
        {
            Position pos = PositionHelper.PositionInAngle(player.Position, player.Rotation, 0.25f);
            Alt.CreateCheckpoint(1, pos, 0.30f, 0.5f, new Rgba(0, 0, 0, 255));

            return new CmdReturn("Vous avez changé la peau");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Orga")]
        [SyntaxType(typeof(Organization))]
        public static CmdReturn Voiture_Organization(Player player, object[] argv)
        {
            Organization org = (Organization)argv[0];
            if (player.Vehicle == null) return CmdReturn.NotInVehicle;
            Vehicle veh = (Vehicle)player.Vehicle;
            veh.SetOwner(org.Id, Vehicle.OwnerType.ORGANIZATION);
            return new CmdReturn("Vous avez changé le proprio du véhicule");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Serrure")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Vehicule_Serrure(Player player, object[] argv)
        {
            if (player.Vehicle == null) return CmdReturn.NotInVehicle;
            Vehicle veh = (Vehicle)player.Vehicle;
            veh.Lock.Code = (string)argv[0];
            return new CmdReturn("Vous avez changé le proprio du véhicule");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Balise(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            target.Emit("attachBeaconToPlayer", target);
            return new CmdReturn("Vous avez changé le proprio du véhicule");
        }

        [Permission("POLICE_BEACON")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Osseccour(Player player, object[] argv)
        {
            foreach (Player target in Player.Players)
            {
                if (target.IsAllowedTo("POLICE_BEACON"))
                {
                    target.Emit("attachBeaconToPlayer", player);
                }
            }

            return new CmdReturn("Vous avez envoyé une balise");
        }

        [Permission("POLICE_BEACON")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Enfaitcava(Player player, object[] argv)
        {
            foreach (Player target in Player.Players)
            {
                if (target.IsAllowedTo("POLICE_BEACON"))
                {
                    target.Emit("detachBeaconToPlayer", player);
                }
            }

            return new CmdReturn("Vous avez retiré une balise");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Jetenferme(Player player, object[] argv)
        {
            Alt.EmitAllClients("closeDoortest");

            return new CmdReturn("Vous avez fermé bla h");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Jetenfouvre(Player player, object[] argv)
        {
            Alt.EmitAllClients("openDoortest");

            return new CmdReturn("Vous avez open bla   h");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Invisible(Player player, object[] argv)
        {
            player.Emit("toggleInvisibility");
            return new CmdReturn("Vous avez changé votre invisibilité.");
        }

        

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Spec_Joueur(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            player.Goto(target);
            player.Emit("setSpecTarget", target, true);
            return new CmdReturn("Vous spequez quelqu'un.");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Spec_Stop(Player player, object[] argv)
        {
            player.Emit("stopSpecTarget");
            return new CmdReturn("Vous arrêtez de spequer quelqu'un.");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Context(Player player, object[] argv)
        {
            player.GenerateContext();

            return new CmdReturn("Vous générez un contexte");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn PlaceProp(Player player, object[] argv)
        {
            Alt.Log("place prop .. ?");
            player.Emit("placeProp", "prop_mp_cone_01");

            return new CmdReturn("Vous placez un prop ?");
        }

        [Command(Command.CommandType.TEST)]
        public static CmdReturn Secret(Player player, object[] argv)
        {
            player.Emit("creatorTest");

            return new CmdReturn("Fonctionalité bêta: Vous êtes en test de la création de perso.");
        }

        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Temps")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Meteo(Player player, object[] argv)
        {

            Alt.EmitAllClients("weather", ((string)argv[0]).ToUpper());
            return new CmdReturn("Vous avez changé la météo ?");
        }

        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Heures", "Minutes")]
        [SyntaxType(typeof(uint), typeof(uint))]
        public static CmdReturn Heure(Player player, object[] argv)
        {
            Alt.EmitAllClients("setTime", (uint)argv[0], (uint)argv[1], 0);
            return new CmdReturn("Vous avez changé la montre ?");
        }
        */
    }
}

