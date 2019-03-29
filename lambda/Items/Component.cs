using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Items
{
    public struct Component
    {
        public uint Drawable { get; set; }
        public uint Texture { get; set; }
        public uint Palette { get; set; }

        public Component(uint drawable = 0, uint texture = 0, uint palette = 0)
        {
            Drawable = drawable;
            Texture = texture;
            Palette = palette;
        }

        public Component(Dictionary<string, string> datas, string name)
        {
            Drawable = uint.Parse(datas[name + "_drawable"]);
            Texture = uint.Parse(datas[name + "_texture"]);
            Palette = uint.Parse(datas[name + "_palette"]);
        }

        public static bool Equals(Component comp1, Component comp2)
        {
            if (comp1.Drawable != comp2.Drawable) return false;
            if (comp1.Texture != comp2.Texture) return false;
            if (comp1.Palette != comp2.Palette) return false;
            return true;
        }

        public static uint MaskMaxValue = 147;
        public static uint HairMaxValue = 73;
        public static uint TorsoMaxValue = 15;
        public static uint LegMaxValue = 114;
        public static uint BagMaxValue = 80;
        public static uint FeetMaxValue = 90;
        public static uint AccessoiriesMaxValue = 131;
        public static uint UndershirtMaxValue = 143;
        public static uint BodyarmorMaxValue = 28;
        public static uint TopMaxValue = 289;
    }
}
