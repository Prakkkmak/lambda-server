using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Lambda.Administration;
using Xunit;
using Lambda.Commands;
using Lambda.Tests.Entity;
using Lambda.Utils;
using Xunit.Sdk;

namespace Lambda.Tests.Commands
{
    public class CommandTests
    {
        [CommandAttribute(Command.CommandType.TEST, "arg1", "arg2")]
        public static CmdReturn CmdTest(Account account, string[] argv)
        {
            string text = "text";
            foreach (string s in argv)
            {
                text += $" {s}";
            }
            return new CmdReturn(text);
        }

        [Theory]
        [InlineData("[SYNTAXE]/cmdtest [arg1] [arg2]", "cmdtest", "arg1", "arg2")]
        [InlineData("[SYNTAXE]/cmdtest", "cmdtest")]
        public void Syntax_ShouldWork(string excepted, string name, params string[] syntax)
        {
            Command cmd = new Command(name, null, Command.CommandType.TEST, syntax);
            string actual = cmd.Syntax().Text;
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void Syntax_ShouldFailWithNullName()
        {
            Command cmd = new Command(null, null, Command.CommandType.TEST, null);
            Assert.Throws<NullReferenceException>(() => cmd.Syntax());
        }

        [Theory]
        [InlineData("/cmdtest", "text cmdtest")]
        [InlineData("/cmdtest arg1 arg2", "text cmdtest arg1 arg2")]
        public void Execute_ShouldWork(string text, string excepted)
        {
            string name = "CmdTest";
            Command.CommandFunc commandFunc = CmdTest;
            Account account = new Account();
            Command cmd = new Command(name, commandFunc, Command.CommandType.TEST, new string[0]);
            CmdReturn cmdReturn = cmd.Execute(account, Chat.TextToArgs(text));
            string actual = cmdReturn.Text;
            Assert.Equal(excepted, actual);
        }
        [Fact]
        public void Execute_ShouldFailWithNullSyntax()
        {
            string name = "CmdTest";
            Command.CommandFunc commandFunc = CmdTest;
            Account account = new Account();
            Command cmd = new Command(name, commandFunc, Command.CommandType.TEST, null);
            Assert.Throws<NullReferenceException>(() => cmd.Execute(account, Chat.TextToArgs("/test")));
        }

        [Fact]
        public void GetCommands_ShouldWork()
        {
            Command cmd = new Command("mycmd", null, Command.CommandType.TEST, new string[0]);
            Command.Commands.Add(cmd);
            string commandName = "myc";
            Command[] actual = Command.GetCommands(commandName);

            Assert.True(actual.Length > 0);
            Assert.Equal("mycmd", actual[0].Name);
        }

        [Fact]
        public void GetCommands_ShouldFail()
        {
            Command cmd = new Command("mycmd", null, Command.CommandType.TEST, new string[0]);
            Command.Commands.Add(cmd);
            string commandName = "wrongcmdname";
            Command[] actual = Command.GetCommands(commandName);

            Assert.True(actual.Length == 0);
        }

        [Fact]
        public void RegisterCommand_ShouldWork()
        {
            Command cmd = new Command("mycmd", null, Command.CommandType.TEST, new string[0]);
            Command.RegisterCommand(cmd);
            bool actual = Command.Commands.Contains(cmd);
            Assert.True(Command.Commands.Count > 0);
            Assert.True(actual);
        }

        [Fact]
        public void GetTypesInNamespace_ShouldWork()
        {
            string namespaceName = typeof(CommandTests).Namespace;
            Type[] types = Command.GetTypesInNamespace(Assembly.GetExecutingAssembly(), namespaceName);
            Type type = typeof(CommandTests);
            bool actual = types.Contains(type);
            Assert.True(types.Length > 0);
            Assert.True(actual);
        }

        [Fact]
        public void GetTypesInNamespace_ShouldFailInvalidNameSpaceName()
        {
            string namespaceName = "InvalidNameSpaceName";
            Type[] types = Command.GetTypesInNamespace(Assembly.GetExecutingAssembly(), namespaceName);
            Type type = typeof(CommandTests);
            bool actual = types.Contains(type);
            Assert.False(types.Length > 0);
            Assert.False(actual);
        }

        [Fact]
        public void RegisterAllCommands_ShouldWork()
        {
            string namespaceName = typeof(CommandTests).Namespace;
            Command.RegisterAllCommands(namespaceName, Assembly.GetExecutingAssembly());
            Assert.True(Command.Commands.Count > 0);
        }

    }
}