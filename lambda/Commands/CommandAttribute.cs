using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class CommandAttribute : Attribute
    {
        public Command.CommandType Type;
        public ushort Length; // The number of parameters after the command name

        public CommandAttribute(Command.CommandType type, ushort length = 0)
        {
            this.Type = type;
            this.Length = length;
        }
    }
}
