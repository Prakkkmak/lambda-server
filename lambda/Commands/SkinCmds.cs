using System;
using System.Collections.Generic;
using System.Text;
using Items;
using Lambda.Administration;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Commands
{
    class SkinCmds
    {
        // Set a skin in a specific slot
        // /vetement 1 0
        [Command(Command.CommandType.DEFAULT, "Slot", "Skin")]
        public static CmdReturn Vetement(Player player, string[] argv)
        {
            uint componentId = uint.Parse(argv[1]);
            ushort value = ushort.Parse(argv[2]);
            Component comp = new Component(value);
            player.GetSkin().SetComponent((byte)componentId, comp);
            player.GetSkin().SendSkin(player);

            return new CmdReturn("Vous avez set un skin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        // Change the component to the next at a specific slot
        // /vetement suivant 1
        [Command(Command.CommandType.DEFAULT, "Slot")]
        public static CmdReturn Vetement_Suivant(Player player, string[] argv)
        {
            byte componentId = byte.Parse(argv[2]);
            Skin skin = player.GetSkin();
            Component comp = skin.GetComponent(componentId);
            comp.Drawable++;
            skin.SetComponent(componentId, comp);
            skin.SendSkin(player);
            string clothName = Enum.GetName(typeof(Skin.ClothNumber), componentId);
            return new CmdReturn("Vous avez changé votre vetement en slot " + clothName.ToLower() + " valeur: " + (comp.Drawable), CmdReturn.CmdReturnType.SUCCESS);
        }

        // Change the component to the previous at a specific slot
        // /vetement suivant 1
        [Command(Command.CommandType.DEFAULT, "Slot")]
        public static CmdReturn Vetement_Precedent(Player player, string[] argv)
        {
            byte componentId = byte.Parse(argv[2]);
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
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Valider(Player player, string[] argv)
        {
            player.GetSkin().SetValid(true);
            //ComponentLink.AddSkinToComponentsLinks(player.Skin, ComponentLink.Valid.TRUE);
            //ComponentLink.SaveAll();
            return new CmdReturn($"Vous avez validé un vetement", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Mauvais(Player player, string[] argv)
        {
            player.GetSkin().SetValid(false);
            //Skin.BadSkins.Add(player.Skin);
            //ComponentLink.GenerateFalseComponentLink(player.Skin, true);
            //ComponentLink.GenerateFalseComponentLinks();
            //ComponentLink.SaveAll();
            //TODO
            //return CmdReturn.NotExceptedError;
            return new CmdReturn($"Vous avez set un vetement comme mauvais", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Tester(Player player, string[] argv)
        {
            return new CmdReturn("EN CONSTRUCTION", CmdReturn.CmdReturnType.ERROR);
            Skin skin = player.GetSkin();
            skin.SetNextNotValidSkin();
            skin.SendSkin(player);

            if (skin != null)
            {
                player.SetSkin(skin);

                return new CmdReturn($"Vous avez changé de vetement", CmdReturn.CmdReturnType.SUCCESS);
            }
            else
            {
                return new CmdReturn($"Aucun vetement trouvé", CmdReturn.CmdReturnType.WARNING);
            }

        }
        /*[Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Aleatoire(Player player, string[] argv)
        {

            Skin skin = Skin.GetRandomGoodSkin(player.Game);

            if (skin != null)
            {
                player.SetSkin(skin);

                return new CmdReturn($"Vous avez changé de vetement", CmdReturn.CmdReturnType.SUCCESS);
            }
            else
            {
                return new CmdReturn($"Aucun vetement trouvé", CmdReturn.CmdReturnType.WARNING);
            }

        }*/

    }
}
