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

        public Player Player { get; set; }
        public uint Id { get; set; }

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

        public Skin(Player player, uint[] components)
        {
            this.Player = player;
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

            player.Game.DbSkin.Save(this);

            player.AltPlayer.Emit("setSkin", clothes.ToArray());
        }









        public Skin Copy()
        {
            Skin skin = new Skin();

            Dictionary<string, string> data = Player.Game.DbSkin.GetData(this);
            Player.Game.DbSkin.SetData(skin, data);
            /*
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
            skin.Top = new Component(datas, "ski_top");*/
            return skin;
        }


        //public static List<Skin> LastSkinDiscovered = new List<Skin>();
        /*public static Skin GetSkinToDiscover()
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
        }*/

        public ComponentLink[] ExtractLinks(ComponentLink.Valid validity)
        {
            List<ComponentLink> links = new List<ComponentLink>();
            foreach (Link link in Link.Links)
            {
                links.Add(new ComponentLink(link, this.GetComponent(link.From).Drawable, this.GetComponent(link.To).Drawable, validity));
            }
            return links.ToArray();
        }
        public ComponentLink[] GetLinksByType(ComponentLink.Valid type)
        {
            ComponentLink[] components = ExtractLinks(ComponentLink.Valid.UNKNOW);
            List<ComponentLink> links = new List<ComponentLink>();
            int nbrNotValidLink = 0;
            foreach (ComponentLink componentLink in components)
            {
                ComponentLink storedComponentLink = Player.Game.GetComponentLink(componentLink);

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
        public void AddToComponentsLinks(ComponentLink.Valid validity)
        {
            ComponentLink[] componentLinks = ExtractLinks(validity);
            foreach (ComponentLink componentLink in componentLinks)
            {
                ComponentLink comp = Player.Game.GetComponentLink(componentLink);
                if (comp != null)
                {
                    comp.Validity = validity;
                    Player.Game.DbComponentLink.Save(comp);
                }
                else
                {
                    Player.Game.AddComponentLink(componentLink);
                    Player.Game.DbComponentLink.Save(componentLink);
                }

            }
        }
        public void GenerateFalseComponentLink(bool isSetByPlayer = false)
        {
            ComponentLink[] componentLink = ExtractLinks(ComponentLink.Valid.UNKNOW);
            List<ComponentLink> badComponentLinks = new List<ComponentLink>();
            foreach (ComponentLink comp in componentLink)
            {
                ComponentLink storedComp = Player.Game.GetComponentLink(comp);
                if (storedComp == null)
                {
                    badComponentLinks.Add(comp);
                    Player.Game.AddComponentLink(comp);
                    //TODO DEPLACER le save autre part
                    Player.Game.DbComponentLink.Save(comp);

                }
                else if (storedComp.Validity != ComponentLink.Valid.TRUE)
                {
                    badComponentLinks.Add(storedComp);

                }
            }

            if (badComponentLinks.Count == 1) // Case where all components are true except this one
            {
                badComponentLinks[0].Validity = ComponentLink.Valid.FALSE;
                //TODO DEPLACER le save autre part
                Player.Game.DbComponentLink.Save(badComponentLinks[0]);

            }

            if (badComponentLinks.Count == 0)
            {
                foreach (ComponentLink comp in componentLink)
                {
                    comp.Validity = ComponentLink.Valid.UNKNOW;
                    ComponentLink storedComp = Player.Game.GetComponentLink(comp);
                    //TODO DEPLACER le save autre part
                    Player.Game.DbComponentLink.Save(storedComp);
                }

            }
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
    }

}