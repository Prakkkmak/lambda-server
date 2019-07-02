using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;

namespace Lambda.Utils
{
    class Context
    {
        public string Name = "Action";
        public string CommandName = "";
        public List<Context> Children = new List<Context>();

        public Context() { }

        public Context(string name = "Action", string commandName = "")
        {
            Name = name;
            CommandName = commandName;
        }

        public override string ToString()
        {

            string others = "";
            foreach (Context context in Children)
            {
                others += context.ToString();
                if (Children.FindLast((elem) => true) != context) others += ",";
            }

            string str = "{" +
                         "\"label\":\"" + Name + "\"," +
                         "\"cmd\":\"/" + CommandName + "\"," +
                         "\"children\":["
                         + others
                         + "]}";
            return str;
        }
    }
}
