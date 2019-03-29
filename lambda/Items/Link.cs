using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Lambda.Items
{
    public struct Link
    {
        public uint From { get; set; }
        public uint To { get; set; }

        public Link(uint from, uint to)
        {
            From = from;
            To = to;
        }

        public static bool Equals(Link linkA, Link linkB)
        {
            return linkA.From == linkB.From && linkA.To == linkB.To;
        }

        public static Link HairToMask = new Link(2, 1);
        public static Link TorsoToUndershirt = new Link(3, 8);
        public static Link TorsoToTop = new Link(3, 11);
        public static Link UndershirtToTop = new Link(8, 11);
        public static Link TopToLeg = new Link(11, 4);
        public static Link FeetToLeg = new Link(6, 4);
        public static Link UndershirtToLeg = new Link(8, 4);
        public static Link MaskToTop = new Link(1, 11);

        public static Link[] Links = { HairToMask, TorsoToUndershirt, TorsoToTop, UndershirtToTop, TopToLeg, FeetToLeg, UndershirtToLeg, MaskToTop };
    }
}
