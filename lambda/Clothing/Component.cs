using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Clothing
{
    public struct Component
    {
        public uint Drawable { get; set; }
        public uint Texture { get; set; }
        public uint Palette { get; set; }

        public Component(ushort drawable = 0, ushort texture = 0, ushort palette = 0)
        {
            Drawable = drawable;
            Texture = texture;
            Palette = palette;
        }

        public Component(Dictionary<string, string> datas, string name)
        {
            Drawable = ushort.Parse(datas[name + "_drawable"]);
            Texture = ushort.Parse(datas[name + "_texture"]);
            Palette = ushort.Parse(datas[name + "_palette"]);
        }

        public Dictionary<string, string> Data(string name)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[name + "_drawable"] = Drawable.ToString();
            data[name + "_texture"] = Texture.ToString();
            data[name + "_palette"] = Palette.ToString();
            return data;
        }

        public bool Equals(Component comp)
        {
            if (Drawable != comp.Drawable) return false;
            if (Texture != comp.Texture) return false;
            if (Palette != comp.Palette) return false;
            return true;
        }

        public static bool Equals(Component comp1, Component comp2)
        {
            if (comp1.Drawable != comp2.Drawable) return false;
            if (comp1.Texture != comp2.Texture) return false;
            if (comp1.Palette != comp2.Palette) return false;
            return true;
        }
    }
}