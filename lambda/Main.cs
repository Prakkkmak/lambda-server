using System;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Items;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;
using IEntity = AltV.Net.Elements.Entities.IEntity;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda
{
    public class Main : Resource
    {

        public Main()
        {

        }
        public override void OnTick()
        {
            base.OnTick();

            //Alt.Log("Test");
        }
        public override void OnStart()
        {
            CreateDbConnect();
            RegisterEvents();
            Chat.RegisterEvents();
            Alt.Log($"Register commands in Lambda.commands");
            Command.RegisterAllCommands("Lambda.Commands"); // Register all command from lambda.command
            Alt.Log("Loading all vehs");
            Vehicle.LoadAllVehicles();
            Alt.Log("Loading all items");
            BaseItem.LoadAllItems();
            Alt.Log("Loading all skins");
            Skin.LoadAllSkins();
            Alt.Log("Loading all areas");
            Area.LoadAll();
            Alt.Log("Loading all components");
            ComponentLink.LoadAllComponentLinks();
        }
        public override void OnStop()
        {
            //
        }

        public static void RegisterEvents()
        {
            Alt.Log("[EVENT] Register events ...");
            Alt.OnPlayerConnect += OnPlayerConnect;
            Alt.Log("[EVENT] OnPlayerConnect registered");
            Alt.OnPlayerDisconnect += OnPlayerDisconnect;
            Alt.Log("[EVENT] OnPlayerDisconnect registered");
            Alt.OnPlayerDead += OnPlayerDead;
            Alt.Log("[EVENT] OnPlayerDead registered");
            Alt.OnVehicleRemove += OnVehicleRemove;
            Alt.Log("Events are registered");


        }

        private static void OnPlayerDisconnect(IPlayer altPlayer, string reason)
        {
            altPlayer.GetData("player", out Player player);
            if (player.Account != null)
            {
                player.Save();
            }
            Player.OnlinePlayers.Remove(player);
        }
        private static void OnPlayerDead(IPlayer altPlayer, IEntity entity, uint nbr)
        {
            altPlayer.GetData("player", out Player player);
            player?.Spawn(Player.SpawnPosition);
        }

        public static void OnPlayerConnect(IPlayer altPlayer, string reason)
        {
            Player player = new Player(altPlayer);
            player.Spawn(Player.SpawnPosition);
            player.Freeze(true);
        }

        public static void OnVehicleRemove(IVehicle vehicle)
        {
            //vehicle.GetData("AltVehicle", out Vehicle veh);
            //veh?.Spawn();
        }

        public static void CreateDbConnect()
        {
            DBConnect.DbConnect = new DBConnect();
        }

        public static void Test()
        {
        }


    }



}
