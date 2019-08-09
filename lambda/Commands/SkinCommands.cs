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

        [Permission("CIVIL_VETEMENTS_ENLEVER_HABITS")]
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
        [Permission("CIVIL_VETEMENTS_ENLEVER_CHAPEAU")]
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
        [Permission("CIVIL_VETEMENTS_ENLEVER_LUNETTES")]
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
                player.Skin.Accessory.Glasses.Drawable = 5;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos lunettes");
        }
        [Permission("CIVIL_VETEMENTS_ENLEVER_OREILLES")]
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
                player.Skin.Accessory.Ears.Drawable = 33;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous vous avez enlevé vos boucles d'oreille");
        }
        [Permission("CIVIL_VETEMENTS_ENLEVER_MONTRE")]
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.SKIN)]
        public static CmdReturn Vetement_Enlever_Montre(Player player, object[] argv)
        {
            if (player.Skin.Model == (uint)PedModel.FreemodeMale01)
            {
                if (player.Skin.Accessory.Watch.Drawable == 2) return new CmdReturn("Vous n'avez pas de montre");
                player.Inventory.AddItem(Enums.Items.WatchMale, 1, player.Skin.Accessory.Watch.ToString());
                player.Skin.Accessory.Watch.Drawable = 2;
            }
            else if (player.Skin.Model == (uint)PedModel.FreemodeFemale01)
            {
                if (player.Skin.Accessory.Watch.Drawable == 1) return new CmdReturn("Vous n'avez pas de montre");
                player.Inventory.AddItem(Enums.Items.WatchFemale, 1, player.Skin.Accessory.Watch.ToString());
                player.Skin.Accessory.Watch.Drawable = 1;
            }
            player.Skin.Send(player, false);
            return new CmdReturn($"Vous avez enlevé votre montre.");
        }
        [Permission("CIVIL_VETEMENTS_ENLEVER_BRACELET")]
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
            return new CmdReturn($"Vous vous avez enlevé votre bracelet");
        }
        [Command(Command.CommandType.SKIN, 3)]
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
        [Command(Command.CommandType.SKIN, 3)]
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
    }
}
