using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Items;

namespace Lambda.Entity
{
    public class PlantProp : Prop
    {

        public DateTime GrowthStart = DateTime.Now;

        public int GrowthDuration = 60;

        public float StartZ = 0;
        public float EndZ = 0;

        public Enums.Items RecoltType = Enums.Items.Invalid;

        public bool AutoPlant = false;

        public PlantProp(Position pos, Rotation rot, short dim, string name, float startZ, float endZ, int growDuration) : base(pos, rot, dim, name)
        {
            StartZ = startZ;
            EndZ = endZ;
            GrowthDuration = growDuration;
            SyncFrequency = growDuration / 50;
            PlantProps.Add(this);
        }

        public override void Update()
        {
            base.Update();
            this.Position = new Position(this.Position.X, this.Position.Y, (float)GetCurrentZ());
            
        }

        public double GetCurrentZ()
        {
            return StartZ + (EndZ - StartZ) * GetGrowProgression();
        }

        public double GetGrowProgression()
        {
            DateTime now = DateTime.Now;
            double secondsSinceStart = (now - GrowthStart).TotalSeconds;
            double result = secondsSinceStart / GrowthDuration;
            if (result > 1)
            {
                result = 1;
                SyncFrequency = 0;
                Alt.Log("Grow max; passage a une sync de 0");
            }
            return result;
        }

        public Item Harvrest()
        {

            Item item = null;
            if (GetGrowProgression() > 0.95f)
            {
                item = new Item(BaseItem.GetBaseItem(RecoltType), 1);
                if (AutoPlant) GrowthStart = DateTime.Now;
                else
                {
                    //TODO faire le machin de remove de la plante
                    this.Position = new Position(0, 0, 0);
                    StartZ = 0;
                    EndZ = 0;
                    GrowthDuration = 1;
                }
            }
            
            return item;
        }

        public static PlantProp CreateTomatoPlant(Position pos, bool auto = false)
        {
            Alt.Log("plant created in " + pos.X + " " + pos.Y + " " + pos.Z);
            Random random = new Random();
            float yaw = (float)(random.NextDouble() * Math.PI);
            PlantProp plant = new PlantProp(pos, new Rotation(0,0,yaw), 0, "prop_bush_med_03", pos.Z - 4, pos.Z - 2, 60*10);
            plant.RecoltType = Enums.Items.Tomato;
            plant.AutoPlant = auto;
            if (auto) plant.GrowthStart = DateTime.Now.AddSeconds(-60 * 10);
            return plant;
        }

        public static void GenerateLane(Position pos1, Position pos2, int nbr)
        {
            float x1 = pos1.X;
            float x2 = pos2.X;
            float y1 = pos1.Y;
            float y2 = pos2.Y;
            float deltaX = x2 - x1;
            float deltaY = y2 - y1;

            //4 cas

            Random rng = new Random();
            for(int i = 0; i < nbr; i++)
            {
                CreateTomatoPlant(new Position(x1 + ((deltaX/ (nbr - 1)) * i) + 0.25f - (float)rng.NextDouble() * 0.5f, y1 + ((deltaY/ (nbr - 1)) * i) + 0.25f - (float)rng.NextDouble() * 0.5f, pos1.Z), true);
            }
        }
        public static void GeneraField(Position pos1, Position pos2, Position pos3, Position pos4, int nbrPerLane, int nbr)
        {
            //Lane 1
            float x1 = pos1.X;
            float x2 = pos2.X;
            float y1 = pos1.Y;
            float y2 = pos2.Y;
            //Lane 2
            float x3 = pos3.X;
            float x4 = pos4.X;
            float y3 = pos3.Y;
            float y4 = pos4.Y;
            //4 cas
            float deltaXa = x3 - x1;
            float deltaYa = y3 - y1;
            float deltaXb = x4 - x2;
            float deltaYb = y4 - y2;
            for(int i = 0; i < nbr; i++)
            {
                Position positionA = new Position(x1 + (deltaXa/(nbr - 1)) * i, y1 + (deltaYa/ (nbr - 1)) * i, pos1.Z);
                Position positionB = new Position(x2 + (deltaXb/(nbr - 1)) * i, y2 + (deltaYb/ (nbr - 1)) * i, pos2.Z);
                GenerateLane(positionA, positionB, nbrPerLane);
            }

        }
        public static List<PlantProp> PlantProps = new List<PlantProp>();
    }
}
