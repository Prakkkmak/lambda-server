using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lambda.Clothing
{
    public struct Overlay
    {
        public uint Index { get; set; }
        public float Opacity { get; set; } // 0 - 1
        public uint Color1 { get; set; }
        public uint Color2 { get; set; }

        public Overlay(ushort index = 0, float opacity = 1, uint color1 = 0, uint color2 = 0)
        {
            Index = index;
            Opacity = opacity;
            Color1 = color1;
            Color2 = color2;
        }
    }
}
