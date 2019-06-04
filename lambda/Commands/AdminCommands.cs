using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Commands
{
    class AdminCommands
    {
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Goto(Player player, object[] argv)
        {
            player.Goto((Player)argv[0]);
            return new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Gethere(Player player, object[] argv)
        {
            ((Player)argv[0]).Goto(player);
            ((Player)argv[0]).SendMessage("Vous vous êtes fait téléporté.");
            return new CmdReturn("Vous vous avez téléporté quelqu'un à vous.", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Freeze(Player player, object[] argv)
        {
            ((Player)argv[0]).Freeze(true);
            ((Player)argv[0]).SendMessage("Vous avez été freeze.");
            return new CmdReturn("Vous vous avez freeze quelqu'un.", CmdReturn.CmdReturnType.SUCCESS);

        }

        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Unfreeze(Player player, object[] argv)
        {
            ((Player)argv[0]).Freeze(false);
            ((Player)argv[0]).SendMessage("Vous avez été défreeze.");
            return new CmdReturn("Vous vous avez unfreeze quelqu'un.", CmdReturn.CmdReturnType.SUCCESS);

        }

        [Command(Command.CommandType.ADMIN, 3)]
        [Syntax("X", "Y", "Z")]
        [SyntaxType(typeof(int), typeof(int), typeof(int))]
        public static CmdReturn Goto_Position(Player player, object[] argv)
        {
            int x = (int)argv[0];
            int y = (int)argv[1];
            int z = (int)argv[2];
            Position pos = new Position(x, y, z);
            player.Goto(pos);
            return new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN, 2)]
        [Syntax("Joueur", "Dimension")]
        [SyntaxType(typeof(Player), typeof(short))]
        public static CmdReturn Dimension(Player player, object[] argv)
        {
            ((Player)argv[0]).Dimension = (short)argv[1];
            ((Player)argv[0]).SendMessage("Un administrateur vous a changé de dimension");
            return new CmdReturn("Vous avez changé de dimension à quelquun.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Respawn(Player player, object[] argv)
        {
            player.Spawn(Spawn.NewSpawn.Position);
            return new CmdReturn("Vous vous avez respawn!", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Permission")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Permission_Ajouter(Player player, object[] argv)
        {
            player.Permissions.Add((string)argv[0]);
            return new CmdReturn("Vous avez ajouté une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Permission")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Permission_Retirer(Player player, object[] argv)
        {
            player.Permissions.Add((string)argv[0]);
            return new CmdReturn("Vous avez supprimé une permission", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.INVENTORY, 3)]
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
    }
}
