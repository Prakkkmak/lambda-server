using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{
    class TaxiCommands
    {
        [Permission("TAXI_DUTY")]
        [Command(Command.CommandType.TAXI)]
        public static CmdReturn Taxi_Service(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            if (player.Vehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);
            foreach (Player player1 in Player.Players)
            {
                player1.SendMessage("Un taxi est en service ! Appelez le " + player.Phone.Number + " pour le contacter.");
            }
            return new CmdReturn("Vous vous êtes mis en service!");
        }
        [Permission("TAXI_TAXIMETER_BEGIN")]
        [Command(Command.CommandType.TAXI)]
        public static CmdReturn Taxi_Compteur_Demarrer(Player player, object[] argv)
        {
            if (player.Vehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);
            Vehicle vehicle = (Vehicle)player.Vehicle;
            vehicle.TaxiMeter = DateTime.Now; // 16:51
            return new CmdReturn("Vous avez démarré votre compteur.");
        }
        [Permission("TAXI_TAXIMETER_SHOW")]
        [Command(Command.CommandType.TAXI)]
        public static CmdReturn Taxi_Compteur_Voir(Player player, object[] argv)
        {
            if (player.Vehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);
            Vehicle vehicle = (Vehicle)player.Vehicle;
            if (vehicle.TaxiMeter == default) return new CmdReturn("Aucun compteur n'est activé", CmdReturn.CmdReturnType.WARNING);
            DateTime current = DateTime.Now;
            long spend = (current - vehicle.TaxiMeter).Ticks / TimeSpan.TicksPerSecond;
            long minutes = spend / 60;
            long seconds = spend % 60;
            return new CmdReturn("Vous avez passé " + minutes + "m" + seconds + "s avec le compteur allumé. Prix: " + (minutes * vehicle.TaxiPrice) + "$.");
        }
        [Permission("TAXI_TAXIMETER_PRICE")]
        [Command(Command.CommandType.TAXI, 1)]
        [Syntax("Prix")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Taxi_Compteur_Prix(Player player, object[] argv)
        {
            if (player.Vehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);
            Vehicle vehicle = (Vehicle)player.Vehicle;
            uint price = (uint)argv[0];
            vehicle.TaxiPrice = price;
            return new CmdReturn("Vous avez passé le prix à " + price + ".");
        }
        [Permission("TAXI_TAXIMETER_STOP")]
        [Command(Command.CommandType.TAXI)]
        public static CmdReturn Taxi_Compteur_Arreter(Player player, object[] argv)
        {
            if (player.Vehicle == null) return new CmdReturn("Vous n'etes pas dans un véhicule", CmdReturn.CmdReturnType.WARNING);
            Vehicle vehicle = (Vehicle)player.Vehicle;
            vehicle.TaxiMeter = default;
            return new CmdReturn("Vous avez désactivé le compteur du taxi.");
        }


    }
}
