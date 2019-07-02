﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AltV.Net;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda.Commands
{
    class InventoryCommands
    {
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Inventaire(Player player, object[] argv)
        {
            string str = $"Voici votre stockage ! 0/0 <br>";
            str += "Voici le contenu de votre inventaire: <br>";
            for (int i = 0; i < player.Inventory.Items.Count; i++)
            {
                Item inventoryItem = player.Inventory.Items[i];
                str += i + "-" + inventoryItem.GetBaseItem().Name + " " + "(" + inventoryItem.Amount + ")";
            }

            return new CmdReturn(str);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Utiliser(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            if (item.Use(player) == 1) return new CmdReturn("Vous ne pouvez pas utiliser ceci.");
            Alt.Log("Utilisation item");
            return new CmdReturn("Vous avez utilisé un objet");
        }
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Argent(Player player, object[] argv)
        {
            return new CmdReturn($"Vous avez {player.Inventory.Money} $");
        }
        [Command(Command.CommandType.INVENTORY, 2)]
        [Syntax("Item", "Nombre")]
        [SyntaxType(typeof(Item), typeof(uint))]
        public static CmdReturn Donner(Player player, object[] argv)
        {
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            Item item = (Item)argv[1];
            uint amount = (uint)argv[2];
            if (target.GetRequest() != null) return CmdReturn.RequestBusy;
            if (item.Amount < amount) return new CmdReturn("Vous n'avez pas assez d'objets", CmdReturn.CmdReturnType.WARNING);
            Request request = new Request(target, "Don", $"{player.FullName} veux vous donner un objet", player);
            request.AddAnswer("Accepter", () =>
            {
                player.SendMessage($"{target.FullName} a accepté votre demande");
                player.Inventory.RemoveItem(item.GetBaseItem().Id, amount);
                target.Inventory.AddItem(item.GetBaseItem().Id, amount);
            });
            request.AddAnswer("Refuser", () =>
            {
                player.SendMessage($"{target.FullName} a refusé votre demande");
            });
            request.Condition = () =>
            {
                if (player.Inventory.GetItem(item.GetBaseItem().Id).Amount < amount)
                {
                    player.SendMessage($"Vous n avez pas assez d'objets");
                    target.SendMessage($"L'envoyeur n a pas assez d'objets");
                    return false;
                }
                if (player.Position.Distance(target.Position) > 3)
                {
                    player.SendMessage($"Vous être trop loin");
                    target.SendMessage($"Vous être trop loin");
                    return false;
                }
                return true;
            };
            target.SendRequest(request);
            return new CmdReturn("Vous avez fait une demande.", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Objets(Player player, object[] argv)
        {
            string str = "Voici la liste des objets:";
            str += BaseItem.BaseItems.Aggregate("", (current, item) => current + $"[{item.Id}]{item.Name} ");
            return new CmdReturn(str);
        }

        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Objet_Info(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            return new CmdReturn($"{item.GetBaseItem().Id} {item.GetBaseItem().Name} ({item.Amount}) : {item.MetaData}");
        }
        [Command(Command.CommandType.INVENTORY, 2)]
        [Syntax("Objet", "Valeur")]
        [SyntaxType(typeof(Item), typeof(string))]
        public static CmdReturn Objet_Data_Changer(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            item.MetaData = argv[1] + "";
            return new CmdReturn("Vous avez changé le metadata de l'objet.");
        }
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Inventaire_Vider(Player player, object[] argv)
        {
            _ = player.Inventory.ClearAsync();
            return new CmdReturn("Vous avez vidé votre inventaire.");
        }
        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Objet_Jeter(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            _ = player.Inventory.RemoveItemAsync(item);
            return new CmdReturn($"Vous avez jeté un objet");
        }

        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Prix")]
        [SyntaxType(typeof(uint))]
        public static CmdReturn Payer(Player player, object[] argv)
        {
            Player target = player.PlayerSelected;
            if (target == null) return CmdReturn.NoSelected;
            uint money = (uint)argv[0];
            if (player.Inventory.Money < money) return new CmdReturn("Vous n'avez pas assez d'argent");
            player.Inventory.Money -= money;
            target.Inventory.Money += money;
            target.SendMessage("Vous avez reçu " + money);
            return new CmdReturn($"Vous avez donné " + money);
        }

    }
}
