using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Entity;

namespace Lambda.Commands
{
    /// <summary>
    /// A cmdReturn is a return of a command
    /// </summary>
    public struct CmdReturn
    {

        public enum CmdReturnType
        {
            DEFAULT,
            SUCCESS,
            WARNING,
            SYNTAX,
            LOCATION,
            NOTIMPLEMENTED,
            ERROR
        }
        public string Text;
        public CmdReturnType Type;

        public CmdReturn(string text = "DEFAULT", CmdReturnType type = CmdReturnType.DEFAULT)
        {
            this.Text = text;
            this.Type = type;
        }

        public static CmdReturn OnlyOnePlayer(Player[] players)
        {
            if (players.Length > 1) return new CmdReturn("Plusieurs joueurs ont le même nom; specifiez", CmdReturnType.WARNING);
            if (players.Length < 1) return new CmdReturn("Aucun joueur trouvé", CmdReturnType.WARNING);
            return new CmdReturn("Player trouvé", CmdReturnType.SUCCESS);
        }
        public static CmdReturn Success = new CmdReturn("Success", CmdReturnType.SUCCESS);
        public static CmdReturn ObjectNotExist = new CmdReturn("Cet objet n existe pas.", CmdReturnType.WARNING);
        public static CmdReturn NoEnoughMoney = new CmdReturn("Vous n avez pas assez d'argent.", CmdReturnType.WARNING);
        public static CmdReturn NoSpaceInInventory = new CmdReturn("Vous n avez pas assez de place dans votre inventaire", CmdReturnType.WARNING);
        public static CmdReturn NoPlayerAttached = new CmdReturn("Aucun player vous est attaché. Merci de contacter un admin.", CmdReturnType.ERROR);
        public static CmdReturn NotExceptedError = new CmdReturn("Erreur non attendue, contactez un admin.", CmdReturnType.ERROR);
        public static CmdReturn NotInVehicle = new CmdReturn("Vous n etes pas dans un véhicule", CmdReturnType.LOCATION);

    }
}
