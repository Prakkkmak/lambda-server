using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Clothing
{
    public class Accessory : ISkinPart
    {
        public Component Mask = new Component(); //  1
        public Prop Hat = new Prop(8); // Prop 0
        public Prop Glasses = new Prop(0); // Prop 1
        public Prop Ears = new Prop(33); // Prop 2
        public Prop Watch = new Prop(0); // Prop 6
        public Prop Bracelet = new Prop(0); // Prop 7


        public void Set(string str)
        {
            string[] infos = str.Split(',');
            int i = 0;
            Mask.Drawable = Convert.ToUInt32(infos[i++]);
            Mask.Texture = Convert.ToUInt32(infos[i++]);
            Mask.Palette = Convert.ToUInt32(infos[i++]);
            Hat.Drawable = Convert.ToUInt32(infos[i++]);
            Hat.Texture = Convert.ToUInt32(infos[i++]);
            Glasses.Drawable = Convert.ToUInt32(infos[i++]);
            Glasses.Texture = Convert.ToUInt32(infos[i++]);
            Ears.Drawable = Convert.ToUInt32(infos[i++]);
            Ears.Texture = Convert.ToUInt32(infos[i++]);
            Watch.Drawable = Convert.ToUInt32(infos[i++]);
            Watch.Texture = Convert.ToUInt32(infos[i++]);
            Bracelet.Drawable = Convert.ToUInt32(infos[i++]);
            Bracelet.Texture = Convert.ToUInt32(infos[i++]);
        }

        public override string ToString()
        {
            return Mask.Drawable + "," +
                   Mask.Texture + "," +
                   Mask.Palette + "," +
                   Hat.Drawable + "," +
                   Hat.Texture + "," +
                   Glasses.Drawable + "," +
                   Glasses.Texture + "," +
                   Ears.Drawable + "," +
                   Ears.Texture + "," +
                   Watch.Drawable + "," +
                   Watch.Texture + "," +
                   Bracelet.Drawable + "," +
                   Bracelet.Texture;
        }

        public void Send(Player player)
        {
            player.Emit("setComponent", 1, Mask.Drawable, Mask.Texture, Mask.Palette);
            player.Emit("setProp", 0, Hat.Drawable, Hat.Texture);
            player.Emit("setProp", 1, Glasses.Drawable, Glasses.Texture);
            player.Emit("setProp", 2, Ears.Drawable, Ears.Texture);
            player.Emit("setProp", 3, Watch.Drawable, Watch.Texture);
            player.Emit("setProp", 4, Bracelet.Drawable, Bracelet.Texture);
        }
    }
}
