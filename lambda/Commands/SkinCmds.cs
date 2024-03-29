﻿using System;
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
            uint value = uint.Parse(argv[2]);
            player.Skin.SetComponent(componentId, value);
            player.Skin.SendSkin(player);

            return new CmdReturn("Vous avez set un skin !", CmdReturn.CmdReturnType.SUCCESS);
        }
        // Change the component to the next at a specific slot
        // /vetement suivant 1
        [Command(Command.CommandType.DEFAULT, "Slot")]
        public static CmdReturn Vetement_Suivant(Player player, string[] argv)
        {
            uint componentId = uint.Parse(argv[2]);
            Component comp = player.Skin.GetComponent(componentId);
            player.Skin.SetComponent(componentId, comp.Drawable + 1);
            player.Skin.SendSkin(player);
            string clothName = Enum.GetName(typeof(Skin.ClothNumber), componentId);
            return new CmdReturn("Vous avez changé votre vetement en slot " + clothName.ToLower() + " valeur: " + (comp.Drawable + 1), CmdReturn.CmdReturnType.SUCCESS);
        }

        // Change the component to the previous at a specific slot
        // /vetement suivant 1
        [Command(Command.CommandType.DEFAULT, "Slot")]
        public static CmdReturn Vetement_Precedent(Player player, string[] argv)
        {
            uint componentId = uint.Parse(argv[2]);
            Component comp = player.Skin.GetComponent(componentId);
            if (comp.Drawable < 1) comp.Drawable++;
            player.Skin.SetComponent(componentId, comp.Drawable - 1);
            player.Skin.SendSkin(player);
            string clothName = Enum.GetName(typeof(Skin.ClothNumber), componentId);
            return new CmdReturn("Vous avez changé votre vetement en slot " + clothName.ToLower() + " valeur: " + (comp.Drawable - 1), CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Valider(Player player, string[] argv)
        {
            ComponentLink.AddSkinToComponentsLinks(player.Skin, ComponentLink.Valid.TRUE);
            ComponentLink.SaveAll();
            return new CmdReturn($"Vous avez validé un vetement", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Mauvais(Player player, string[] argv)
        {
            Skin.BadSkins.Add(player.Skin);
            ComponentLink.GenerateFalseComponentLink(player.Skin, true);
            ComponentLink.GenerateFalseComponentLinks();
            ComponentLink.SaveAll();

            return new CmdReturn($"Vous avez set un vetement comme mauvais", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Generation(Player player, string[] argv)
        {
            int nbr = Skin.GenerateSkins();
            return new CmdReturn($"Il y a {nbr} skins valides", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Aleratoire(Player player, string[] argv)
        {
            player.SetSkin(Skin.Random());
            return new CmdReturn($"Vous avez un skin aléatoire", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.DEFAULT)]
        public static CmdReturn Vetement_Tester(Player player, string[] argv)
        {

            Skin skin = Skin.GetSkinToDiscover();

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

    }
}
