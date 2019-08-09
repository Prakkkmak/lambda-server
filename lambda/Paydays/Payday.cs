using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Paydays
{
    public class Payday
    {

        public DateTime Date = new DateTime();

        public List<Transaction> Transactions = new List<Transaction>();
        public Payday()
        {

        }
        public Payday(string str)
        {
            string[] strs = str.Split(',');
            foreach(string s in strs)
            {
                if(!string.IsNullOrEmpty(s))Transactions.Add(new Transaction(s));
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach(Transaction transaction in Transactions)
            {
                str += transaction + ",";
            }
            return str;
        }
        public Transaction GetTransaction(Enums.TransactionGroups Group, string param = "")
        {
            foreach(Transaction tr in Transactions)
            {
                if (tr.Group == Group && (string.IsNullOrEmpty(param) || tr.Parameter.Equals(param))) return tr;
            }
            return null;
        }

    }
}
