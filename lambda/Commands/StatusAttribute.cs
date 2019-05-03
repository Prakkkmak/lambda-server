using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class StatusAttribute : Attribute
    {
        public Command.CommandStatus Status;

        public StatusAttribute(Command.CommandStatus status)
        {
            Status = status;
        }
    }
}
