using System;
using System.Collections.Generic;
using System.Text;
using Items;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Commands
{
    class SkinCommands
    {
        [Command(Command.CommandType.SKIN, 2)]
        [Syntax("Slot", "Id")]
        [SyntaxType(typeof(byte), typeof(uint))]
        public static CmdReturn Vetement(Player player, object[] argv)
        {
            byte componentId = (byte)argv[0];
            uint value = (uint)argv[1];
            Component comp = new Component((ushort)value);
            player.GetSkin().SetComponent((byte)componentId, comp);
            player.GetSkin().SendSkin(player);
            return new CmdReturn("Vous avez set un skin !", CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
