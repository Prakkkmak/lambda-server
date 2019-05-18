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
using Lambda.Utils;
using IEntity = AltV.Net.Elements.Entities.IEntity;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;
using VehicleFactory = Lambda.FactoryStuff.VehicleFactory;
using PlayerFactory = Lambda.FactoryStuff.PlayerFactory;

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
            //DatabaseElement.DbConnect = new DBConnect();
            Alt.Log("=== Register events... ===");
            Events.RegisterEvents();
            Alt.Log(">Server events registered");
            Chat.RegisterEvents();
            Alt.Log(">Chat registered");
            Alt.Log("=== Events are registered ===");
            Alt.Log("=== Register Spawns... ===");
            //AddSpawn(new Spawn(new Position(-263.2484f, 2195.248f, 130.3956f)));
            Alt.Log("=== Spawns added... ===");
            Alt.Log("=== Register Commands... ===");
            Command.GetAllCommands();
            Alt.Log("=== Commands are registered ===");
            Alt.Log("=== Load in database... ===");
            Interior.LoadInteriors();
            Alt.Log(">All interiors loaded");
            BaseItem.LoadBaseItems();
            Alt.Log(">Base items created");
            Vehicle.LoadVehicles();
            Alt.Log(">Vehicles spawned");
            Area.LoadAreas();
            Alt.Log(">All areas loaded");
            Organization.LoadOrganizations();
            Alt.Log(">All organizations loaded");
            Anim.RegisterAnims();
            //AddAllLinks();
            //Game = new Game();
            //Game.Init();
            //Game.BaseGame = Game;
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
