using Lambda.Entity;
using Lambda.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Paydays
{
    public class Transaction
    {
        public Enums.TransactionGroups Group = Enums.TransactionGroups.Other;

        public string Parameter = "";

        public string Name = "Transaction";


        
        public long Amount
        {
            get {
                {
                    long v = Timer * AmountPerHour / 60;
                    if (v > Max) return Max;
                    else return v;  
                }
            }
        }

        public int Timer = 0; // Number of minutes for this income / outcome
        public long AmountPerHour
        {
            get {
                if(Group == Enums.TransactionGroups.Rank)
                {
                    Rank r = Rank.GetRank(Convert.ToUInt32(Parameter));
                    return r.Salary;
                }
                if(Group == Enums.TransactionGroups.Base)
                {
                    return Organization.BaseIncome;
                }
                return 0;
            }
        }
            // Number of money per hour
        public long Max
        {
            //TODO
            get { return AmountPerHour; }
        }
        public Transaction()
        {

        }
        public Transaction(string str)
        {
            string[] strs = str.Split(':');
            
            Group = (Enums.TransactionGroups)Enum.Parse(typeof(Enums.TransactionGroups), strs[0]);
            Parameter = strs[1];
            Timer = Convert.ToInt32(strs[2]);
        }

        public void Resolve(Player player)
        {

            switch (Group)
            {
                case Enums.TransactionGroups.Base:
                    if(Organization.Governement != null)Organization.Governement.BankMoney -= Amount;
                    break;
            }
            player.BankMoney += Amount;
            Timer = 0;
        }

        public override string ToString()
        {
            string str = "";
            str += $"{Group.ToString()}:";
            str += $"{Parameter}:";
            str += $"{Timer}";
            return str;
        }
        public string ToText()
        {
            string str = "";
            if (Amount < 0) str += "-";
            else str += "+";
            str += Amount + "$ ";
            switch (Group)
            {
                case Enums.TransactionGroups.Base:
                    str += "Revenu de base";
                    break;
            }
            str += "(" + Timer + "minutes)";
            return str;
        }

    }
}
