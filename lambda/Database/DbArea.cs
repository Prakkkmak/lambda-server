using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using Lambda.Entity;

namespace Lambda.Database
{
    public class DbArea : DbElement<Area>
    {
        public DbArea(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Area area)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["are_type"] = area.Type.ToString();
            data["are_position_x"] = area.Position.X.ToString();
            data["are_position_y"] = area.Position.Y.ToString();
            data["are_position_z"] = area.Position.Z.ToString();
            data["are_radius"] = area.Radius.ToString();
            data["are_metadata"] = area.GetMetaData();
            //datas["are_checkpoint"] = Type.ToString();
            return data;
        }

        public override void SetData(Area area, Dictionary<string, string> data)
        {
            Enum.TryParse(data["are_type"], out Area.AreaType type);
            area.Type = type;
            Position position = new Position();
            position.X = float.Parse(data["are_position_x"]);
            position.Y = float.Parse(data["are_position_y"]);
            position.Z = float.Parse(data["are_position_z"]);
            area.Radius = float.Parse(data["are_radius"]);
            area.SetMetaData(data["are_metadata"]);
            area.Spawn(position);

        }
        public Area[] GetAll()
        {
            List<Area> entities = new List<Area>();
            List<Dictionary<string, string>> results = DbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                Area area;
                if (result["are_type"] == "SHOP")
                {
                    area = new Shop();
                    //Shop shop = new Shop();

                }
                else
                {
                    area = new Area();
                }
                SetData(area, result);
                area.Id = uint.Parse(result[Prefix + "_id"]);
                entities.Add(area);
                //SetData(entity, result);
                //entity.Id = uint.Parse(result[Prefix + "_id"]);
                //entities.Add(entity);
            }

            return entities.ToArray();
        }
    }
}
