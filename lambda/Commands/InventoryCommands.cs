using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Argent(Player player, object[] argv)
        {
            return new CmdReturn($"Vous avez {player.Inventory.Money} $");
        }
        [Command(Command.CommandType.INVENTORY, 3)]
        [Syntax("Joueur", "Item", "Nombre")]
        [SyntaxType(typeof(Player), typeof(Item), typeof(uint))]
        public static CmdReturn Donner(Player player, object[] argv)
        {
            Player p = (Player)argv[0];
            Item item = (Item)argv[1];
            uint amount = (uint)argv[2];
            if (p == player) return new CmdReturn("Vous ne pouvez pas vous cibler vous meme", CmdReturn.CmdReturnType.WARNING);
            if (p.GetRequest() != null) return CmdReturn.RequestBusy;
            if (item.Amount < amount) return new CmdReturn("Vous n'avez pas assez d'objets", CmdReturn.CmdReturnType.WARNING);
            Request request = new Request(p, "Don", $"{player.FullName} veux vous donner un objet", player);
            request.AddAnswer("Accepter", (sender, receiver) =>
            {
                sender.SendMessage($"{receiver.FullName} a accepté votre demande");
                sender.Inventory.RemoveItem(item.GetBaseItem().Id, amount);
                receiver.Inventory.AddItem(item.GetBaseItem().Id, amount);
            });
            request.AddAnswer("Refuser", (sender, receiver) =>
            {
                sender.SendMessage($"{receiver.FullName} a refusé votre demande");
            });
            request.Condition = (sender, receiver) =>
            {
                if (sender.Inventory.GetItem(item.GetBaseItem().Id).Amount < amount)
                {
                    sender.SendMessage($"Vous n avez pas assez d'objets");
                    receiver.SendMessage($"L'envoyeur n a pas assez d'objets");
                    return false;
                }
                if (sender.Position.Distance(receiver.Position) > 3)
                {
                    sender.SendMessage($"Vous être trop loin");
                    receiver.SendMessage($"Vous être trop loin");
                    return false;
                }
                return true;
            };
            p.SendRequest(request);
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
        [Command(Command.CommandType.INVENTORY, 2)]
        [Syntax("Objet", "Nombre")]
        [SyntaxType(typeof(uint), typeof(uint))]
        public static CmdReturn Objet_Jeter(Player player, object[] argv)
        {
            uint slot = (uint)argv[0];
            uint amount = (uint)argv[1];
            if (player.Inventory.Items.Count <= slot) return new CmdReturn("Index incorrect");
            player.Inventory.RemoveItem(slot, amount);
            return new CmdReturn($"Vous avez jeté un objet");
        }
        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Utiliser(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            return new CmdReturn($"Vous avez jeté un objet");
        }
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Deshabiller(Player player, object[] argv)
        {
            player.Inventory.AddItem(Enums.Items.Clothes, 1, player.Skin.Clothes.ToString());
            //player.Skin.Clothes.Set("");
            return new CmdReturn($"Vous vous êtes déshabillé");
        }
    }
}
