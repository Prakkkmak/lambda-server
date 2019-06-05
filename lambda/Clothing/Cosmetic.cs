using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Cosmetic : ISkinPart
    {
        public Overlay Makeup = new Overlay(36, 1); // 4 0 - 74
        public Overlay Blush = new Overlay(33, 1); // 5 0 - 32
        public Overlay Lipstick = new Overlay(6, 1); // 8 0 - 9 NORMAL = 6



        public void Set(string str)
        {
            string[] infos = str.Split(',');
            int i = 0;
            Makeup.Index = Convert.ToUInt32(infos[i++]);
            Makeup.Opacity = Convert.ToSingle(infos[i++]);
            Makeup.Color1 = Convert.ToUInt32(infos[i++]);
            Makeup.Color2 = Convert.ToUInt32(infos[i++]);
            Blush.Index = Convert.ToUInt32(infos[i++]);
            Blush.Opacity = Convert.ToSingle(infos[i++]);
            Blush.Color1 = Convert.ToUInt32(infos[i++]);
            Blush.Color2 = Convert.ToUInt32(infos[i++]);
            Lipstick.Index = Convert.ToUInt32(infos[i++]);
            Lipstick.Opacity = Convert.ToSingle(infos[i++]);
            Lipstick.Color1 = Convert.ToUInt32(infos[i++]);
            Lipstick.Color2 = Convert.ToUInt32(infos[i++]);
        }

        public override string ToString()
        {
            return Makeup.Index + "," +
                   Makeup.Opacity + "," +
                   Makeup.Color1 + "," +
                   Makeup.Color2 + "," +
                   Blush.Index + "," +
                   Blush.Opacity + "," +
                   Blush.Color1 + "," +
                   Blush.Color2 + "," +
                   Lipstick.Index + "," +
                   Lipstick.Opacity + "," +
                   Lipstick.Color1 + "," +
                   Lipstick.Color2;
        }
        public void Send(Player player)
        {
            player.Emit("setHeadOverlay", 4, Makeup.Index, Makeup.Opacity, Makeup.Color1, Makeup.Color2);
            player.Emit("setHeadOverlay", 5, Blush.Index, Blush.Opacity, Blush.Color1, Blush.Color2);
            player.Emit("setHeadOverlay", 8, Lipstick.Index, Lipstick.Opacity, Lipstick.Color1, Lipstick.Color2);
        }

    }
}
