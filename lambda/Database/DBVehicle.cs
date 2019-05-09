using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AltV.Net.Data;
using AltV.Net.Enums;
using Lambda.Entity;

namespace Lambda.Database
{
    public class DbVehicle : DbElement<Vehicle>
    {
        public DbVehicle(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Vehicle vehicle)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["veh_model"] = vehicle.Model.ToString();
            data["veh_position_x"] = vehicle.SpawnPosition.X.ToString();
            data["veh_position_y"] = vehicle.SpawnPosition.Y.ToString();
            data["veh_position_z"] = vehicle.SpawnPosition.Z.ToString();
            data["veh_rotation_r"] = vehicle.SpawnRotation.Roll.ToString();
            data["veh_rotation_p"] = vehicle.SpawnRotation.Pitch.ToString();
            data["veh_rotation_y"] = vehicle.SpawnRotation.Yaw.ToString();
            data["veh_color_r"] = vehicle.Color.R.ToString();
            data["veh_color_g"] = vehicle.Color.G.ToString();
            data["veh_color_b"] = vehicle.Color.B.ToString();
            data["veh_color2_r"] = vehicle.SecondaryColor.R.ToString();
            data["veh_color2_g"] = vehicle.SecondaryColor.G.ToString();
            data["veh_color2_b"] = vehicle.SecondaryColor.B.ToString();
            data["veh_lock"] = vehicle.Lock.Code;
            data["veh_plate"] = vehicle.GetPlate();

            if (vehicle.GetOwnerId() != 0)
            {
                if (vehicle.GetOwnerType() == Vehicle.OwnerType.CHARACTER)
                {
                    data["cha_id"] = vehicle.GetOwnerId().ToString();
                }
                else if (vehicle.GetOwnerType() == Vehicle.OwnerType.ORGANIZATION)
                {
                    data["org_id"] = vehicle.GetOwnerId().ToString();
                }
            }

            return data;
        }

        public override void SetData(Lambda.Entity.Vehicle vehicle, Dictionary<string, string> data)
        {
            vehicle.Model = (VehicleModel)Enum.Parse(typeof(VehicleModel), data["veh_model"]);
            Position position = new Position();
            position.X = float.Parse(data["veh_position_x"]);
            position.Y = float.Parse(data["veh_position_y"]);
            position.Z = float.Parse(data["veh_position_z"]);
            vehicle.SpawnPosition = position;
            vehicle.Lock.Code = data["veh_lock"];
            vehicle.Spawn();
            Rotation rotation = new Rotation();
            rotation.Roll = float.Parse(data["veh_rotation_r"]);
            rotation.Pitch = float.Parse(data["veh_rotation_p"]);
            rotation.Yaw = float.Parse(data["veh_rotation_y"]);
            vehicle.Rotation = rotation;
            Rgba color = new Color();
            color.R = byte.Parse(data["veh_color_r"]);
            color.G = byte.Parse(data["veh_color_g"]);
            color.B = byte.Parse(data["veh_color_b"]);
            Rgba secondaryColor = new Color();
            secondaryColor.R = byte.Parse(data["veh_color2_r"]);
            secondaryColor.G = byte.Parse(data["veh_color2_g"]);
            secondaryColor.B = byte.Parse(data["veh_color2_b"]);
            vehicle.Color = color;
            vehicle.SecondaryColor = secondaryColor;
            if (data.ContainsKey("cha_id"))
            {
                vehicle.SetOwnerType(Vehicle.OwnerType.CHARACTER);
                vehicle.SetOwnerId(uint.Parse(data["cha_id"]));
            }
            if (data.ContainsKey("org_id"))
            {
                vehicle.SetOwnerType(Vehicle.OwnerType.ORGANIZATION);
                vehicle.SetOwnerId(uint.Parse(data["org_id"]));
            }
            vehicle.SetPlate(data["veh_plate"]);

        }
    }
}
