using Items;
using Lambda.Items;
using System;
using System.Collections.Generic;
using System.Text;

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
            data["ski_valid"] = (skin.Valid ? 1 : 0).ToString();
            data["ski_type"] = skin.Type;
            data["ski_model"] = skin.Model;
            data["ski_mask_drawable"] = skin.Mask.Drawable.ToString();
            data["ski_mask_texture"] = skin.Mask.Texture.ToString();
            data["ski_mask_palette"] = skin.Mask.Palette.ToString();
            data["ski_hair_drawable"] = skin.Hair.Drawable.ToString();
            data["ski_hair_texture"] = skin.Hair.Texture.ToString();
            data["ski_hair_palette"] = skin.Hair.Palette.ToString();
            data["ski_torso_drawable"] = skin.Torso.Drawable.ToString();
            data["ski_torso_texture"] = skin.Torso.Texture.ToString();
            data["ski_torso_palette"] = skin.Torso.Palette.ToString();
            data["ski_leg_drawable"] = skin.Leg.Drawable.ToString();
            data["ski_leg_texture"] = skin.Leg.Palette.ToString();
            data["ski_leg_palette"] = skin.Leg.Texture.ToString();
            data["ski_bag_drawable"] = skin.Bag.Drawable.ToString();
            data["ski_bag_texture"] = skin.Bag.Texture.ToString();
            data["ski_bag_palette"] = skin.Bag.Palette.ToString();
            data["ski_feet_drawable"] = skin.Feet.Drawable.ToString();
            data["ski_feet_texture"] = skin.Feet.Texture.ToString();
            data["ski_feet_palette"] = skin.Feet.Palette.ToString();
            data["ski_accessoiries_drawable"] = skin.Accessoiries.Drawable.ToString();
            data["ski_accessoiries_texture"] = skin.Accessoiries.Texture.ToString();
            data["ski_accessoiries_palette"] = skin.Accessoiries.Palette.ToString();
            data["ski_undershirt_drawable"] = skin.Undershirt.Drawable.ToString();
            data["ski_undershirt_texture"] = skin.Undershirt.Texture.ToString();
            data["ski_undershirt_palette"] = skin.Undershirt.Palette.ToString();
            data["ski_bodyarmor_drawable"] = skin.BodyArmor.Drawable.ToString();
            data["ski_bodyarmor_texture"] = skin.BodyArmor.Texture.ToString();
            data["ski_bodyarmor_palette"] = skin.BodyArmor.Palette.ToString();
            data["ski_decal_drawable"] = skin.Decal.Drawable.ToString();
            data["ski_decal_texture"] = skin.Decal.Texture.ToString();
            data["ski_decal_palette"] = skin.Decal.Palette.ToString();
            data["ski_top_drawable"] = skin.Top.Drawable.ToString();
            data["ski_top_texture"] = skin.Top.Drawable.ToString();
            data["ski_top_palette"] = skin.Top.Drawable.ToString();
            return data;
        }

        public override void SetData(Skin skin, Dictionary<string, string> data)
        {
            skin.Model = data["ski_model"];
            skin.Mask = new Component(data, "ski_mask");
            skin.Hair = new Component(data, "ski_hair");
            skin.Torso = new Component(data, "ski_torso");
            skin.Leg = new Component(data, "ski_leg");
            skin.Bag = new Component(data, "ski_bag");
            skin.Feet = new Component(data, "ski_feet");
            skin.Accessoiries = new Component(data, "ski_accessoiries");
            skin.Undershirt = new Component(data, "ski_undershirt");
            skin.BodyArmor = new Component(data, "ski_bodyarmor");
            skin.Decal = new Component(data, "ski_decal");
            skin.Top = new Component(data, "ski_top");
        }
    }
}
