using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Hairiness : ISkinPart

    {
        public Component Head = new Component(0); // componentVariation 2: 0 - 73
        public uint HairColor = 0;
        public uint HairColor2 = 0;
        public Overlay Facial = new Overlay(0); // headOverlay 1: 0 - 28 29 ?
        public Overlay EyeBrows = new Overlay(0); // headOverlay 2 : 0 - 33
        public Overlay Chest = new Overlay(0); // headOverlay 10: 0 - 16


        public void Set(string str)
        {
            string[] infos = str.Split(',');
            int i = 0;
            Head.Drawable = Convert.ToUInt32(infos[i++]);
            Head.Texture = Convert.ToUInt32(infos[i++]);
            Head.Palette = Convert.ToUInt32(infos[i++]);
            HairColor = Convert.ToUInt32(infos[i++]);
            HairColor2 = Convert.ToUInt32(infos[i++]);
            Facial.Index = Convert.ToUInt32(infos[i++]);
            Facial.Opacity = Convert.ToSingle(infos[i++]);
            Facial.Color1 = Convert.ToUInt32(infos[i++]);
            Facial.Color2 = Convert.ToUInt32(infos[i++]);
            EyeBrows.Index = Convert.ToUInt32(infos[i++]);
            EyeBrows.Opacity = Convert.ToSingle(infos[i++]);
            EyeBrows.Color1 = Convert.ToUInt32(infos[i++]);
            EyeBrows.Color2 = Convert.ToUInt32(infos[i++]);
            Chest.Index = Convert.ToUInt32(infos[i++]);
            Chest.Opacity = Convert.ToSingle(infos[i++]);
            Chest.Color1 = Convert.ToUInt32(infos[i++]);
            Chest.Color2 = Convert.ToUInt32(infos[i++]);
        }

        public override string ToString()
        {
            return Head.Drawable + "," +
                   Head.Texture + "," +
                   Head.Palette + "," +
                   HairColor + "," +
                   HairColor2 + "," +
                   Facial.Index + "," +
                   Facial.Opacity + "," +
                   Facial.Color1 + "," +
                   Facial.Color2 + "," +
                   EyeBrows.Index + "," +
                   EyeBrows.Opacity + "," +
                   EyeBrows.Color1 + "," +
                   EyeBrows.Color2 + "," +
                   Chest.Index + "," +
                   Chest.Opacity + "," +
                   Chest.Color1 + "," +
                   Chest.Color2;
        }

        public void Send(Player player)
        {
            player.Emit("setComponent", 2, Head.Drawable, Head.Texture, Head.Palette);
            player.Emit("setHeadOverlay", 1, Facial.Index, Facial.Opacity, Facial.Color1, Facial.Color2);
            player.Emit("setHeadOverlay", 2, EyeBrows.Index, EyeBrows.Opacity, EyeBrows.Color1, EyeBrows.Color2);
            player.Emit("setHeadOverlay", 10, Chest.Index, Chest.Opacity, Chest.Color1, Chest.Color2);
            player.Emit("setHairColor", HairColor, HairColor2);
        }
    }
}
