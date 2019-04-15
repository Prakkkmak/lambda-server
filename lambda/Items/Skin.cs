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
    public class Skin : IDBElement
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


        public string Type { get; set; } //Everyone can't take every closes.
        public string Model { get; set; } // The model associated with this skin
        public bool Valid { get; set; }

        public Component Mask { get; set; } // 1
        public Component Hair { get; set; } // 2
        public Component Torso { get; set; } // 3
        public Component Leg { get; set; } // 4
        public Component Bag { get; set; } // 5
        public Component Feet { get; set; } // 6
        public Component Accessoiries { get; set; } // 7
        public Component Undershirt { get; set; } // 8
        public Component BodyArmor { get; set; } // 9
        public Component Decal { get; set; } // 10
        public Component Top { get; set; } // 11

        public uint Id;
        public Player Player { get; set; }
        uint IDBElement.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Skin()
        {
            Id = 0;
            Type = "DEFAULT";
            Model = "mp_m_freemode_01";
            Valid = true;
            for (uint i = 1; i < 12; i++)
            {
                SetComponent(i, 0);
            }

        }

        public Skin(uint[] components)
        {
            Id = 0;
            Type = "DEFAULT";
            Model = "mp_m_freemode_01";
            Valid = true;
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
            Model = value;
        }

        public void SetComponent(uint componentId, uint drawable, uint texture = 0, uint palette = 0)
        {
            if (componentId < 0) componentId = 0;
            switch (componentId)
            {
                case 1:
                    Mask = new Component(drawable, texture, palette);
                    break;
                case 2:
                    Hair = new Component(drawable, texture, palette);
                    break;
                case 3:
                    Torso = new Component(drawable, texture, palette);
                    break;
                case 4:
                    Leg = new Component(drawable, texture, palette);
                    break;
                case 5:
                    Bag = new Component(drawable, texture, palette);
                    break;
                case 6:
                    Feet = new Component(drawable, texture, palette);
                    break;
                case 7:
                    Accessoiries = new Component(drawable, texture, palette);
                    break;
                case 8:
                    Undershirt = new Component(drawable, texture, palette);
                    break;
                case 9:
                    BodyArmor = new Component(drawable, texture, palette);
                    break;
                case 10:
                    Decal = new Component(drawable, texture, palette);
                    break;
                case 11:
                    Top = new Component(drawable, texture, palette);
                    break;
            }
        }

        public Component GetComponent(uint componentId)
        {
            switch (componentId)
            {
                case 1:
                    return Mask;

                case 2:
                    return Hair;

                case 3:
                    return Torso;

                case 4:
                    return Leg;

                case 5:
                    return Bag;

                case 6:
                    return Feet;

                case 7:
                    return Accessoiries;

                case 8:
                    return Undershirt;

                case 9:
                    return BodyArmor;

                case 10:
                    return Decal;

                case 11:
                    return Top;
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
            if (!Component.Equals(skin1.Mask, skin2.Mask)) return false;
            if (!Component.Equals(skin1.Hair, skin2.Hair)) return false;
            if (!Component.Equals(skin1.Torso, skin2.Torso)) return false;
            if (!Component.Equals(skin1.Leg, skin2.Leg)) return false;
            if (!Component.Equals(skin1.Bag, skin2.Bag)) return false;
            if (!Component.Equals(skin1.Feet, skin2.Feet)) return false;
            if (!Component.Equals(skin1.Accessoiries, skin2.Accessoiries)) return false;
            if (!Component.Equals(skin1.Undershirt, skin2.Undershirt)) return false;
            if (!Component.Equals(skin1.BodyArmor, skin2.BodyArmor)) return false;
            if (!Component.Equals(skin1.Decal, skin2.Decal)) return false;
            if (!Component.Equals(skin1.Top, skin2.Top)) return false;
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
            Model = datas["ski_model"];
            Mask = new Component(datas, "ski_mask");
            Hair = new Component(datas, "ski_hair");
            Torso = new Component(datas, "ski_torso");
            Leg = new Component(datas, "ski_leg");
            Bag = new Component(datas, "ski_bag");
            Feet = new Component(datas, "ski_feet");
            Accessoiries = new Component(datas, "ski_accessoiries");
            Undershirt = new Component(datas, "ski_undershirt");
            BodyArmor = new Component(datas, "ski_bodyarmor");
            Decal = new Component(datas, "ski_decal");
            Top = new Component(datas, "ski_top");
        }


        public Skin Copy()
        {
            Skin skin = new Skin();
            Dictionary<string, string> datas = GetSkinData();
            skin.Model = datas["ski_model"];
            skin.Mask = new Component(datas, "ski_mask");
            skin.Hair = new Component(datas, "ski_hair");
            skin.Torso = new Component(datas, "ski_torso");
            skin.Leg = new Component(datas, "ski_leg");
            skin.Bag = new Component(datas, "ski_bag");
            skin.Feet = new Component(datas, "ski_feet");
            skin.Accessoiries = new Component(datas, "ski_accessoiries");
            skin.Undershirt = new Component(datas, "ski_undershirt");
            skin.BodyArmor = new Component(datas, "ski_bodyarmor");
            skin.Decal = new Component(datas, "ski_decal");
            skin.Top = new Component(datas, "ski_top");
            return skin;
        }

        public static int GenerateSkins()
        {
            return 0;
            /*ComponentLink[] links = ComponentLink.ComponentLinks.ToArray();
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
                                            skin.Feet = new Component(feetToLeg.DrawableA);
                                            skin.Leg = new Component(feetToLeg.DrawableB);
                                            skin.Top = new Component(topToLeg.DrawableA);
                                            skin.Torso = new Component(torsoToTop.DrawableA);
                                            skin.Undershirt = new Component(undershortToTop.DrawableA);
                                            skin.Hair = new Component(hairToMask.DrawableA);
                                            skin.Mask = new Component(hairToMask.DrawableB);
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
            return GoodSkins.Length;*/
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

        /*
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

        */

        private long Insert()
        {
            /*Dictionary<string, string> datas = GetSkinData();
            return DbConnect.Insert(TableName, datas);*/
            return 0;
        }

        private void Update()
        {

            /*Dictionary<string, string> datas = GetSkinData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["ski_id"] = Id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            dbConnect.Update(TableName, datas, wheres);*/

        }

        public static Dictionary<string, string> Select(uint id)
        {
            /*Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["ski_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.SelectOne(TableName, wheres);*/
            return null;
        }


        #endregion


        public static string TableName = "t_skin_ski";
        public static Skin[] GoodSkins = new Skin[0];
        public static List<Skin> BadSkins = new List<Skin>();
    }
}