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

        public void Set(string str)
        {
            string[] s = str.Split(',');
            Drawable = Convert.ToUInt32(s[0]);
            Texture = Convert.ToUInt32(s[1]);
        }

        public override string ToString()
        {
            return Drawable + "," + Texture;
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
