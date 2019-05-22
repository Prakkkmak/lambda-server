using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;
using Lambda.Utils;

namespace Lambda.Commands
{
    class AreaCommands
    {
        [Command(Command.CommandType.AREA, 1)]
        [Syntax("ipl")]
        [SyntaxType(typeof(string))]
        public static CmdReturn Interieur_Ajouter(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            area.InteriorLocation.Interior.AddIpl((string)argv[0]);
            area.SetLocations();
            _ = area.SaveAsync();
            return new CmdReturn("Vous avez ajouter ipl");
        }
        [Command(Command.CommandType.AREA)]
        public static CmdReturn Interieur_Raz(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            area.InteriorLocation.Interior.SetIPLs("");
            area.SetLocations();
            _ = area.SaveAsync();
            return new CmdReturn("Vous avez remis à zéro l'interieur");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.TEST, 3)]
        [Syntax("x", "y", "z")]
        [SyntaxType(typeof(float), typeof(float), typeof(float))]
        public static CmdReturn Interieur_Position(Player player, object[] argv)
        {
            Area area = (Area)Area.GetArea(player.FeetPosition);
            Position pos = area.InteriorLocation.Interior.Position;
            pos.X = (float)argv[0];
            pos.Y = (float)argv[1];
            pos.Z = (float)argv[2];
            area.InteriorLocation.Interior.Position = pos;
            area.SetLocations(area.InteriorLocation.Interior, area.Dimension);
            _ = area.SaveAsync();
            return new CmdReturn("Vous avez changé la position de l'interieur");
        }
    }
}
