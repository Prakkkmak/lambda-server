using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace Lambda.Entity
{
    public class Checkpoint : IEntity
    {
        public short Dimension { get; set; } = 0;

        public Position Position { get; set; } = Position.Zero;

        public Rotation Rotation { get; set; } = Rotation.Zero;

        public Position GroundPosition
        {
            get {
                return new Position(Position.X, Position.Y, Position.Z - 1);
            }
            set {
                value = new Position(Position.X, Position.Y, Position.Z + 1);
            }
        } 

        public float Range = 0.5f;
        public float Height  = 0.5f;

        public Rgba Color  = Rgba.Zero;

        public string Name = "";

        public CheckpointType Type = CheckpointType.Cyclinder;
        
        public Action<Player> Action = null;

        public ICheckpoint AltCheckpoint;

        public Checkpoint(Position position, short dim, Action<Player> action, CheckpointType type = CheckpointType.Cyclinder, float range = 0.5f, float height = 1, Rgba color = default, string name = "")
        {
            Dimension = dim;
            Position = position;
            Range = range;
            Height = height;
            Color = color;
            Action = action;
            Name = name;
            DrawCheckpoint();
            Checkpoints.Add(this);
            
        }

        public void DrawCheckpoint()
        {
            if (AltCheckpoint != null) AltCheckpoint.Remove();
            AltCheckpoint = Alt.CreateCheckpoint(Type, GroundPosition, Range, Height, Color);
        }


        public static void CreateFarmJobCheckpoints()
        {
            new Checkpoint(new Position(2016.62f, 4987.859f, 42.08569f), 0, (player) =>
            {
                player.SendMessage("Bienvenue à la ferme publique ! La ville vous a fourni un agréable endroit pour pratiquer votre passion.");
                player.SendMessage("Faites /licence info pour en savoir plus sur cette licence");
            }, CheckpointType.Cyclinder3, 1, 1, new Rgba(20,200,150,150), "farm");
        }
        public static Checkpoint[] GetCheckpointsByName(string name)
        {
            List<Checkpoint> checkpoints = new List<Checkpoint>();
            foreach(Checkpoint checkpoint in Checkpoints)
            {
                if (checkpoint.Name.Equals(name)) checkpoints.Add(checkpoint);
            }
            return checkpoints.ToArray();
        }
        public static List<Checkpoint> Checkpoints = new List<Checkpoint>();

    }
}
