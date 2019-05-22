using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Items;
using Lambda.Administration;
using Lambda.Database;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda
{
    static class Events
    {


        public static void RegisterEvents()
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
            Alt.OnClient("setskin", OnClientSetSkin);
            Alt.OnClient("setheaddata", OnClientSetHeadData);
            Alt.Log("[EVENT] OnVehicleEnter registered");
            Alt.OnPlayerEnterVehicle += OnVehicleEnter;
            Alt.OnPlayerLeaveVehicle += OnVehicleLeave;
        }

        public static void OnPlayerConnect(IPlayer altPlayer, string reason)
        {
            Player pl = (Player)altPlayer;
        }

        private static void OnPlayerDisconnect(IPlayer altPlayer, string reason)
        {
            Player player = (Player)altPlayer;
            player.Remove();
        }

        private static void OnPlayerDead(IPlayer altPlayer, AltV.Net.Elements.Entities.IEntity killer, uint nbr)
        {
            Player player = (Player)altPlayer;
            //player.Spawn(Spawn.NewSpawn.Position);
        }

        public static void OnVehicleRemove(IVehicle vehicle)
        {
            //vehicle.GetData("AltVehicle", out Vehicle veh);
            //veh?.Spawn();
        }

        public static void OnVehicleEnter(IVehicle altVehicle, IPlayer altPlayer, byte seat)
        {
            Vehicle vehicle = (Vehicle)altVehicle;
            Player.VoiceChannel.RemovePlayer(altPlayer);
            vehicle.VoiceChannel.AddPlayer(altPlayer);
        }

        public static void OnVehicleLeave(IVehicle altVehicle, IPlayer altPlayer, byte seat)
        {
            altVehicle.GetData("vehicle", out Vehicle vehicle);
            if (vehicle == null) return;
            Player.VoiceChannel.AddPlayer(altPlayer);
            vehicle.VoiceChannel.RemovePlayer(altPlayer);
        }

        public static void OnPlayerSetLicenseHash(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string license = (string)args[0];
            Alt.Log("License set to " + license);
            altPlayer.SetData("license", license);

            player.Spawn(Spawn.NewSpawn.Position);
            player.Freeze(false);
            Account account = new Account(license);
            account = DatabaseElement.Get<Account>(account, "acc_license", license);
            if (account != null)
            {
                DatabaseElement.Get<Player>(player, "acc_id", account.Id.ToString());
                player.Account = account;
            }
            else
            {
                player.Account = new Account(player.License);
                _ = player.SaveAsync();

            }


            Alt.Log($"[{player.ServerId}]{player.FullName} c'est connécté.");
            Player.AddPlayer(player);
            Alt.Log("Skin chargement");
            player.GetSkin().SendModel(player);
            player.GetSkin().SendSkin(player);
        }

        public static void OnClientSetSkin(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string str = "";
            object[] arg = (object[])args[0];
            foreach (object o in arg)
            {
                str += o.ToString() + ',';
            }

            str = str.Remove(str.Length - 1);
            player.GetSkin().SetString(str);
            _ = player.SaveAsync();
        }
        public static void OnClientSetHeadData(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string str = "";
            foreach (object o in args)
            {
                str += o.ToString() + ',';
            }
            str = str.Remove(str.Length - 1);
            player.GetSkin().SetHeadDataString(str);
            _ = player.SaveAsync();
        }



    }
}
