using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Clothes : ISkinPart
    {
        public Component Torso = new Component(); //3
        public Component Leg = new Component(); // 4
        public Component Shoe = new Component(); // 6
        public Component Accessories = new Component(); // 7
        public Component Undershirt = new Component(); // 8
        public Component Top = new Component(); // 11



        public void Set(string str)
        {
            string[] infos = str.Split(',');
            int i = 0;
            Torso.Drawable = Convert.ToUInt32(infos[i++]);
            Torso.Texture = Convert.ToUInt32(infos[i++]);
            Torso.Palette = Convert.ToUInt32(infos[i++]);
            Leg.Drawable = Convert.ToUInt32(infos[i++]);
            Leg.Texture = Convert.ToUInt32(infos[i++]);
            Leg.Palette = Convert.ToUInt32(infos[i++]);
            Shoe.Drawable = Convert.ToUInt32(infos[i++]);
            Shoe.Texture = Convert.ToUInt32(infos[i++]);
            Shoe.Palette = Convert.ToUInt32(infos[i++]);
            Accessories.Drawable = Convert.ToUInt32(infos[i++]);
            Accessories.Texture = Convert.ToUInt32(infos[i++]);
            Accessories.Palette = Convert.ToUInt32(infos[i++]);
            Undershirt.Drawable = Convert.ToUInt32(infos[i++]);
            Undershirt.Texture = Convert.ToUInt32(infos[i++]);
            Undershirt.Palette = Convert.ToUInt32(infos[i++]);
            Top.Drawable = Convert.ToUInt32(infos[i++]);
            Top.Texture = Convert.ToUInt32(infos[i++]);
            Top.Palette = Convert.ToUInt32(infos[i++]);
        }

        public override string ToString()
        {
            return Torso.Drawable + "," +
                   Torso.Texture + "," +
                   Torso.Palette + "," +
                   Leg.Drawable + "," +
                   Leg.Texture + "," +
                   Leg.Palette + "," +
                   Shoe.Drawable + "," +
                   Shoe.Texture + "," +
                   Shoe.Palette + "," +
                   Accessories.Drawable + "," +
                   Accessories.Texture + "," +
                   Accessories.Palette + "," +
                   Undershirt.Drawable + "," +
                   Undershirt.Texture + "," +
                   Undershirt.Palette + "," +
                   Top.Drawable + "," +
                   Top.Texture + "," +
                   Top.Palette;
        }
        public void Send(Player player)
        {
            player.Emit("setComponent", 3, Torso.Drawable, Torso.Texture, Torso.Palette);
            player.Emit("setComponent", 4, Leg.Drawable, Leg.Texture, Leg.Palette);
            player.Emit("setComponent", 6, Shoe.Drawable, Shoe.Texture, Shoe.Palette);
            player.Emit("setComponent", 7, Accessories.Drawable, Accessories.Texture, Accessories.Palette);
            player.Emit("setComponent", 8, Undershirt.Drawable, Undershirt.Texture, Undershirt.Palette);
            player.Emit("setComponent", 11, Top.Drawable, Top.Texture, Top.Palette);
        }
    }


}
