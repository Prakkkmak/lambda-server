using Lambda.Administration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Utils;
using System.ComponentModel.DataAnnotations;
namespace Lambda.Commands
{
    public class AccountCmds
    {
        /// <summary>
        /// Send an bugg to the Trello
        /// /bugg It is an example of bug
        /// </summary>
        [Command(Command.CommandType.ACCOUNT)]
        public static CmdReturn Bug(Player player, string[] argv)
        {
            int minSize = 10;
            string text = "";
            string sender;
            if (player.Account != null)
            {
                sender = player.Account.Mail + " - " + player.Name; //used for have the mail and the character name of the sender
            }
            else
            {
                sender = player.AltPlayer.Name;
            }
            for (int i = 1; i < argv.Length; i++)
            {
                text += argv[i] + " ";
            }
            if (text.Length < minSize) return new CmdReturn("Votre bug doit être plus long ! ", CmdReturn.CmdReturnType.SYNTAX); // The minimum length of the report bugg
            Bug bug = new Bug(text, sender); // Create the bugg object
            bug.Send(); // Send the bugg
            return new CmdReturn("Vous avez envoyé votre bug !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.ACCOUNT, "mail", "motdepasse", "motdepasse")]
        public static CmdReturn Enregistrer(Player player, string[] argv)
        {
            string mail = argv[1];
            string password = argv[2];
            string confirmPassword = argv[3];
            EmailAddressAttribute emailAdressAttribute = new EmailAddressAttribute();
            if (player.Account != null) return new CmdReturn("Vous êtes déjà connecté", CmdReturn.CmdReturnType.WARNING);
            if (!emailAdressAttribute.IsValid(mail)) return new CmdReturn("Mail invalide.", CmdReturn.CmdReturnType.SYNTAX);
            if (password.Length < 6) return new CmdReturn("Mot de passe trop court. Il doit faire 6 caractères", CmdReturn.CmdReturnType.SYNTAX);
            Account account = new Account(mail);
            account.Register(password);
            player.Account = account;
            player.Save();
            return new CmdReturn("Vous vous êtes enregistré !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ACCOUNT, "Mail", "motdepasse")]
        public static CmdReturn Connecter(Player player, string[] argv)
        {
            string mail = argv[1];
            string password = argv[2];
            EmailAddressAttribute emailAdressAttribute = new EmailAddressAttribute();
            if (player.Account != null) return new CmdReturn("Vous êtes déjà connecté", CmdReturn.CmdReturnType.WARNING);
            if (!emailAdressAttribute.IsValid(mail) || password.Length < 6) return new CmdReturn("Identifiants incorrects", CmdReturn.CmdReturnType.SYNTAX);
            Account account = Account.LogIn(mail, password);
            if (account == null) return new CmdReturn("Identifiants incorrects", CmdReturn.CmdReturnType.WARNING);
            player.Account = account;
            if (!player.Load())
            {
                player.Account = null;
                return new CmdReturn("Vous n'avez pas de personnage", CmdReturn.CmdReturnType.WARNING);

            }
            return new CmdReturn("Vous vous êtes connecté !", CmdReturn.CmdReturnType.SUCCESS);
        }

        [Command(Command.CommandType.ACCOUNT, "Prénom", "Nom")]
        public static CmdReturn Personnage_Nom(Player player, string[] argv)
        {
            string firstName = argv[2];
            string lastName = argv[3];
            Regex regex = new Regex("[A-Z][a-z]+");
            if (!regex.Match(firstName).Success) return new CmdReturn("Votre prénom doit être de la forme : Babar");
            if (!regex.Match(lastName).Success) return new CmdReturn("Votre nom doit être de la forme : Jocombe");
            player.FirstName = firstName;
            player.LastName = lastName;
            player.AltPlayer.Name = player.Name;
            player.Save();
            return new CmdReturn("Vous avez changé de nom !", CmdReturn.CmdReturnType.SUCCESS);
        }
        [Command(Command.CommandType.ACCOUNT)]
        public static CmdReturn Personnage_Sauvegarder(Player player, string[] argv)
        {
            player.Save();
            return new CmdReturn("Vous avez sauvegardé votre personnage !", CmdReturn.CmdReturnType.SUCCESS);
        }


        /*
        /// <summary>
        /// Permet au joueur de se connecter.
        /// /connect monmail@example.fr monmotdepasse
        /// </summary>
        [Command(Command.CommandType.ACCOUNT, "Mail", "motdepasse")]
        public static CmdReturn Connect(Player player, string[] argv)
        {
            string mail = argv[1]; // The mail
            string password = argv[2]; // The password
            if (mail.Length < 3 || !mail.Contains("@")) return new CmdReturn("Mail invalide.", CmdReturn.CmdReturnType.SYNTAX); // We need to have at least 3 chars and the mail contains an @
            if (password.Length < 6) return new CmdReturn("Mot de passe trop court. Il doit faire 6 caractères", CmdReturn.CmdReturnType.SYNTAX); // we need to have a password greater than 6 chars
            Account newAccount = Account.GetAccount(mail, password); // Get the account associated with this mail and password
            if (newAccount == null) return new CmdReturn("Identifiants incorects.", CmdReturn.CmdReturnType.WARNING); // Return if the ids are bad
            Account.Accounts.Add(newAccount); // Addd the new account to the list of accounts
            newAccount.Connected = true;
            newAccount.Character = player
            account.Character.AltPlayer.SetData("account", newAccount);
            Character character = newAccount.LoadCharacter(0);
            if (character == null) return new CmdReturn("Bravo ! Vous vous êtes connecté. Veuillez créer un personnage avec /personnage creer");
            return new CmdReturn("Bravo vous êtes connecté ! Votre personnage a été chargé automatiquement", CmdReturn.CmdReturnType.SUCCESS);
        }
        /// <summary>
        /// Permet au joueur de s'enregister.
        /// /register monmail@example.fr monmotdepasse monmotdepasse
        /// </summary>
        [Command(Command.CommandType.ACCOUNT, "Mail", "motdepasse", "motdepasse")]
        public static CmdReturn Register(Account account, string[] argv)
        {
            string mail = argv[1];
            string password = argv[2];
            string confirmPassword = argv[3];
            if (mail.Length < 3 || !mail.Contains("@")) return new CmdReturn("Mail invalide.", CmdReturn.CmdReturnType.SYNTAX); // We need to have at least 3 chars and the mail contains an @
            if (password.Length < 6) return new CmdReturn("Mot de passe trop court.", CmdReturn.CmdReturnType.SYNTAX);  // we need to have a password greater than 6 chars
            if (!string.Equals(password, confirmPassword)) return new CmdReturn("Mots de passe différents.", CmdReturn.CmdReturnType.SYNTAX); // Test if passwords are the same
            return account.Register(mail, password, null) // Register the account
                ? new CmdReturn($"Félicitations {account.Mail} ! Vous êtes enregistré. Veuillez vous connécter avec /connect", CmdReturn.CmdReturnType.SUCCESS)
                : new CmdReturn("Erreur dans votre enregistrement. Contactez un administrateur", CmdReturn.CmdReturnType.ERROR);
        }

        /// <summary>
        /// Permet au joueur de s'enregister.
        /// /register monmail@example.fr monmotdepasse monmotdepasse
        /// </summary>
        [Command(Command.CommandType.DEFAULT, "Prenom", "Nom")]
        public static CmdReturn Personnage_Creer(Account account, string[] argv)
        {
            string firstName = argv[2];
            string lastName = argv[3];
            Regex regex = new Regex("[A-Z][a-z]+");
            if (!regex.Match(firstName).Success) return new CmdReturn("Votre prénom doit être de la forme : 'Babar'");
            if (!regex.Match(lastName).Success) return new CmdReturn("Votre nom doit être de la forme : 'Jocombe'");
            account.CreateCharacter(firstName, lastName);
            return new CmdReturn("vous avez créé un personnage", CmdReturn.CmdReturnType.SUCCESS);
        }
        /*
        [Command(Command.CommandType.DEFAULT, "Slot")]
        public static CmdReturn Personnage_Selectioner(Account account, string[] argv)
        {
            if (!uint.TryParse(argv[2], out uint slot)) return new CmdReturn("Slot invalide ", CmdReturn.CmdReturnType.SYNTAX);
            Character character = account.LoadCharacter((int)slot);
            return new CmdReturn("vous avez chargé un personnage", CmdReturn.CmdReturnType.SUCCESS);
        }*/






    }
}
