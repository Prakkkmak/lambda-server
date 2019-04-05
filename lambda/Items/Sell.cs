using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Items
{
    struct Sell
    {
        public uint ItemId { get; set; }
        public int Price { get; set; }

        public Sell(uint itemId, int price)
        {
            ItemId = itemId;
            Price = price;
        }

        public void SetPrice(int newPrice)
        {
            Price = newPrice;
        }
    }
}
