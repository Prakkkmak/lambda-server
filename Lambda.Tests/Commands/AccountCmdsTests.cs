using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Tests.Database;
using Xunit;

namespace Lambda.Tests.Commands
{
    public class AccountCmdsTests
    {
        /*[Fact]
        public void Register_ShouldWork()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[4];
            argv[0] = "register";
            argv[1] = "toto@gmail.fr";
            argv[2] = "password456";
            argv[3] = "password456";
            CmdReturn.CmdReturnType actual = AccountCmds.Register(account, argv, dbConnect).Type;
            CmdReturn.CmdReturnType excepted = CmdReturn.CmdReturnType.SUCCESS;
            Assert.Equal(excepted, actual);
        }
        [Fact]
        public void Register_ShouldFailNotEnoughParams()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[1];
            Assert.Throws<ArgumentException>("argv", () => AccountCmds.Register(account, argv, dbConnect));
        }

        [Theory]
        [InlineData("noat", "password", "password", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("@t", "password", "password", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("test@test.fr", "short", "short", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("test@test.fr", "password", "diff", CmdReturn.CmdReturnType.SYNTAX)]
        public void Register_ShouldFail(string mail, string password, string verifPassword, CmdReturn.CmdReturnType excepted)
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[4];
            argv[0] = "register";
            argv[1] = mail;
            argv[2] = password;
            argv[3] = verifPassword;
            CmdReturn.CmdReturnType actual = AccountCmds.Register(account, argv, dbConnect).Type;
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void Connect_ShouldWork()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[3];
            argv[0] = "register";
            argv[1] = "test@example.fr";
            argv[2] = "password123";
            CmdReturn.CmdReturnType actual = AccountCmds.Connect(account, argv, dbConnect).Type;
            CmdReturn.CmdReturnType excepted = CmdReturn.CmdReturnType.SUCCESS;
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void Connect_ShouldFailNotEnoughParams()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[1];
            Assert.Throws<ArgumentException>("argv", () => AccountCmds.Connect(account, argv, dbConnect));
        }

        [Theory]
        [InlineData("noat", "password", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("@t", "password", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("test@test.fr", "short", CmdReturn.CmdReturnType.SYNTAX)]
        [InlineData("wrongmail@test.fr", "passworldiswrong", CmdReturn.CmdReturnType.WARNING)]
        public void Connect_ShouldFail(string mail, string password, CmdReturn.CmdReturnType excepted)
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();
            string[] argv = new string[3];
            argv[0] = "connect";
            argv[1] = mail;
            argv[2] = password;
            CmdReturn.CmdReturnType actual = AccountCmds.Connect(account, argv, dbConnect).Type;
            Assert.Equal(excepted, actual);
        }*/
    }
}
