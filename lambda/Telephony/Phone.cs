using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Lambda.Database;
using Player = Lambda.Entity.Player;

namespace Lambda.Telephony
{
    public class Phone : IDBElement
    {
        public enum PhoneRing
        {
            Normal,
        }
        public uint Id { get; set; }
        public List<Contact> Contacts = new List<Contact>();
        public List<Message> Messages = new List<Message>();
        public string Number = "0000023";
        public bool Vibrator = true;
        public PhoneRing Ring = PhoneRing.Normal;
        public ushort RingVolume = 0; // 0 - 10
        public Phone Calling = null;
        public IVoiceChannel VoiceChannel = null;

        public Phone()
        {

        }

        public Player Player
        {
            get {
                foreach (Player player in Player.Players)
                {
                    if (player.Phone == this) return player;
                }

                return null;
            }
        }

        /** Contacts **/

        public void AddContact(Contact contact)
        {
            Contacts.Add(contact);
            contact.Save();
        }

        public Contact[] GetContacts(string name)
        {
            name = name.Replace('_', ' ');
            List<Contact> contacts = new List<Contact>();
            foreach (Contact contact in Contacts)
            {
                if (contact.Name.ToLower().StartsWith(name.ToLower())) contacts.Add(contact);
                if (contact.PhoneNumber.ToLower().StartsWith(name.ToLower())) contacts.Add(contact);
            }

            return contacts.ToArray();
        }

        public void RemoveContact(Contact contact)
        {
            Contacts.Remove(contact);
        }

        /** Messages **/
        public void SendTextMessage(Message message)
        {
            //Get the distant phone
            Phone target = GetPhone(message.To);
            if (target == null) return;
            //Send the message
            target.GetTextMessage(message);
            //Save the message sended
            Messages.Add(message);
            message.Save();

        }

        public void GetTextMessage(Message message)
        {
            string from = message.From;
            Contact[] contacts = GetContacts(from);
            if (contacts.Length == 1)
            {
                from = contacts[0].Name;
            }
            //If a player is attached to this phone
            if (Player != null)
            {
                Player.SendMessage($"{from} vous envoie un message:");
                Player.SendMessage($"{message.Body}");
            }
            Alt.Log(Number + " Recois le message:<br> " + message.Body + " <br>___ " + message.From);
            //Change message data
            message.Phone = this;
            message.Id = 0; // On repasse a 0 le message
            Messages.Add(message);
            //Save the message
            message.Save();

        }
        /** Appels **/
        public void SendCall(string number)
        {
            Phone target = GetPhone(number);
            if (target == null) return;
            target.GetCall(this);
            Calling = target;
        }

        public void GetCall(Phone phone)
        {
            string from = phone.Number;
            Contact[] contacts = GetContacts(from);

            if (contacts.Length == 1)
            {
                from = contacts[0].Name;
            }
            if (Vibrator == true)
            {
                Player.SendMessage($"Votre téléphone vibre ! Numero entrant : {from}");
                Player.SendMessage($"/telephone deccrocher pour déccrocher");
            }
            Calling = phone;
        }

        public void PickUp()
        {
            if (VoiceChannel != null) return;
            if (Calling == null) return;
            VoiceChannel = Alt.CreateVoiceChannel(false, 0);
            Calling.VoiceChannel = VoiceChannel;
            VoiceChannel.AddPlayer(Player);
            VoiceChannel.AddPlayer(Calling.Player);
        }
        public void PickOff()
        {
            if (VoiceChannel == null) return;
            if (Calling == null) return;
            VoiceChannel.RemovePlayer(Player);
            VoiceChannel.RemovePlayer(Calling.Player);
            //Alt.RemoveVoiceChannel(VoiceChannel);
            VoiceChannel = null;
            Calling.VoiceChannel = null;
            Calling = null;
        }
        /** **/

        public static Phone GetPhone(string number)
        {
            foreach (Phone phone in Phone.Phones)
            {
                if (phone.Number.Equals(number)) return phone;
            }

            return null;
        }
        public static Phone GetPhone(uint id)
        {
            foreach (Phone phone in Phone.Phones)
            {
                if (phone.Id == id) return phone;
            }

            return null;
        }

        public static string GeneratePhoneNumber()
        {
            string numbers = "0123456789";
            string number = "";
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                number += numbers[random.Next(numbers.Length)];
            }

            if (GetPhone(number) != null) return GeneratePhoneNumber();
            else return number;

        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["pho_number"] = Number;
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Number = data["pho_number"];
            Id = Convert.ToUInt32(data["pho_id"]);
            Messages = DatabaseElement.GetAllMessages(this).ToList();
            Contacts = DatabaseElement.GetAllContacts(this).ToList();
        }

        public void Save()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Phone Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            long t = DateTime.Now.Ticks;
            DatabaseElement.Delete(this);
            Alt.Log("Phone Deleted en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Phone Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public static void LoadPhones()
        {
            Phones.AddRange(DatabaseElement.GetAllPhones());
        }

        public static List<Phone> Phones = new List<Phone>();
    }
}
