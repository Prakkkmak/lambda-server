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
            Request request = new Request(p, "Don", $"{player.Name} veux vous donner un objet", player);
            request.AddAnswer("Accepter", (sender, receiver) =>
            {
                sender.SendMessage($"{receiver.Name} a accepté votre demande");
                sender.Inventory.RemoveItem(item.GetBaseItem().Id, amount);
                receiver.Inventory.AddItem(item.GetBaseItem().Id, amount);
            });
            request.AddAnswer("Refuser", (sender, receiver) =>
            {
                sender.SendMessage($"{receiver.Name} a refusé votre demande");
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
        [Command(Command.CommandType.INVENTORY, 3)]
        [Syntax("Joueur", "Objet", "Quantité")]
        [SyntaxType(typeof(Player), typeof(BaseItem), typeof(uint))]
        public static CmdReturn Give(Player player, object[] argv)
        {

            Player target = (Player)argv[0];
            BaseItem baseItem = (BaseItem)argv[1];
            uint amount = (uint)argv[2];
            target.Inventory.AddItem(baseItem.Id, amount);
            if (amount > 10) return new CmdReturn("Ne te donne pas trop d'objets stp");
            return new CmdReturn("Vous avez donné des objets", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Objet_Info(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            return new CmdReturn($"{item.GetBaseItem().Id} {item.GetBaseItem().Name} ({item.Amount}) : {item.MetaData}");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.INVENTORY, 2)]
        [Syntax("Objet", "Valeur")]
        [SyntaxType(typeof(Item), typeof(string))]
        public static CmdReturn Objet_Data_Changer(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            item.MetaData = argv[1] + "";
            return new CmdReturn("Vous avez changé le metadata de l'objet.");
        }

        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.INVENTORY)]
        public static CmdReturn Inventaire_Vider(Player player, object[] argv)
        {
            player.Inventory.Clear();
            return new CmdReturn("Vous avez vidé votre inventaire.");
        }
        [Status(Command.CommandStatus.NEW)]
        [Command(Command.CommandType.INVENTORY, 1)]
        [Syntax("Objet")]
        [SyntaxType(typeof(Item))]
        public static CmdReturn Objet_Jeter(Player player, object[] argv)
        {
            Item item = (Item)argv[0];
            player.Inventory.RemoveItem(item.Id, item.Amount);
            return new CmdReturn($"Vous avez jeté un objet");
        }
    }
}
