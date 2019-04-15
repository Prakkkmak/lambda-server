using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Administration;

namespace Lambda.Database
{
    public class DbAccount : DbElement<Account>
    {
        public DbAccount(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Account account)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["acc_mail"] = account.Mail;
            data["acc_hoursplayed"] = account.hoursPlayed.ToString();
            data["acc_note"] = account.note.ToString();
            data["acc_admin"] = account.admin.ToString();
            data["acc_license"] = account.License;
            return data;
        }

        public override void SetData(Account account, Dictionary<string, string> data)
        {
            account.Id = uint.Parse(data["acc_id"]);
            account.Mail = data["acc_mail"];
            account.hoursPlayed = uint.Parse(data["acc_hoursplayed"]);
            account.note = short.Parse(data["acc_note"]);
            account.admin = short.Parse(data["acc_admin"]);
            account.License = data["acc_license"];
        }
        public Account Get(string license)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            where[Prefix + "_license"] = license;
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return null;
            Account account = new Account();
            SetData(account, result);
            return account;
        }
        /*public Account Get(string mail)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            where[Prefix + "_mail"] = mail;
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return null;
            Account account = new Account();
            SetData(account, result);
            return account;
        }
        public Account Get(string mail, string password)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            where[Prefix + "_mail"] = mail;
            where[Prefix + "_password"] = password;
            Dictionary<string, string> result = DbConnect.SelectOne(TableName, where);
            if (result.Count == 0) return null;
            Account account = new Account();
            SetData(account, result);
            return account;
        }*/
        public void Save(Account account, string password)
        {
            Dictionary<string, string> data = GetData(account);
            data[Prefix + "_password"] = password;
            if (account.Id == 0)
            {
                account.Id = (uint)DbConnect.Insert(TableName, data);
            }
            else
            {
                Dictionary<string, string> where = new Dictionary<string, string>();
                where[Prefix + "_id"] = account.Id.ToString();
                DbConnect.Update(TableName, data, where);
            }
        }

    }
}
