using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;

namespace Lambda.Commands
{
    class AdminCommands
    {
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Goto_Joueur(Player player, object[] argv)
        {
            player.Goto((Player)argv[0]);
            return new CmdReturn("Vous vous êtes téléporté.", CmdReturn.CmdReturnType.SUCCESS);


        }

        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Freeze(Player player, object[] argv)
        {
            ((Player)argv[0]).Freeze(true);
            return new CmdReturn("Vous vous avez freeze quelqu'un.", CmdReturn.CmdReturnType.SUCCESS);

        }

        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Joueur")]
        [SyntaxType(typeof(Player))]
        public static CmdReturn Unfreeze(Player player, object[] argv)
        {
            ((Player)argv[0]).Freeze(false);
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
            player.Spawn(player.Game.GetSpawn(0).Position);
            return new CmdReturn("Vous vous avez respawn!", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
