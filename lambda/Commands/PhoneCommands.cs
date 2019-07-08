using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Lambda.Telephony;
using Player = Lambda.Entity.Player;

namespace Lambda.Commands
{
    class PhoneCommands
    {
        [Permission("CIVIL_TELEPHONE_TELEPHONER")]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Contact")]
        [SyntaxType(typeof(Contact))]
        public static CmdReturn Telephone_Telephoner(Player player, object[] argv)
        {
            Contact contact = (Contact)argv[0];
            string number = contact.PhoneNumber;
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone.");
            if (Phone.GetPhone(number) == null) return new CmdReturn("Tonalité vide");
            if (Phone.GetPhone(number).Player == null) return new CmdReturn("Ca ne répponds pas");
            player.Phone.SendCall(number);
            return new CmdReturn("Vous tentez de contacter un téléphone");
        }
        [Permission("CIVIL_TELEPHONE_DECCROCHER")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Deccrocher(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone.");
            if (player.Phone.Calling == null) return new CmdReturn("Personne ne vous appelle");
            player.Phone.PickUp();
            player.Phone.Calling.Player.SendMessage("Machin a déccrocher");
            return new CmdReturn("Vous déccrochez");
        }
        [Permission("CIVIL_TELEPHONE_RACCROCHER")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Raccrocher(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone.");
            if (player.Phone.VoiceChannel == null) return new CmdReturn("Vous n'etes pas en appel");
            player.Phone.Calling.Player.SendMessage("Machin a raccroché");
            player.Phone.PickOff();
            return new CmdReturn("Vous raccrochez");
        }

        [Permission("TESTEUR_TELEPHONE_OBTENIR")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Obtenir(Player player, object[] argv)
        {
            if (player.Phone != null) return new CmdReturn("Vous avez déjà un téléphone", CmdReturn.CmdReturnType.WARNING);
            player.Phone = new Phone();
            player.Phone.Number = Phone.GeneratePhoneNumber();
            Phone.Phones.Add(player.Phone);
            player.Phone.Save();
            return new CmdReturn("Vous obtenez un téléphone");
        }
        [Permission("CIVIL_TELEPHONE_NUMERO")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Numero(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            return new CmdReturn(player.Phone.Number);
        }
        [Permission("CIVIL_TELEPHONE_MESSAGES")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Message_Liste(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            string text = "";
            foreach (Message message in player.Phone.Messages)
            {
                string target;
                if (message.From == player.Phone.Number)
                {
                    target = message.To;
                    Contact[] contacts = player.Phone.GetContacts(message.To);
                    if (contacts.Length == 1)
                    {
                        target = contacts[0].Name;
                    }

                    text += "->";
                }
                else
                {
                    target = message.From;
                    Contact[] contacts = player.Phone.GetContacts(message.From);
                    if (contacts.Length == 1)
                    {
                        target = contacts[0].Name;
                    }

                    text += "<-";
                }

                text += target + ":" + message.Body + "<br>";
            }
            return new CmdReturn(text);
        }
        [Permission("CIVIL_TELEPHONE_MESSAGE_ENVOYER")]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Contact", "Texte")]
        [SyntaxType(typeof(Contact), typeof(string))]
        public static CmdReturn Telephone_Message_Envoyer(Player player, object[] argv)
        {
            Contact contact = (Contact)argv[0];
            string number = contact.PhoneNumber;
            string text = "";

            for (int i = 1; i < argv.Length; i++)
            {
                text += (string)argv[i] + " ";
            }
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            player.Phone.SendTextMessage(new Message(0, player.Phone, player.Phone.Number, number, text));
            return new CmdReturn(player.Phone.Number);
        }
        [Permission("CIVIL_TELEPHONE_CONTACTS")]
        [Command(Command.CommandType.TEST)]
        public static CmdReturn Telephone_Contact_Liste(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            string text = "";
            foreach (Contact phoneContact in player.Phone.Contacts)
            {
                text += phoneContact.Name + " : " + phoneContact.PhoneNumber;
            }
            return new CmdReturn(text);
        }
        [Permission("CIVIL_TELEPHONE_CONTACT_AJOUTER")]
        [Command(Command.CommandType.TEST, 2)]
        [Syntax("Numero", "Texte")]
        [SyntaxType(typeof(string), typeof(string))]
        public static CmdReturn Telephone_Contact_Ajouter(Player player, object[] argv)
        {
            string number = (string)argv[0];
            string text = "";
            for (int i = 1; i < argv.Length; i++)
            {
                text += (string)argv[i] + " ";
            }
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            Contact contact = new Contact(0, player.Phone, text, number);
            player.Phone.AddContact(contact);

            return new CmdReturn("Vous avez ajouté un contact");
        }
        [Permission("CIVIL_TELEPHONE_CONTACT_SUPPRIMER")]
        [Command(Command.CommandType.TEST, 1)]
        [Syntax("Contact")]
        [SyntaxType(typeof(Contact))]
        public static CmdReturn Telephone_Contact_Supprimer(Player player, object[] argv)
        {
            if (player.Phone == null) return new CmdReturn("Vous n'avez pas de téléphone", CmdReturn.CmdReturnType.WARNING);
            Contact contact = (Contact)argv[0];
            player.Phone.RemoveContact(contact);
            return new CmdReturn("Vous avez ajouté un contact");
        }

    }
}
