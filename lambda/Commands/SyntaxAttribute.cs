using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class SyntaxAttribute : Attribute
    {
        public string[] Attributes;

        public SyntaxAttribute(params string[] attributes)
        {
            this.Attributes = attributes;
        }
    }
}
