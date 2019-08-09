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
using System.Net;
using System.Configuration;
using System.IO;
using Lambda.Quests;

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
            if ((DateTime.Now.Ticks - startTicks) / TimeSpan.TicksPerMinute > minutes)
            {
                minutes++;
                foreach (Player player in Player.Players)
                {
                    player.OnMinutePass();
                }
            }
            lastTime = DateTime.Now;

            //Prop.Props.ForEach((elem) => elem.Update());
            Prop.SyncProps();
        }
        public override void OnStart()
        {
            Alt.Log("aa");
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
            Console.ForegroundColor = ConsoleColor.Green;
            Whitelist.MainWhitelist.GetWhitelist();
            Console.WriteLine("Whitelist loaded");
            Console.ResetColor();
            Entity.Checkpoint.CreateFarmJobCheckpoints();
            Quest.GenerateQuests();
            PlantProp.GeneraField(new Position(2029.53f, 4904.769f, 43), new Position(2006.44f, 4927.266f, 43),
                new Position(2005.292f, 4880.439f, 43), new Position(1981.883f, 4902.883f, 43), 10, 18) ;
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
