using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Lambda.Database;

namespace Lambda.Administration
{
    public class Account
    {

        private short note;
        private int admin;
        private uint hoursPlayed;

        public uint Id { get; set; }
        public string Mail { get; set; }


        public Account(string mail)
        {
            Mail = mail;
            Id = 0;
            note = 0;
            admin = 0;
        }

        public Account(Dictionary<string, string> datas)
        {
            Id = uint.Parse(datas["acc_id"]);
            Mail = datas["acc_mail"];
            hoursPlayed = uint.Parse(datas["acc_hoursplayed"]);
            note = short.Parse(datas["acc_note"]);
            admin = short.Parse(datas["acc_admin"]);
        }


        public void Register(string password)
        {
            Id = (uint)Insert(password);
        }

        public static Account LogIn(string mail, string password)
        {
            Account account = Account.GetAccount(mail, password);
            return account;
        }

        #region database
        private Dictionary<string, string> GetPlayerData(string password)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["acc_mail"] = Mail;
            datas["acc_password"] = password;
            datas["acc_hoursplayed"] = hoursPlayed.ToString();
            datas["acc_note"] = note.ToString();
            datas["acc_admin"] = admin.ToString();
            return datas;
        }
        private Dictionary<string, string> GetPlayerData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["acc_mail"] = Mail;
            datas["acc_hoursplayed"] = hoursPlayed.ToString();
            datas["acc_note"] = note.ToString();
            datas["acc_admin"] = admin.ToString();
            return datas;
        }

        private long Insert(string password)
        {
            Dictionary<string, string> data = GetPlayerData(password);
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Insert(TableName, data);
        }

        private int Update()
        {
            Dictionary<string, string> datas = GetPlayerData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["acc_id"] = Id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Update(TableName, datas, wheres);
        }

        private static Account GetAccount(string mail, string password)
        {
            DBConnect dbConnect = DBConnect.DbConnect;
            Dictionary<string, string> datas = Select(mail, password);
            return datas.Count > 0 ? new Account(datas) : null;
        }

        private static Dictionary<string, string> Select(int id)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["acc_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        private static Dictionary<string, string> Select(string mail, string password)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["acc_mail"] = mail;
            wheres["acc_password"] = password;
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        #endregion
        public static string TableName = "t_account_acc";
    }
}
