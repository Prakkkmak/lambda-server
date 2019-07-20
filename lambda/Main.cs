using System;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Factories;
using AltV.Net.Enums;
using Items;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Telephony;
using Lambda.Utils;
using IEntity = AltV.Net.Elements.Entities.IEntity;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;
using VehicleFactory = Lambda.FactoryStuff.VehicleFactory;
using PlayerFactory = Lambda.FactoryStuff.PlayerFactory;
using Lambda.Buildings;

namespace Lambda
{
    public class Main : Resource
    {



        public Main()
        {

        }


        private long minutes = 0;
        private long startTicks = DateTime.Now.Ticks;
        private DateTime lastTime = DateTime.Now;
        public override void OnTick()
        {

            base.OnTick();
            long delta = DateTime.Now.Ticks - lastTime.Ticks;
            if ((DateTime.Now.Ticks - startTicks) / TimeSpan.TicksPerMinute > minutes)
            {
                minutes++;
                Alt.Log(minutes + "");
                foreach (Player player in Player.Players)
                {
                    player.TimeOnline++;
                    player.TotalTimeOnline++;
                }
            }
            lastTime = DateTime.Now;
        }
        public override void OnStart()
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("LAMBDA RP");
            //DatabaseElement.DbConnect = new DBConnect();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Register events... ===");
            Console.ForegroundColor = ConsoleColor.Green;
            Events.RegisterEvents();
            Console.WriteLine("Server events registered.");
            Chat.RegisterEvents();
            Console.WriteLine("Chat registered.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Events are registered ===");
            Console.WriteLine("=== Register Commands... ===");
            Console.ForegroundColor = ConsoleColor.Green;
            Command.GetAllCommands();
            Console.WriteLine(Command.Commands.Count + " Commands registered.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Commands are registered ===");
            Console.WriteLine("=== Load in database... ===");
            Console.ForegroundColor = ConsoleColor.Green;
            BaseItem.LoadBaseItems();
            Console.WriteLine(BaseItem.BaseItems.Count + " base items created.");
            Vehicle.LoadVehicles();
            Console.WriteLine(Vehicle.Vehicles.Count + " vehicles spawned.");
            Phone.LoadPhones();
            Console.WriteLine(Phone.Phones.Count + " phones loaded.");
            House.LoadHouses();
            Console.WriteLine(House.Houses.Count + " houses loaded.");
            Bank.LoadBanks();
            Console.WriteLine(House.Houses.Count + " banks loaded.");
            Organization.LoadOrganizations();
            Console.WriteLine(Organization.Organizations.Count + " organizations loaded.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== Database loaded ===");
            Console.WriteLine("=== Others ===");
            Console.ForegroundColor = ConsoleColor.Green;
            Anim.RegisterAnims();
            Console.WriteLine(Anim.Anims.Length + " anims loaded.");
            Command.CommandsToTextfile();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Commands text file created");
            Console.ResetColor();
           
        }
        public override void OnStop()
        {
            Alt.Log("Stopping ...");
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new VehicleFactory();
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new PlayerFactory();
        }




    }



}
