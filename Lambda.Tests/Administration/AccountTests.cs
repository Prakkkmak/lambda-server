using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Lambda.Database;
using Lambda.Tests.Database;
using Lambda.Administration;

namespace Lambda.Tests.Administration
{
    public class AccountTests
    {
        [Fact]
        public void Insert_ShouldWork()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();

            //Act
            int actual = account.Insert("pass", dbConnect);

            //Assert
            Assert.True(actual > 0);
        }

        [Fact]
        public void Update_ShouldWork()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();

            //Act
            int actual = account.Update(dbConnect);

            //Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void Delete_ShouldWork()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = new Account();

            //Act
            int actual = account.Delete(dbConnect);

            //Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void Select_ShouldWork()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();

            //Act
            Dictionary<string, string> actual = Account.Select(0, dbConnect);

            //Assert
            Assert.NotNull(actual);
        }
        [Fact]
        public void Select_ShouldWorkWithMail()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();


            //Act
            Dictionary<string, string> actual = Account.Select("test@example.fr", "password123", dbConnect);

            //Assert
            Assert.True(actual.Count > 0);
        }
        [Fact]
        public void Select_ShouldFailWithMail()
        {
            //Arrange
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();


            //Act
            Dictionary<string, string> actual = Account.Select("false@notwork.fr", "samarchpa", dbConnect);

            //Assert
            Assert.False(actual.Count > 0);
        }

        [Fact]
        public void LogIn_ShouldWork()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = Account.LogIn("test@example.fr", "password123", dbConnect);
            Assert.NotNull(account);
        }

        [Fact]
        public void LogIn_ShouldFail()
        {
            DBConnect dbConnect = DBConnectTests.OpenTestDbConnect();
            Account account = Account.LogIn("false@notwork.fr", "samarchpa", dbConnect);
            Assert.Null(account);
        }



    }
}
