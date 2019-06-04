using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Face : ISkinPart
    {
        /* Mother father */
        public uint ShapeMother = 0;
        public uint ShapeFather = 0;
        public float ShapeMix = 0; // 0 - 1;
        public uint SkinMother = 0;
        public uint SkinFather = 0;
        public float SkinMix = 0; // 0 - 1;
        /* Overlay */
        public Overlay Blemishes = new Overlay(22); // 2 => 0 - 23
        public Overlay Ageing = new Overlay(0); // 3 => 0 - 14
        public Overlay Complexion = new Overlay(12); // 6 => 0-11
        public Overlay SunDamage = new Overlay(0);// 7 => 0-10
        public Overlay Freckles = new Overlay(18); // 9 => 0-17
        /* Face Feature -1 -> 1 */
        public float NoseWidth = 0; // 0
        public float NoseHeight = 0; // 1
        public float NoseLength = 0; // 2
        public float NoseBridge = 0; // 3
        public float NoseTip = 0; // 4
        public float NoseBridgeShift = 0; // 5
        public float BrowHeight = 0; // 6
        public float BrowWidth = 0; // 7
        public float CheekboneHeight = 0; // 8
        public float CheekboneWidth = 0; // 9
        public float CheecksWidth = 0; // 10
        public float Eyes = 0; // 11
        public float Lips = 0; //12
        public float JawWidth = 0; // 13
        public float JawHeight = 0; // 14
        public float ChinLength = 0;  // 15
        public float ChinPosition = 0; // 16 0->1
        public float ChinWidth = 0; //17
        public float ChinShape = 0; // 18
        public float NeckWidth = 0; // 19
        /* Other */
        public uint EyeColor = 0;
        public uint EyeColor2 = 0;

        public void Set(string str)
        {
            string[] infos = str.Split(',');
            int i = 0;
            ShapeMother = Convert.ToUInt32(infos[i++]);
            ShapeFather = Convert.ToUInt32(infos[i++]);
            ShapeMix = Convert.ToSingle(infos[i++]);
            SkinMother = Convert.ToUInt32(infos[i++]);
            SkinFather = Convert.ToUInt32(infos[i++]);
            SkinMix = Convert.ToSingle(infos[i++]);
            Blemishes.Index = Convert.ToUInt32(infos[i++]);
            Blemishes.Opacity = Convert.ToSingle(infos[i++]);
            Blemishes.Color1 = Convert.ToUInt32(infos[i++]);
            Blemishes.Color2 = Convert.ToUInt32(infos[i++]);
            Ageing.Index = Convert.ToUInt32(infos[i++]);
            Ageing.Opacity = Convert.ToSingle(infos[i++]);
            Ageing.Color1 = Convert.ToUInt32(infos[i++]);
            Ageing.Color2 = Convert.ToUInt32(infos[i++]);
            Complexion.Index = Convert.ToUInt32(infos[i++]);
            Complexion.Opacity = Convert.ToSingle(infos[i++]);
            Complexion.Color1 = Convert.ToUInt32(infos[i++]);
            Complexion.Color2 = Convert.ToUInt32(infos[i++]);
            SunDamage.Index = Convert.ToUInt32(infos[i++]);
            SunDamage.Opacity = Convert.ToSingle(infos[i++]);
            SunDamage.Color1 = Convert.ToUInt32(infos[i++]);
            SunDamage.Color2 = Convert.ToUInt32(infos[i++]);
            Freckles.Index = Convert.ToUInt32(infos[i++]);
            Freckles.Opacity = Convert.ToSingle(infos[i++]);
            Freckles.Color1 = Convert.ToUInt32(infos[i++]);
            Freckles.Color2 = Convert.ToUInt32(infos[i++]);
            NoseWidth = Convert.ToSingle(infos[i++]);
            NoseHeight = Convert.ToSingle(infos[i++]);
            NoseLength = Convert.ToSingle(infos[i++]);
            NoseBridge = Convert.ToSingle(infos[i++]);
            NoseTip = Convert.ToSingle(infos[i++]);
            NoseBridgeShift = Convert.ToSingle(infos[i++]);
            BrowHeight = Convert.ToSingle(infos[i++]);
            BrowWidth = Convert.ToSingle(infos[i++]);
            CheekboneHeight = Convert.ToSingle(infos[i++]);
            CheekboneWidth = Convert.ToSingle(infos[i++]);
            CheecksWidth = Convert.ToSingle(infos[i++]);
            Eyes = Convert.ToSingle(infos[i++]);
            Lips = Convert.ToSingle(infos[i++]);
            JawWidth = Convert.ToSingle(infos[i++]);
            JawHeight = Convert.ToSingle(infos[i++]);
            ChinLength = Convert.ToSingle(infos[i++]);
            ChinPosition = Convert.ToSingle(infos[i++]);
            ChinWidth = Convert.ToSingle(infos[i++]);
            ChinShape = Convert.ToSingle(infos[i++]);
            NeckWidth = Convert.ToSingle(infos[i++]);
            EyeColor = Convert.ToUInt32(infos[i++]);
            EyeColor2 = Convert.ToUInt32(infos[i++]);
        }


        public override string ToString()
        {
            return ShapeMother + "," +
                   ShapeFather + "," +
                   ShapeMix + "," +
                   SkinMother + "," +
                   SkinFather + "," +
                   SkinMix + "," +
                   Blemishes.Index + "," +
                   Blemishes.Opacity + "," +
                   Blemishes.Color1 + "," +
                   Blemishes.Color2 + "," +
                   Ageing.Index + "," +
                   Ageing.Opacity + "," +
                   Ageing.Color1 + "," +
                   Ageing.Color2 + "," +
                   Complexion.Index + "," +
                   Complexion.Opacity + "," +
                   Complexion.Color1 + "," +
                   Complexion.Color2 + "," +
                   SunDamage.Index + "," +
                   SunDamage.Opacity + "," +
                   SunDamage.Color1 + "," +
                   SunDamage.Color2 + "," +
                   Freckles.Index + "," +
                   Freckles.Opacity + "," +
                   Freckles.Color1 + "," +
                   Freckles.Color2 + "," +
                   NoseWidth + "," +
                   NoseHeight + "," +
                   NoseLength + "," +
                   NoseBridge + "," +
                   NoseTip + "," +
                   NoseBridgeShift + "," +
                   BrowHeight + "," +
                   BrowWidth + "," +
                   CheekboneHeight + "," +
                   CheekboneWidth + "," +
                   CheecksWidth + "," +
                   Eyes + "," +
                   Lips + "," +
                   JawWidth + "," +
                   JawHeight + "," +
                   ChinLength + "," +
                   ChinPosition + "," +
                   ChinWidth + "," +
                   ChinShape + "," +
                   NeckWidth + "," +
                   EyeColor + "," +
                   EyeColor2;
        }

        public void Send(Player player)
        {
            player.Emit("setShape", ShapeMother, ShapeFather, ShapeMix);
            player.Emit("setSkin", SkinMother, SkinFather, SkinMix);
            player.Emit("setHeadOverlay", 2, Blemishes.Index, Blemishes.Opacity, Blemishes.Color1, Blemishes.Color2);
            player.Emit("setHeadOverlay", 3, Ageing.Index, Ageing.Opacity, Ageing.Color1, Ageing.Color2);
            player.Emit("setHeadOverlay", 6, Complexion.Index, Complexion.Opacity, Complexion.Color1, Complexion.Color2);
            player.Emit("setHeadOverlay", 7, SunDamage.Index, SunDamage.Opacity, SunDamage.Color1, SunDamage.Color2);
            player.Emit("setHeadOverlay", 9, Freckles.Index, Freckles.Opacity, Freckles.Color1, Freckles.Color2);
            player.Emit("setFaceFeature", 0, NoseWidth);
            player.Emit("setFaceFeature", 1, NoseHeight);
            player.Emit("setFaceFeature", 2, NoseLength);
            player.Emit("setFaceFeature", 3, NoseBridge);
            player.Emit("setFaceFeature", 4, NoseTip);
            player.Emit("setFaceFeature", 5, NoseBridgeShift);
            player.Emit("setFaceFeature", 6, BrowHeight);
            player.Emit("setFaceFeature", 7, BrowWidth);
            player.Emit("setFaceFeature", 8, CheekboneHeight);
            player.Emit("setFaceFeature", 9, CheekboneWidth);
            player.Emit("setFaceFeature", 10, CheecksWidth);
            player.Emit("setFaceFeature", 11, Eyes);
            player.Emit("setFaceFeature", 12, Lips);
            player.Emit("setFaceFeature", 13, JawWidth);
            player.Emit("setFaceFeature", 14, JawHeight);
            player.Emit("setFaceFeature", 15, ChinLength);
            player.Emit("setFaceFeature", 16, ChinPosition);
            player.Emit("setFaceFeature", 17, ChinShape);
            player.Emit("setFaceFeature", 18, NeckWidth);
            player.Emit("setEyeColor", EyeColor, EyeColor2);


        }
    }
}
