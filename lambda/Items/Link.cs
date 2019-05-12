using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Lambda.Database;

namespace Lambda.Items
{
    public class Link : IDBElement, IEquatable<Link>
    {
        public enum LinkType
        {
            UNKNOW,
            VALID,
            UNVALID
        }
        public uint Id { get; set; }
        public Dictionary<string, string> GetData()
        {
            throw new NotImplementedException();
        }



        public Tuple<byte, ushort> Component1;
        public Tuple<byte, ushort> Component2;
        public LinkType Type;

        public Link()
        {
            Id = 0;
            Type = LinkType.UNKNOW;
        }
        public Link(byte from, byte to, ushort fromd, ushort tod, LinkType type = LinkType.UNKNOW) : this()
        {
            Component1 = new Tuple<byte, ushort>(from, fromd);
            Component2 = new Tuple<byte, ushort>(to, tod);
            Type = type;
        }

        public bool Equals(Link link)
        {
            if (!Component1.Equals(link.Component1)) return false;
            if (!Component2.Equals(link.Component2)) return false;
            return true;
        }
        public override int GetHashCode()
        {
            int code = Component1.Item1 ^ Component1.Item2 + 10;

            //Calculate the hash code for the product. 
            //Alt.Log(code + "");
            return code;
        }

        public static bool GetCompatibility(params Link[] links)
        {
            for (int index = 0; index < links.Length; index++)
            {
                Link link = links[index];
                for (int i = index + 1; i < links.Length; i++)
                {
                    Link link1 = links[i];
                    if (link1.Component1.Item1 == link.Component1.Item1)
                    {
                        if (link1.Component1.Item2 != link.Component1.Item2) return false;
                    }

                    if (link1.Component1.Item1 == link.Component2.Item1)
                    {
                        if (link1.Component1.Item2 != link.Component2.Item2) return false;
                    }

                    if (link1.Component2.Item1 == link.Component1.Item1)
                    {
                        if (link1.Component2.Item2 != link.Component1.Item2) return false;
                    }

                    if (link1.Component2.Item1 == link.Component2.Item1)
                    {
                        if (link1.Component2.Item2 != link.Component2.Item2) return false;
                    }
                }
            }

            return true;
        }
        public static Tuple<byte, byte>[] LinksPairs =
        {
            new Tuple<byte, byte>(1, 2),
            new Tuple<byte, byte>(1, 11),
            new Tuple<byte, byte>(3, 8),
            new Tuple<byte, byte>(3, 11),
            new Tuple<byte, byte>(4, 6),
            new Tuple<byte, byte>(4, 8),
            new Tuple<byte, byte>(4, 11),
            new Tuple<byte, byte>(8, 11),
        };

        public void SetData(Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
