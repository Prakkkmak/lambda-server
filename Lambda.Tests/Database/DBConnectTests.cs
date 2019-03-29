using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lambda;
using Lambda.Database;
using Xunit;
namespace Lambda.Tests.Database
{
    public class DBConnectTests
    {
        public static DBConnect OpenTestDbConnect()
        {
            string server = "localhost";
            string port = "3307";
            string database = "lambda_tests";
            string uid = "root";
            string password = "";
            string connectionString =
                $"SERVER={server}; DATABASE={database}; UID={uid}; PASSWORD={password}; PORT={port}";
            DBConnect dBConnect = new DBConnect(connectionString);
            /*Insert data to table */
            string table = "t_account_acc";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["acc_mail"] = "test@example.fr";
            data["acc_password"] = "password123";
            dBConnect.Insert(table, data);
            dBConnect.Insert(table, data);
            /*  */
            return dBConnect;
        }

        [Fact]
        public void OpenConnection_TestDatabaseShouldConnect()
        {
            //Arrange
            string server = "localhost";
            string port = "3307";
            string database = "lambda_tests";
            string uid = "root";
            string password = "";
            string connectionString = string.Format("SERVER={0}; DATABASE={1}; UID={2}; PASSWORD={3}; PORT={4}", server, database, uid, password, port);
            DBConnect dBConnect = new DBConnect(connectionString);
            //Act
            bool actual = dBConnect.OpenConnection();

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void OpenConnection_WrongCredentialsShouldFailed()
        {
            //Arrange
            string server = "localhost";
            string port = "3307";
            string database = "lambda_tests";
            string uid = "root";
            string password = "wrongpassword";
            string connectionString = $"SERVER={server}; DATABASE={database}; UID={uid}; PASSWORD={password}; PORT={port}";
            DBConnect dBConnect = new DBConnect(connectionString);
            //Act
            bool actual = dBConnect.OpenConnection();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void CloseConnection_ShouldDisconnect()
        {
            //Arrange
            DBConnect dBConnect = OpenTestDbConnect();
            //Act
            dBConnect.OpenConnection();
            bool actual = dBConnect.CloseConnection();
            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void Insert_ShouldWork()
        {
            //Arrange
            DBConnect dBConnect = OpenTestDbConnect();
            string table = "t_account_acc";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["acc_mail"] = "test@example.fr";
            data["acc_password"] = "password123";
            //Act
            int actual = dBConnect.Insert(table, data);
            //Assert
            Assert.True(actual > 0);
        }
        [Fact]
        public void Update_ShouldWork()
        {
            //Arrange
            DBConnect dBConnect = OpenTestDbConnect();
            string table = "t_account_acc";
            Dictionary<string, string> datas = new Dictionary<string, string>
            {
                ["acc_mail"] = "test_replaced@example.fr",
                ["acc_password"] = "newPassword"
            };
            Dictionary<string, string> wheres = new Dictionary<string, string>
            {
                ["acc_mail"] = "test@example.fr"
            };
            //Act
            int actual = dBConnect.Update(table, datas, wheres);
            //Assert
            Assert.True(actual >= 0);
        }
        [Fact]
        public void Delete_ShouldWork()
        {
            //Arrange
            DBConnect dBConnect = OpenTestDbConnect();
            string table = "t_account_acc";
            Dictionary<string, string> wheres = new Dictionary<string, string>
            {
                ["acc_note"] = "1"
            };
            //Act
            int actual = dBConnect.Delete(table, wheres);
            //Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void SelectOne_ShouldWork()
        {
            //Arrange
            DBConnect dBConnect = OpenTestDbConnect();
            string table = "t_account_acc";
            Dictionary<string, string> wheres = new Dictionary<string, string>
            {
                ["acc_note"] = "0"
            };
            //Act
            Dictionary<string, string> actual = dBConnect.SelectOne(table, wheres);
            //Assert
            Assert.NotNull(actual);
        }



        [Fact]
        public void DataToInsertQuery_ShouldWork()
        {
            string table = "table";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["dat1"] = "val1";
            data["dat2"] = "val2";
            string excepted = "INSERT INTO table (`dat1`,`dat2`) VALUES ('val1','val2');";
            string actual = DBConnect.DataToInsertQuery(table, data);
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void DataToUpdateQuery_ShouldWork()
        {
            string table = "table";
            Dictionary<string, string> datas = new Dictionary<string, string>();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            datas["dat1"] = "val1";
            datas["dat2"] = "val2";
            datas["dat3"] = "val3";
            wheres["dat1"] = "val1";
            wheres["datid"] = "0";
            string excepted = "UPDATE table SET `dat1` = 'val1' , `dat2` = 'val2' , `dat3` = 'val3' WHERE `dat1` = 'val1' AND `datid` = '0';";
            string actual = DBConnect.DataToUpdateQuery(table, datas, wheres);
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void DataToDeleteQuery_ShouldWork()
        {
            string table = "table";
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["dat1"] = "val1";
            wheres["datid"] = "0";
            string excepted = "DELETE FROM table WHERE `dat1` = 'val1' AND `datid` = '0';";
            string actual = DBConnect.DataToDeleteQuery(table, wheres);
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void DataToSelectQuery_ShouldWork()
        {
            string table = "table";
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            string[] fields = { "dat1" };
            wheres["dat1"] = "val1";
            wheres["datid"] = "0";
            string excepted = "SELECT dat1 FROM table WHERE `dat1` = 'val1' AND `datid` = '0';";
            string actual = DBConnect.DataToSelectQuery(table, wheres, fields);
            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void DataToSelectQuery_ShouldWorkWithoutFields()
        {
            string table = "table";
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["dat1"] = "val1";
            wheres["datid"] = "0";
            string excepted = "SELECT * FROM table WHERE `dat1` = 'val1' AND `datid` = '0';";
            string actual = DBConnect.DataToSelectQuery(table, wheres);
            Assert.Equal(excepted, actual);
        }



        [Theory]
        [InlineData("", "dat1", "val1", "table")]
        [InlineData("table", "", "val1", "datas")]
        [InlineData("table", "dat1", "", "datas")]
        public void DataToInsertQuery_ShouldFail(string table, string dat, string val, string param)
        {
            Dictionary<string, string> datas = new Dictionary<string, string> { [dat] = val };
            Assert.Throws<ArgumentException>(param, () => DBConnect.DataToInsertQuery(table, datas));
        }


        [Theory]
        [InlineData("", "dat1", "val1", "whk", "whv", "table")]
        [InlineData("table", "", "val1", "whk", "whv", "datas")]
        [InlineData("table", "dat1", "", "whk", "whv", "datas")]
        [InlineData("table", "dat1", "val1", "", "whv", "wheres")]
        [InlineData("table", "dat1", "val1", "whk", "", "wheres")]
        public void DataToUpdateQuery_ShouldFail(string table, string dat, string val, string whereKey, string whereVal, string param)
        {
            Dictionary<string, string> datas = new Dictionary<string, string> { [dat] = val };
            Dictionary<string, string> wheres = new Dictionary<string, string> { [whereKey] = whereVal };
            Assert.Throws<ArgumentException>(param, () => DBConnect.DataToUpdateQuery(table, datas, wheres));
        }

        [Theory]
        [InlineData("", "whk", "whv", "table")]
        [InlineData("table", "", "whv", "wheres")]
        [InlineData("table", "whk", "", "wheres")]
        public void DataToDeleteQuery_ShouldFail(string table, string whereKey, string whereVal, string param)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string> { [whereKey] = whereVal };
            Assert.Throws<ArgumentException>(param, () => DBConnect.DataToDeleteQuery(table, wheres));
        }

        [Theory]
        [InlineData("", "whk", "whv", "whk1", "whk2", "table")]
        [InlineData("table", "", "whv", "whk1", "whk2", "wheres")]
        [InlineData("table", "whk", "", "whk1", "whk2", "wheres")]
        [InlineData("table", "whk", "whv", "", "whk2", "fields")]
        public void DataToSelectQuery_ShouldFail(string table, string whereKey, string whereVal, string field1, string field2, string param)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string> { [whereKey] = whereVal };
            string[] fields = { field1, field2 };
            Assert.Throws<ArgumentException>(param, () => DBConnect.DataToSelectQuery(table, wheres, fields));
        }
    }
}
