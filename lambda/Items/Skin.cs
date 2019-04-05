using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Administration;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;

namespace Items
{
    public class Skin
    {
        public enum ClothNumber
        {
            NONE,
            MASK,
            HAIR,
            TORSO,
            LEG,
            BAG,
            FEET,
            ACCESSOIRIES,
            UNDERSHIRT,
            BODYARMOR,
            DECAL,
            TOP
        }


        private string type; //Everyone can't take every closes.
        private string model; // The model associated with this skin
        private bool valid;

        private Component mask; // 1
        private Component hair; // 2
        private Component torso; // 3
        private Component leg; // 4
        private Component bag; // 5
        private Component feet; // 6
        private Component accessoiries; // 7
        private Component undershirt; // 8
        private Component bodyArmor; // 9
        private Component decal; // 10
        private Component top; // 11

        public uint Id;

        public Skin()
        {
            Id = 0;
            type = "DEFAULT";
            model = "mp_m_freemode_01";
            valid = true;
            for (uint i = 1; i < 12; i++)
            {
                SetComponent(i, 0);
            }

        }

        public Skin(uint[] components)
        {
            Id = 0;
            type = "DEFAULT";
            model = "mp_m_freemode_01";
            valid = true;
            for (uint i = 1; i < components.Length; i++)
            {
                SetComponent(i, components[i]);
            }
        }

        public Skin(Dictionary<string, string> datas)
        {

        }

        public void SetModel(string value)
        {
            model = value;
        }

        public void SetComponent(uint componentId, uint drawable, uint texture = 0, uint palette = 0)
        {
            if (componentId < 0) componentId = 0;
            switch (componentId)
            {
                case 1:
                    mask = new Component(drawable, texture, palette);
                    break;
                case 2:
                    hair = new Component(drawable, texture, palette);
                    break;
                case 3:
                    torso = new Component(drawable, texture, palette);
                    break;
                case 4:
                    leg = new Component(drawable, texture, palette);
                    break;
                case 5:
                    bag = new Component(drawable, texture, palette);
                    break;
                case 6:
                    feet = new Component(drawable, texture, palette);
                    break;
                case 7:
                    accessoiries = new Component(drawable, texture, palette);
                    break;
                case 8:
                    undershirt = new Component(drawable, texture, palette);
                    break;
                case 9:
                    bodyArmor = new Component(drawable, texture, palette);
                    break;
                case 10:
                    decal = new Component(drawable, texture, palette);
                    break;
                case 11:
                    top = new Component(drawable, texture, palette);
                    break;
            }
        }

        public Component GetComponent(uint componentId)
        {
            switch (componentId)
            {
                case 1:
                    return mask;

                case 2:
                    return hair;

                case 3:
                    return torso;

                case 4:
                    return leg;

                case 5:
                    return bag;

                case 6:
                    return feet;

                case 7:
                    return accessoiries;

                case 8:
                    return undershirt;

                case 9:
                    return bodyArmor;

                case 10:
                    return decal;

                case 11:
                    return top;
                default:
                    return new Component();

            }
        }

        public void SendSkin(Player player)
        {
            List<uint> clothes = new List<uint>();
            for (uint i = 1; i <= 11; i++)
            {
                clothes.Add(GetComponent(i).Drawable);
            }

            player.AltPlayer.Emit("setSkin", clothes.ToArray());
        }






        public static bool Equals(Skin skin1, Skin skin2)
        {
            if (!Component.Equals(skin1.mask, skin2.mask)) return false;
            if (!Component.Equals(skin1.hair, skin2.hair)) return false;
            if (!Component.Equals(skin1.torso, skin2.torso)) return false;
            if (!Component.Equals(skin1.leg, skin2.leg)) return false;
            if (!Component.Equals(skin1.bag, skin2.bag)) return false;
            if (!Component.Equals(skin1.feet, skin2.feet)) return false;
            if (!Component.Equals(skin1.accessoiries, skin2.accessoiries)) return false;
            if (!Component.Equals(skin1.undershirt, skin2.undershirt)) return false;
            if (!Component.Equals(skin1.bodyArmor, skin2.bodyArmor)) return false;
            if (!Component.Equals(skin1.decal, skin2.decal)) return false;
            if (!Component.Equals(skin1.top, skin2.top)) return false;
            return true;

        }

        public void Save()
        {
            if (Id == 0)
            {
                Id = (uint)Insert();
            }
            else
            {
                Update();
            }
        }

        public void Load()
        {
            Dictionary<string, string> datas = Select(Id);
            if (datas.Count == 0)
            {
                Id = (uint)Insert();
                return;
            }
            //}TODO type
            model = datas["ski_model"];
            mask = new Component(datas, "ski_mask");
            hair = new Component(datas, "ski_hair");
            torso = new Component(datas, "ski_torso");
            leg = new Component(datas, "ski_leg");
            bag = new Component(datas, "ski_bag");
            feet = new Component(datas, "ski_feet");
            accessoiries = new Component(datas, "ski_accessoiries");
            undershirt = new Component(datas, "ski_undershirt");
            bodyArmor = new Component(datas, "ski_bodyarmor");
            decal = new Component(datas, "ski_decal");
            top = new Component(datas, "ski_top");
        }

        public Player GetPlayer()
        {
            foreach (Player onlinePlayer in Player.OnlinePlayers)
            {
                if (onlinePlayer.Skin == this)
                {
                    return onlinePlayer;
                }
            }

            return null;
        }

        public Skin Copy()
        {
            Skin skin = new Skin();
            Dictionary<string, string> datas = GetSkinData();
            skin.model = datas["ski_model"];
            skin.mask = new Component(datas, "ski_mask");
            skin.hair = new Component(datas, "ski_hair");
            skin.torso = new Component(datas, "ski_torso");
            skin.leg = new Component(datas, "ski_leg");
            skin.bag = new Component(datas, "ski_bag");
            skin.feet = new Component(datas, "ski_feet");
            skin.accessoiries = new Component(datas, "ski_accessoiries");
            skin.undershirt = new Component(datas, "ski_undershirt");
            skin.bodyArmor = new Component(datas, "ski_bodyarmor");
            skin.decal = new Component(datas, "ski_decal");
            skin.top = new Component(datas, "ski_top");
            return skin;
        }

        public static int GenerateSkins()
        {
            ComponentLink[] links = ComponentLink.ComponentLinks.ToArray();
            List<ComponentLink> TopToLeg = new List<ComponentLink>();
            List<ComponentLink> FeetToLeg = new List<ComponentLink>();
            List<ComponentLink> HairToMask = new List<ComponentLink>();
            List<ComponentLink> TorsoToUndershirt = new List<ComponentLink>();
            List<ComponentLink> TorsoToTop = new List<ComponentLink>();
            List<ComponentLink> UndershirtToTop = new List<ComponentLink>();
            List<ComponentLink> UndershirtToLeg = new List<ComponentLink>();
            List<ComponentLink> MaskToTop = new List<ComponentLink>();
            List<Skin> goodskins = new List<Skin>();
            foreach (ComponentLink link in links)
            {
                if (Link.Equals(link.Link, Link.TopToLeg))
                {
                    TopToLeg.Add(link);
                }
                else if (Link.Equals(link.Link, Link.FeetToLeg))
                {
                    FeetToLeg.Add(link);
                }
                else if (Link.Equals(link.Link, Link.HairToMask))
                {
                    HairToMask.Add(link);
                }
                else if (Link.Equals(link.Link, Link.TorsoToUndershirt))
                {
                    TorsoToUndershirt.Add(link);
                }
                else if (Link.Equals(link.Link, Link.TorsoToTop))
                {
                    TorsoToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.UndershirtToTop))
                {
                    UndershirtToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.MaskToTop))
                {
                    MaskToTop.Add(link);
                }
                else if (Link.Equals(link.Link, Link.UndershirtToLeg))
                {
                    UndershirtToLeg.Add(link);
                }
            }

            foreach (ComponentLink feetToLeg in FeetToLeg)
            {
                bool isGoodSkin = true;
                if (feetToLeg.Validity != ComponentLink.Valid.TRUE) continue;
                //if (feetToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                foreach (ComponentLink topToLeg in TopToLeg)
                {
                    if (topToLeg.Validity != ComponentLink.Valid.TRUE ||
                        topToLeg.DrawableB != feetToLeg.DrawableB) continue;
                    //if (topToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                    foreach (ComponentLink torsoToTop in TorsoToTop)
                    {
                        if (torsoToTop.Validity != ComponentLink.Valid.TRUE ||
                            torsoToTop.DrawableB != topToLeg.DrawableA) continue;
                        //if (torsoToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                        foreach (ComponentLink torsoToUndershirt in TorsoToUndershirt)
                        {
                            if (torsoToUndershirt.Validity != ComponentLink.Valid.TRUE ||
                                torsoToUndershirt.DrawableA != torsoToTop.DrawableA) continue;
                            //if (torsoToUndershirt.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                            foreach (ComponentLink undershortToTop in UndershirtToTop)
                            {
                                //if (torsoToUndershirt.Link.To != undershortToTop.Link.From) continue;
                                if (undershortToTop.Validity != ComponentLink.Valid.TRUE ||
                                    undershortToTop.DrawableB != torsoToTop.DrawableB) continue;
                                // if (undershortToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                foreach (ComponentLink hairToMask in HairToMask)
                                {
                                    if (hairToMask.Validity != ComponentLink.Valid.TRUE) continue;
                                    // if (hairToMask.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                    foreach (ComponentLink maskToTop in MaskToTop)
                                    {
                                        if (maskToTop.Validity != ComponentLink.Valid.TRUE ||
                                            maskToTop.DrawableB != topToLeg.DrawableA) continue;
                                        // if (maskToTop.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;
                                        foreach (ComponentLink undershirtToLeg in UndershirtToLeg)
                                        {
                                            if (undershirtToLeg.Validity != ComponentLink.Valid.TRUE ||
                                                undershirtToLeg.DrawableA != undershortToTop.DrawableA) continue;
                                            //if (undershirtToLeg.Validity == ComponentLink.Valid.FALSE) isGoodSkin = false;

                                            Skin skin = new Skin();
                                            skin.feet = new Component(feetToLeg.DrawableA);
                                            skin.leg = new Component(feetToLeg.DrawableB);
                                            skin.top = new Component(topToLeg.DrawableA);
                                            skin.torso = new Component(torsoToTop.DrawableA);
                                            skin.undershirt = new Component(undershortToTop.DrawableA);
                                            skin.hair = new Component(hairToMask.DrawableA);
                                            skin.mask = new Component(hairToMask.DrawableB);
                                            if (isGoodSkin) goodskins.Add(skin);
                                            else BadSkins.Add(skin);

                                        }

                                    }


                                }
                            }
                        }
                    }
                }



            }
            GoodSkins = goodskins.ToArray();
            return GoodSkins.Length;
        }

        public static Skin Random()
        {
            if (GoodSkins.Length == 0) return new Skin();
            Random rnd = new Random();
            int r = rnd.Next(GoodSkins.Length);
            return GoodSkins[r];
        }
        //public static List<Skin> LastSkinDiscovered = new List<Skin>();
        public static Skin GetSkinToDiscover()
        {

            foreach (Skin goodSkin in GoodSkins)
            {
                Skin skinToDiscover = goodSkin.Copy();
                for (int nbrDiff = 1; nbrDiff <= 11; nbrDiff++)
                {
                    uint i;

                    for (i = 0; i < Component.HairMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.HAIR, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        ComponentLink[] links = skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW);
                        if (links.Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.MaskMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.MASK, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.TopMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.TOP, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.UndershirtMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.UNDERSHIRT, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.TorsoMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.TORSO, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.FeetMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.FEET, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                    for (i = 0; i < Component.LegMaxValue; i++)
                    {
                        skinToDiscover.SetComponent((uint)ClothNumber.LEG, i);
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.FALSE).Length > 0) continue;
                        if (skinToDiscover.GetLinksByType(ComponentLink.Valid.UNKNOW).Length == nbrDiff) return skinToDiscover;
                    }
                }

            }

            return null;
        }


        public ComponentLink[] GetLinksByType(ComponentLink.Valid type)
        {
            ComponentLink[] components = ComponentLink.ExtractSkinLinks(this, ComponentLink.Valid.UNKNOW);
            List<ComponentLink> links = new List<ComponentLink>();
            int nbrNotValidLink = 0;
            foreach (ComponentLink componentLink in components)
            {
                ComponentLink storedComponentLink = ComponentLink.GetComponentLink(componentLink);

                if (storedComponentLink == null)
                {
                    if (type != ComponentLink.Valid.UNKNOW) continue;
                    links.Add(componentLink);
                    continue;
                }
                if (storedComponentLink.Validity == type) links.Add(componentLink);

            }

            return links.ToArray();
        }

        #region database



        private Dictionary<string, string> GetSkinData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["ski_valid"] = (valid ? 1 : 0).ToString();
            datas["ski_type"] = type;
            datas["ski_model"] = model;
            datas["ski_mask_drawable"] = mask.Drawable.ToString();
            datas["ski_mask_texture"] = mask.Texture.ToString();
            datas["ski_mask_palette"] = mask.Palette.ToString();
            datas["ski_hair_drawable"] = hair.Drawable.ToString();
            datas["ski_hair_texture"] = hair.Texture.ToString();
            datas["ski_hair_palette"] = hair.Palette.ToString();
            datas["ski_torso_drawable"] = torso.Drawable.ToString();
            datas["ski_torso_texture"] = torso.Texture.ToString();
            datas["ski_torso_palette"] = torso.Palette.ToString();
            datas["ski_leg_drawable"] = leg.Drawable.ToString();
            datas["ski_leg_texture"] = leg.Palette.ToString();
            datas["ski_leg_palette"] = leg.Texture.ToString();
            datas["ski_bag_drawable"] = bag.Drawable.ToString();
            datas["ski_bag_texture"] = bag.Texture.ToString();
            datas["ski_bag_palette"] = bag.Palette.ToString();
            datas["ski_feet_drawable"] = feet.Drawable.ToString();
            datas["ski_feet_texture"] = feet.Texture.ToString();
            datas["ski_feet_palette"] = feet.Palette.ToString();
            datas["ski_accessoiries_drawable"] = accessoiries.Drawable.ToString();
            datas["ski_accessoiries_texture"] = accessoiries.Texture.ToString();
            datas["ski_accessoiries_palette"] = accessoiries.Palette.ToString();
            datas["ski_undershirt_drawable"] = undershirt.Drawable.ToString();
            datas["ski_undershirt_texture"] = undershirt.Texture.ToString();
            datas["ski_undershirt_palette"] = undershirt.Palette.ToString();
            datas["ski_bodyarmor_drawable"] = bodyArmor.Drawable.ToString();
            datas["ski_bodyarmor_texture"] = bodyArmor.Texture.ToString();
            datas["ski_bodyarmor_palette"] = bodyArmor.Palette.ToString();
            datas["ski_decal_drawable"] = decal.Drawable.ToString();
            datas["ski_decal_texture"] = decal.Texture.ToString();
            datas["ski_decal_palette"] = decal.Palette.ToString();
            datas["ski_top_drawable"] = top.Drawable.ToString();
            datas["ski_top_texture"] = top.Drawable.ToString();
            datas["ski_top_palette"] = top.Drawable.ToString();
            return datas;
        }

        private long Insert()
        {
            Dictionary<string, string> datas = GetSkinData();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Insert(TableName, datas);
        }

        private void Update()
        {

            Dictionary<string, string> datas = GetSkinData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["ski_id"] = Id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            dbConnect.Update(TableName, datas, wheres);

        }

        public static Dictionary<string, string> Select(uint id)
        {
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["ski_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);
        }

        public static void LoadAllSkins()
        {
            /*DBConnect dbConnect = DBConnect.DbConnect;
            List<Dictionary<string, string>> results = dbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                Skin skin = new Skin(result);
                SkinsSaved.Add(skin);
            }*/
        }

        #endregion


        public static string TableName = "t_skin_ski";
        public static Skin[] GoodSkins = new Skin[0];
        public static List<Skin> BadSkins = new List<Skin>();
    }
}