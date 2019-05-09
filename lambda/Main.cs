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
        public Game Game;

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
                foreach (Player player in Game.GetPlayers())
                {
                    player.TimeOnline++;
                    player.TotalTimeOnline++;
                }
            }
            lastTime = DateTime.Now;
        }
        public override void OnStart()
        {
            Game = new Game();
            Game.Init();
            //Game.BaseGame = Game;
        }
        public override void OnStop()
        {
            Alt.Log("Stopping ...");
        }








    }



}
