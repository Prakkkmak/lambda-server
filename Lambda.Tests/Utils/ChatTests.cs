using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Utils;
using Xunit;

namespace Lambda.Tests.Utils
{
    public class ChatTests
    {
        [Fact]
        public void InvokeCmd_ShouldWork()
        {
            Command cmd = new Command("test", (
                (account1, parameters) => new CmdReturn("test_return", CmdReturn.CmdReturnType.SUCCESS)),
                Command.CommandType.TEST, new[] { "arg1" });
            Command.Commands.Add(cmd);
            Account account = new Account();
            string msg = "/test arg1";
            CmdReturn actual = Chat.InvokeCmd(account, msg);
            Assert.Equal(CmdReturn.CmdReturnType.SUCCESS, actual.Type);
        }
    }
}
