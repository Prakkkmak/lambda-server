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
        // Change the component to the next at a specific slot
        // /vetement suivant 1
        [Command(Command.CommandType.SKIN, 1)]
        [Syntax("Slot")]
        [SyntaxType(typeof(byte))]
        public static CmdReturn Vetement_Suivant(Player player, object[] argv)
        {
            byte componentId = (byte)argv[0];
            Skin skin = player.GetSkin();
            Component comp = skin.GetComponent(componentId);
            comp.Drawable++;
            skin.SetComponent(componentId, comp);
            skin.SendSkin(player);
            string clothName = Enum.GetName(typeof(Skin.ClothNumber), componentId);
            return new CmdReturn("Vous avez changé votre vetement en slot " + clothName.ToLower() + " valeur: " + (comp.Drawable), CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.SKIN, 1)]
        [Syntax("Slot")]
        [SyntaxType(typeof(byte))]
        public static CmdReturn Vetement_Precedent(Player player, object[] argv)
        {
            byte componentId = (byte)argv[0];
            Skin skin = player.GetSkin();
            Component comp = skin.GetComponent(componentId);
            if (comp.Drawable >= 1)
            {
                comp.Drawable--;
                skin.SetComponent(componentId, comp);
                skin.SendSkin(player);
            }
            string clothName = Enum.GetName(typeof(Skin.ClothNumber), componentId);
            return new CmdReturn("Vous avez changé votre vetement en slot " + clothName.ToLower() + " valeur: " + (comp.Drawable), CmdReturn.CmdReturnType.SUCCESS);
        }
    }
}
