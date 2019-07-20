using AltV.Net.Data;
using Lambda.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace Lambda.Buildings
{
    public class Bank : Building
    {
        //public List<Checkpoint> Atms = new List<Checkpoint>();
        public Bank(Position position, short dim = 0) : base(position, dim)
        {
            Checkpoint.Color = new Rgba(0, 200, 0, 100);
            Checkpoint.DrawCheckpoint();
            Banks.Add(this);
        }

        public static void LoadBanks()
        {
            Banks.Add(new Bank(new Position(1175, 2706, 38), 0));
        }

        public static List<Bank> Banks = new List<Bank>();
    }
}
