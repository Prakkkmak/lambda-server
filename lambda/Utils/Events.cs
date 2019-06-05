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
using Lambda.Items;
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
            Alt.OnClient("sethaircolor", OnClientSetHairColor);
            Alt.OnClient("seteyecolor", OnClientSetEyeColor);
            Alt.OnClient("setfacefeatures", OnClientSetFeatures);
            Alt.OnClient("setheadoverlays", OnClientSetOverlays);
            Alt.OnClient("setprops", OnClientSetProps);
            Alt.OnClient("chatConsole", OnClientSendChatConsole);
            Alt.OnClient("changeSelectedPlayer", OnClientChangePlayerSelected);
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
            Alt.Log($"[{player.ServerId}]{player.FullName} s'est connecté.");
            Player.AddPlayer(player);
            player.Emit("playerLoaded");
            //player.Skin.SendModel(player);
            //player.GetSkin().SendSkin(player);
        }

        public static void OnClientSetSkin(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            object[] test = (object[])args[0];
            int[] converted = Array.ConvertAll(test, item => (int)(long)item);
            player.Skin.SetComponents(converted);
            _ = player.SaveAsync();
        }
        public static void OnClientSetHeadData(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string[] converted = Array.ConvertAll(args, item => item + "");
            player.Skin.SetHeadData(converted);
            //_ = player.SaveAsync();
        }
        public static void OnClientSetHairColor(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            player.Skin.Hairiness.HairColor = (uint)(long)args[0];
            player.Skin.Hairiness.HairColor2 = (uint)(long)args[1];
            //_ = player.SaveAsync();
        }
        public static void OnClientSetEyeColor(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            player.Skin.Face.EyeColor = (uint)(long)args[0];
            //_ = player.SaveAsync();
        }
        public static void OnClientSetFeatures(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string str = "";
            object[] test = (object[])args[0];
            int[] converted = Array.ConvertAll(test, item => (int)(long)item);
            player.Skin.SetFeatures(converted);
            //_ = player.SaveAsync();
        }
        public static void OnClientSetProps(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string str = "";
            object[] test = (object[])args[0];
            int[] converted = Array.ConvertAll(test, item => (int)(long)item);
            player.Skin.SetProps(converted);
            //_ = player.SaveAsync();
        }
        public static void OnClientSetOverlays(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            string str = "";
            object[] test = (object[])args[0];
            int[] converted = Array.ConvertAll(test, item => (int)(long)item);
            player.Skin.SetOverlays(converted);
            //_ = player.SaveAsync();
        }
        public static void OnClientChangePlayerSelected(IPlayer altPlayer, object[] args)
        {
            Player player = (Player)altPlayer;
            int selectedId = Convert.ToInt32(args[0]);
            if (selectedId == -1) player.PlayerSelected = null;
            else
            {
                player.PlayerSelected = Player.GetPlayers(selectedId + "")[0];
                Alt.Log(player.Name + " Séléctionne " + player.PlayerSelected.Name);
            }


        }
        public static void OnClientSendChatConsole(IPlayer altPlayer, object[] args)
        {
            Alt.Log(args[0] + "");
        }




    }
}
