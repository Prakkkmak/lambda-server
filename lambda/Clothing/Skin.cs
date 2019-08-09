using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using AltV.Net;
using AltV.Net.Enums;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Skin
    {

        public uint Model = (uint)PedModel.FreemodeMale01;
        public Face Face = new Face();
        public Hairiness Hairiness = new Hairiness();
        public Cosmetic Cosmetic = new Cosmetic();
        public Clothes Clothes = new Clothes();
        public Accessory Accessory = new Accessory();


        public void Send(Player player, bool resetModel = true)
        {
            if (resetModel) player.Model = Model;
            Face.Send(player);
            Hairiness.Send(player);
            Cosmetic.Send(player);
            Clothes.Send(player);
            Accessory.Send(player);
        }

        public void SetComponents(int[] args)
        {
            string str = "";
            foreach (int i1 in args)
            {
                str += i1 + ",";
            }
            Alt.Log(str);
            int i = 0;
            i += 3; // Face
            Accessory.Mask.Drawable = (ushort)args[i++];
            Accessory.Mask.Texture = (ushort)args[i++];
            Accessory.Mask.Palette = (ushort)args[i++];
            Hairiness.Head.Drawable = (ushort)args[i++];
            Hairiness.Head.Texture = (ushort)args[i++];
            Hairiness.Head.Palette = (ushort)args[i++];
            Clothes.Torso.Drawable = (ushort)args[i++];
            Clothes.Torso.Texture = (ushort)args[i++];
            Clothes.Torso.Palette = (ushort)args[i++];
            Clothes.Leg.Drawable = (ushort)args[i++];
            Clothes.Leg.Texture = (ushort)args[i++];
            Clothes.Leg.Palette = (ushort)args[i++];
            i += 3; // bag
            Clothes.Shoe.Drawable = (ushort)args[i++];
            Clothes.Shoe.Texture = (ushort)args[i++];
            Clothes.Shoe.Palette = (ushort)args[i++];
            Clothes.Accessories.Drawable = (ushort)args[i++];
            Clothes.Accessories.Texture = (ushort)args[i++];
            Clothes.Accessories.Palette = (ushort)args[i++];
            Clothes.Undershirt.Drawable = (ushort)args[i++];
            Clothes.Undershirt.Texture = (ushort)args[i++];
            Clothes.Undershirt.Palette = (ushort)args[i++];
            i += 3; //Body armor
            i += 3; //Decal
            Clothes.Top.Drawable = (ushort)args[i++];
            Clothes.Top.Texture = (ushort)args[i++];
            Clothes.Top.Palette = (ushort)args[i++];
        }
        public void SetProps(int[] args)
        {
            int i = 0;
            Accessory.Hat.Drawable = (ushort)args[i++];
            Accessory.Hat.Texture = (ushort)args[i++];
            Accessory.Glasses.Drawable = (ushort)args[i++];
            Accessory.Glasses.Texture = (ushort)args[i++];
            Accessory.Ears.Drawable = (ushort)args[i++];
            Accessory.Ears.Texture = (ushort)args[i++];
            Accessory.Watch.Drawable = (ushort)args[i++];
            Accessory.Watch.Texture = (ushort)args[i++];
            Accessory.Bracelet.Drawable = (ushort)args[i++];
            Accessory.Bracelet.Texture = (ushort)args[i++];

        }
        public void SetOverlays(int[] args)
        {
            int i = 0;
            Face.Blemishes.Index = (ushort)args[i++];
            Face.Blemishes.Color1 = (ushort)args[i++];
            Face.Blemishes.Color2 = (ushort)args[i++];
            Hairiness.Facial.Index = (ushort)args[i++];
            Hairiness.Facial.Color1 = (ushort)args[i++];
            Hairiness.Facial.Color2 = (ushort)args[i++];
            Hairiness.EyeBrows.Index = (ushort)args[i++];
            Hairiness.EyeBrows.Color1 = (ushort)args[i++];
            Hairiness.EyeBrows.Color2 = (ushort)args[i++];
            Face.Ageing.Index = (ushort)args[i++];
            Face.Ageing.Color1 = (ushort)args[i++];
            Face.Ageing.Color2 = (ushort)args[i++];
            Cosmetic.Makeup.Index = (ushort)args[i++];
            Cosmetic.Makeup.Color1 = (ushort)args[i++];
            Cosmetic.Makeup.Color2 = (ushort)args[i++];
            Cosmetic.Blush.Index = (ushort)args[i++];
            Cosmetic.Blush.Color1 = (ushort)args[i++];
            Cosmetic.Blush.Color2 = (ushort)args[i++];
            Face.Complexion.Index = (ushort)args[i++];
            Face.Complexion.Color1 = (ushort)args[i++];
            Face.Complexion.Color2 = (ushort)args[i++];
            Face.SunDamage.Index = (ushort)args[i++];
            Face.SunDamage.Color1 = (ushort)args[i++];
            Face.SunDamage.Color2 = (ushort)args[i++];
            Cosmetic.Lipstick.Index = (ushort)args[i++];
            Cosmetic.Lipstick.Color1 = (ushort)args[i++];
            Cosmetic.Lipstick.Color2 = (ushort)args[i++];
            Face.Freckles.Index = (ushort)args[i++];
            Face.Freckles.Color1 = (ushort)args[i++];
            Face.Freckles.Color2 = (ushort)args[i++];
            Hairiness.Chest.Index = (ushort)args[i++];
            Hairiness.Chest.Color1 = (ushort)args[i++];
            Hairiness.Chest.Color2 = (ushort)args[i++];

        }
        public void SetFeatures(float[] args)
        {
            int i = 0;
            Face.NoseWidth = (float)args[i++];
            Face.NoseHeight = (float)args[i++];
            Face.NoseLength = (float)args[i++];
            Face.NoseBridge = (float)args[i++];
            Face.NoseTip = (float)args[i++];
            Face.NoseBridgeShift = (float)args[i++];
            Face.BrowHeight = (float)args[i++];
            Face.BrowWidth = (float)args[i++];
            Face.CheekboneHeight = (float)args[i++];
            Face.CheekboneWidth = (float)args[i++];
            Face.CheecksWidth = (float)args[i++];
            Face.Eyes = (float)args[i++];
            Face.Lips = (float)args[i++];
            Face.JawWidth = (float)args[i++];
            Face.JawHeight = (float)args[i++];
            Face.ChinLength = (float)args[i++];
            Face.ChinPosition = (float)args[i++];
            Face.ChinWidth = (float)args[i++];
            Face.ChinShape = (float)args[i++];
            Face.NeckWidth = (float)args[i++];
        }
        public void SetHeadData(string[] args)
        {
            int i = 0;
            uint.TryParse(args[0], out uint shapeMother);
            Face.ShapeMother = shapeMother;
            uint.TryParse(args[1], out uint shapeFather);
            Face.ShapeFather = shapeFather;
            uint.TryParse(args[2], out uint skinMother);
            Face.SkinMother = skinMother;
            uint.TryParse(args[3], out uint skinFather);
            Face.SkinFather = skinFather;
            float.TryParse(args[4], out float shapeMix);
            Face.ShapeMix = shapeMix;
            float.TryParse(args[5], out float skinMix);
            Face.SkinMix = skinMix;
        }

    }
}
