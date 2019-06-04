using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Clothing
{
    public struct Prop
    {
        public uint Drawable { get; set; }
        public uint Texture { get; set; }

        public Prop(ushort drawable = 0, ushort texture = 0, ushort palette = 0)
        {
            Drawable = drawable;
            Texture = texture;
        }

        public Prop(Dictionary<string, string> datas, string name)
        {
            Drawable = ushort.Parse(datas[name + "_drawable"]);
            Texture = ushort.Parse(datas[name + "_texture"]);
        }

        public Dictionary<string, string> Data(string name)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[name + "_drawable"] = Drawable.ToString();
            data[name + "_texture"] = Texture.ToString();
            return data;
        }

        public bool Equals(Prop prop)
        {
            if (Drawable != prop.Drawable) return false;
            if (Texture != prop.Texture) return false;
            return true;
        }
        public static bool Equals(Component prop1, Component prop2)
        {
            if (prop1.Drawable != prop2.Drawable) return false;
            if (prop1.Texture != prop2.Texture) return false;
            return true;
        }
    }
}
