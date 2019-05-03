using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class SyntaxTypeAttribute : Attribute
    {
        public Type[] Types;

        public SyntaxTypeAttribute(params Type[] types)
        {
            this.Types = types;
        }
    }
}
