using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Administration
{
    class Ticket
    {
        public uint Id = 0;
        public Player Owner = null;
        public Player TakedBy = null;

        public string Text = "";

        public ushort Level = 1;

        public Ticket(Player owner, string text)
        {
            Owner = owner;
            Text = text;
            uint i = 0;
            while (GetTicket(i++) != null) ;
            Id = i - 1;
            Tickets.Add(this);
        }

        public void Notify(int level)
        {
            if (level > 4) return;
            bool notified = false;
            foreach(Player player in Player.Players)
            {
                if(player.TicketLevel == level)
                {
                    player.SendMessage("Le ticket " + Id + " est en attente ");
                    notified = true;
                }
            }
            if (notified == false) Notify(level + 1);
        }

        public void Upgrade(bool notify = true)
        {
            if(Level < 4)
            {
                Level++;
                if(notify)Notify(Level);
            }
        }

        public static void SendMessage(string text,uint level = 1)
        {
            foreach (Player player in Player.Players)
            {
                if (player.TicketLevel >= level)
                {
                    player.SendMessage(text);
                }
            }
        }

        public static void RemoveTicket(uint id)
        {
            Ticket ticket = GetTicket(id);
            if (ticket == null) return;
            if(ticket.Owner!= null) ticket.Owner.SendMessage("Votre ticket a été fermé.");
            if(ticket.TakedBy != null) ticket.TakedBy.SendMessage("Le ticket " + id + " a été fermé.");
            Tickets.Remove(ticket);
        }

        public static Ticket GetTicket(uint id)
        {
            foreach (Ticket ticket in Tickets)
            {
                if (ticket.Id == id) return ticket;
            }
            return null;
        }
        public static Ticket GetTicket(Player pair)
        {
            foreach(Ticket ticket in Tickets)
            {
                if (ticket.Owner == pair) return ticket;
                if (ticket.TakedBy == pair) return ticket;
             }
            return null;
        }

        public static List<Ticket> Tickets = new List<Ticket>();

    }
}
