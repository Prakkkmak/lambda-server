using Lambda.Administration;
using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    class TicketCommands
    {
        [Permission("CIVIL_CREER_TICKET")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Ticket_Creer(Player player, object[] argv)
        {
            string text = (string)argv[0];
            Ticket ticket = new Ticket(player, text);
            ticket.Notify(1);
            return new CmdReturn("Vous avez créé un ticket!");
        }
        [Permission("MODERATEUR_TICKET_NIVEAU")]
        [Command(Command.CommandType.ADMIN, 1)]
        [Syntax("Niveau")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Ticket_Niveau(Player player, object[] argv)
        {
            uint level = (uint)argv[0];
            player.TicketLevel = level;
            return new CmdReturn("Vous recevez désormais les ticket de niveau " + level + ".");
        }
        [Permission("MODERATEUR_TICKETS")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Ticket_Liste(Player player, object[] argv)
        {
            string str = "Voici la liste des tickets: <br>";
            foreach(Ticket ticket in Ticket.Tickets)
            {
                str += "[" + ticket.Id + "]" + ticket.Text;
            }
            return new CmdReturn(str);
        }
        [Permission("MODERATEUR_TICKET_PRENDRE")]
        [Command(Command.CommandType.ADMIN,1)]
        [Syntax("Id")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Ticket_Prendre(Player player, object[] argv)
        {
            uint ticketId = (uint)argv[0];
            Ticket ticket = Ticket.GetTicket(ticketId);
            if (Ticket.GetTicket(player) != null) return new CmdReturn("Vous êtes déjà sur un ticket", CmdReturn.CmdReturnType.WARNING);
                if (ticket == null) return new CmdReturn("Aucun ticket trouvé", CmdReturn.CmdReturnType.WARNING);
            if(ticket.TakedBy != null) return new CmdReturn("Ce ticket est déjà pris", CmdReturn.CmdReturnType.WARNING);
            
            ticket.TakedBy = player;
            Ticket.SendMessage(player.FullName + " a pris le ticket " + ticket.Id + ".");
            return new CmdReturn("Vous avez pris un ticket");
        }
        [Permission("MODERATEUR_TICKET_REMONTER")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Ticket_Remonter(Player player, object[] argv)
        {
            Ticket ticket = Ticket.GetTicket(player);
            if (ticket == null) return new CmdReturn("Aucun ticket trouvé", CmdReturn.CmdReturnType.WARNING);
            ticket.Upgrade(true);
            ticket.TakedBy = null;
            Ticket.SendMessage(player.FullName + " a remonté le ticket " + ticket.Id + " au niveau " + ticket.Level + ".");
            return new CmdReturn("Vous avez remonté le ticket");
        }

        [Permission("CIVIL_TICKET_FERMER")]
        [Command(Command.CommandType.ADMIN)]
        public static CmdReturn Ticket_Fermer(Player player, object[] argv)
        {
            Ticket ticket = Ticket.GetTicket(player);
            if (ticket == null) return new CmdReturn("Aucun ticket trouvé", CmdReturn.CmdReturnType.WARNING);
            if (ticket.Owner != null) ticket.Owner.SendMessage("Votre ticket vient d'etre fermé"); 
            if(ticket.TakedBy != null) ticket.TakedBy.SendMessage("Votre ticket vient d'etre fermé");
            Ticket.RemoveTicket(ticket.Id);
            Ticket.SendMessage(player.FullName + " a fermé le ticket " + ticket.Id + ".");
            return new CmdReturn("Vous avez fermé un tticket");
        }

    }
}
