using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Elements.Entities;
using Lambda.Administration;
using IEntity = Lambda.Entity.IEntity;
using Player = Lambda.Entity.Player;

namespace Lambda
{
    class Events
    {

        private Game game;

        public Events(Game g)
        {
            game = g;
            RegisterEvents();
        }

        public void RegisterEvents()
        {
            Alt.OnPlayerConnect += OnPlayerConnect;
            Alt.Log("[EVENT] OnPlayerConnect registered");
            Alt.OnPlayerDisconnect += OnPlayerDisconnect;
            Alt.Log("[EVENT] OnPlayerDisconnect registered");
            Alt.OnPlayerDead += OnPlayerDead;
            Alt.Log("[EVENT] OnPlayerDead registered");
            Alt.OnVehicleRemove += OnVehicleRemove;
            Alt.Log("[EVENT] OnPlayerDisconnect registered");
            Alt.OnClient("setlicense", OnPlayerSetLicenseHash);

        }

        public void OnPlayerConnect(IPlayer altPlayer, string reason)
        {

        }

        private void OnPlayerDisconnect(IPlayer altPlayer, string reason)
        {
            altPlayer.GetData("player", out Player player);

            //game.DbPlayer.Save(player);
            game.RemovePlayer(player);
        }

        private void OnPlayerDead(IPlayer altPlayer, AltV.Net.Elements.Entities.IEntity killer, uint nbr)
        {
            altPlayer.GetData("player", out Player player);
            player?.Spawn(game.GetSpawn(0).Position);
        }

        public void OnVehicleRemove(IVehicle vehicle)
        {
            //vehicle.GetData("AltVehicle", out Vehicle veh);
            //veh?.Spawn();
        }

        public void OnPlayerSetLicenseHash(IPlayer altPlayer, object[] args)
        {
            Alt.Log("License set to " + (string)args[0]);
            altPlayer.SetData("license", (string)args[0]);
            Player player = new Player(altPlayer, game);
            player.Spawn(game.GetSpawn(0).Position);
            player.Freeze(false);
            Account account = game.DbAccount.Get(player.license);
            if (account != null)
            {
                game.DbPlayer.Get(account, player);
                player.Account = account;
                game.DbSkin.Get(player.GetSkin().Id, player.GetSkin());
                player.GetSkin().SendSkin(player);
            }
            else
            {
                player.Account = new Account(player.license);
                game.DbAccount.Save(player.Account);
                game.DbPlayer.Save(player);

            }
            game.AddPlayer(player);
        }

    }
}
