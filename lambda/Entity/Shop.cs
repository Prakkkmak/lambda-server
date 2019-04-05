using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net.Data;
using Lambda.Commands;
using Lambda.Items;

namespace Lambda.Entity
{
    class Shop : Area
    {
        public List<Sell> Sells { get; set; }


        public Shop() : base(2, 1, AreaType.SHOP)
        {
            Sells = new List<Sell>();
            CheckpointTypeId = 2;
        }

        public Shop(Dictionary<string, string> datas) : base(datas)
        {
            Sells = new List<Sell>();
            CheckpointTypeId = 2;
            Type = AreaType.SHOP;
        }

        public void AddSell(uint id, int price)
        {
            Sell sell = GetSell(id);
            if (sell.ItemId == 0)
            {
                Sells.Add(new Sell(id, price));
            }
            else
            {
                ModifySell(id, price);
            }
        }

        public void RemoveSell(uint id)
        {
            Sell sell = GetSell(id);
            if (sell.ItemId != 0)
            {
                Sells.Remove(sell);
            }
        }

        public void ModifySell(uint id, int price)
        {
            for (int i = 0; i < Sells.Count; i++)
            {
                if (Sells[i].ItemId == id)
                {
                    Sells[i] = new Sell(Sells[i].ItemId, price);
                }
            }
        }
        public Sell GetSell(uint id)
        {
            foreach (Sell sell in Sells)
            {
                if (sell.ItemId == id) return sell;
            }

            return new Sell(0, 0);
        }

        public CmdReturn Sell(uint id, uint amount, Inventory inv)
        {
            Sell sell = GetSell(id);
            if (sell.ItemId == 0) return CmdReturn.ObjectNotExist;
            uint price = (uint)sell.Price * amount;
            if (!inv.Withdraw(price)) return CmdReturn.NoEnoughMoney;
            if (!inv.AddItem(id, amount)) return CmdReturn.NoSpaceInInventory;
            return CmdReturn.Success;
        }

        public override string GetMetaData()
        {
            string str = "";
            foreach (Sell sell in Sells)
            {
                str += $"{sell.ItemId}:{sell.Price},";
            }
            if (str.Length > 0) str.Remove(str.Length - 1);
            return str;
        }


    }
}
