using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Enums;
using Items;
using Lambda.Entity;
using Lambda.Items;

namespace Lambda.Commands
{
    class SkinCommands
    {
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Habits(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.ClothesMale, 1, player.Skin.Clothes.ToString());
                player.Skin.Clothes.Accessories.Drawable = 0;
                player.Skin.Clothes.Leg.Drawable = 21;
                player.Skin.Clothes.Shoe.Drawable = 34;
                player.Skin.Clothes.Torso.Drawable = 15;
                player.Skin.Clothes.Undershirt.Drawable = 15;
                player.Skin.Clothes.Top.Drawable = 15;

            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.ClothesFemale, 1, player.Skin.Clothes.ToString());
                player.Skin.Clothes.Accessories.Drawable = 0;
                player.Skin.Clothes.Leg.Drawable = 15;
                player.Skin.Clothes.Shoe.Drawable = 35;
                player.Skin.Clothes.Torso.Drawable = 15;
                player.Skin.Clothes.Undershirt.Drawable = 15;
                player.Skin.Clothes.Top.Drawable = 15;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous êtes déshabillé");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Chapeau(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.HatMale, 1, player.Skin.Accessory.Hat.ToString());
                player.Skin.Accessory.Hat.Drawable = 8;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.HatFemale, 1, player.Skin.Accessory.Hat.ToString());
                player.Skin.Accessory.Hat.Drawable = 57;
            }

            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé votre chapeau");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Lunettes(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.GlassesMale, 1, player.Skin.Accessory.Glasses.ToString());
                player.Skin.Accessory.Glasses.Drawable = 0;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.GlassesFemale, 1, player.Skin.Accessory.Glasses.ToString());
                player.Skin.Accessory.Glasses.Drawable = 1;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos lunettes");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Oreilles(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.EarsMale, 1, player.Skin.Accessory.Ears.ToString());
                player.Skin.Accessory.Ears.Drawable = 33;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.EarsFemale, 1, player.Skin.Accessory.Ears.ToString());
                player.Skin.Accessory.Ears.Drawable = 0;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos boucles d'oreille");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Montre(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.WatchMale, 1, player.Skin.Accessory.Watch.ToString());
                player.Skin.Accessory.Watch.Drawable = 2;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.WatchFemale, 1, player.Skin.Accessory.Watch.ToString());
                player.Skin.Accessory.Watch.Drawable = 1;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos boucles d'oreille");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Bracelet(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                player.Inventory.AddItem(Enums.Items.BraceletMale, 1, player.Skin.Accessory.Bracelet.ToString());
                player.Skin.Accessory.Bracelet.Drawable = 0;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                player.Inventory.AddItem(Enums.Items.BraceletFemale, 1, player.Skin.Accessory.Bracelet.ToString());
                player.Skin.Accessory.Bracelet.Drawable = 0;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos boucles d'oreille");
        }
    }
}
