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



        public override void OnTick()
        {
            base.OnTick();

            //Alt.Log("Test");
        }
        public override void OnStart()
        {
            Game = new Game();
            Game.Init();

        }

        public override void OnStop()
        {
            Alt.Log("Stopping ...");
        }








    }



}
