using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lambda.Entity
{
    public class Lock
    {
        public enum Complexity
        {
            NUMERICAL,
            ALPHAMIN,
            ALPHAMAJ
        }

        public string Code;

        public Lock()
        {
            Code = "";
        }

        public Lock(string code) : this()
        {
            Code = code;
        }
        public Lock(int size, params Complexity[] complexities) : this()
        {
            string elems = complexities.Aggregate("", (current, complexity) => current + ComplexityString[complexity]);
            Code = Generate(elems, size);
        }

        public static string Generate(string elems, int size)
        {
            string code = "";
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                int index = r.Next(elems.Length);
                code += elems[index];
            }
            return code;
        }

        public static Dictionary<Complexity, string> ComplexityString = new Dictionary<Complexity, string>()
        {
            {Complexity.NUMERICAL, "0123456789"},
            {Complexity.ALPHAMAJ , "ABCDEFGHIJKLMNOPQRSTUVWXYZ"},
            {Complexity.ALPHAMIN , "abcdefghijklmnopqrstuvwxyz"}
        };
    }
}
