using Items;
using Lambda.Items;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Enums;

namespace Lambda.Database
{
    public class DbSkin : DbElement<Skin>
    {
        public DbSkin(Game game, DBConnect dbConnect, string tableName, string prefix) : base(game, dbConnect, tableName, prefix)
        {
        }

        public override Dictionary<string, string> GetData(Skin skin)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data["ski_valid"] = (skin.Valid ? 1 : 0).ToString();
            data["ski_type"] = skin.Type;
            data["ski_model"] = skin.Model.ToString();
            data["ski_mask_drawable"] = skin.GetComponent(0).Drawable.ToString();
            data["ski_mask_texture"] = skin.GetComponent(0).Texture.ToString();
            data["ski_mask_palette"] = skin.GetComponent(0).Palette.ToString();
            data["ski_hair_drawable"] = skin.GetComponent(1).Drawable.ToString();
            data["ski_hair_texture"] = skin.GetComponent(1).Texture.ToString();
            data["ski_hair_palette"] = skin.GetComponent(1).Palette.ToString();
            data["ski_torso_drawable"] = skin.GetComponent(2).Drawable.ToString();
            data["ski_torso_texture"] = skin.GetComponent(2).Texture.ToString();
            data["ski_torso_palette"] = skin.GetComponent(2).Palette.ToString();
            data["ski_leg_drawable"] = skin.GetComponent(3).Drawable.ToString();
            data["ski_leg_texture"] = skin.GetComponent(3).Texture.ToString();
            data["ski_leg_palette"] = skin.GetComponent(3).Palette.ToString();
            data["ski_bag_drawable"] = skin.GetComponent(4).Drawable.ToString();
            data["ski_bag_texture"] = skin.GetComponent(4).Texture.ToString();
            data["ski_bag_palette"] = skin.GetComponent(4).Palette.ToString();
            data["ski_feet_drawable"] = skin.GetComponent(5).Drawable.ToString();
            data["ski_feet_texture"] = skin.GetComponent(5).Texture.ToString();
            data["ski_feet_palette"] = skin.GetComponent(5).Palette.ToString();
            data["ski_accessoiries_drawable"] = skin.GetComponent(6).Drawable.ToString();
            data["ski_accessoiries_texture"] = skin.GetComponent(6).Texture.ToString();
            data["ski_accessoiries_palette"] = skin.GetComponent(6).Palette.ToString();
            data["ski_undershirt_drawable"] = skin.GetComponent(7).Drawable.ToString();
            data["ski_undershirt_texture"] = skin.GetComponent(7).Texture.ToString();
            data["ski_undershirt_palette"] = skin.GetComponent(7).Palette.ToString();
            data["ski_bodyarmor_drawable"] = skin.GetComponent(8).Drawable.ToString();
            data["ski_bodyarmor_texture"] = skin.GetComponent(8).Texture.ToString();
            data["ski_bodyarmor_palette"] = skin.GetComponent(8).Palette.ToString();
            data["ski_decal_drawable"] = skin.GetComponent(9).Drawable.ToString();
            data["ski_decal_texture"] = skin.GetComponent(9).Texture.ToString();
            data["ski_decal_palette"] = skin.GetComponent(9).Palette.ToString();
            data["ski_top_drawable"] = skin.GetComponent(10).Drawable.ToString();
            data["ski_top_texture"] = skin.GetComponent(10).Drawable.ToString();
            data["ski_top_palette"] = skin.GetComponent(10).Drawable.ToString();
            return data;
        }

        public override void SetData(Skin skin, Dictionary<string, string> data)
        {
            skin.Model = (PedModel)Enum.Parse(typeof(PedModel), data["ski_model"]);
            skin.SetComponent(0, new Component(data, "ski_hair"));
            skin.SetComponent(1, new Component(data, "ski_mask"));
            skin.SetComponent(2, new Component(data, "ski_torso"));
            skin.SetComponent(3, new Component(data, "ski_leg"));
            skin.SetComponent(4, new Component(data, "ski_bag"));
            skin.SetComponent(5, new Component(data, "ski_feet"));
            skin.SetComponent(6, new Component(data, "ski_accessoiries"));
            skin.SetComponent(7, new Component(data, "ski_undershirt"));
            skin.SetComponent(8, new Component(data, "ski_bodyarmor"));
            skin.SetComponent(9, new Component(data, "ski_decal"));
            skin.SetComponent(10, new Component(data, "ski_top"));
        }
    }
}
