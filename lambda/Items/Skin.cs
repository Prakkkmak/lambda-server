using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Enums;
using Lambda;
using Lambda.Administration;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using MoreLinq;

namespace Items
{
    public class Skin : IDBElement, IEquatable<Skin>
    {
        public uint Id { get; set; }


        public enum ClothNumber
        {
            HEAD,
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

        public Component[] Components;
        public Component[] Props;
        public float[] HeadData;
        public float[] Features;
        public Component[] Overlays;
        public uint HairColor = 0;
        public uint HairTaint = 0;
        public uint EyeColor = 0;
        public string Type;
        public PedModel Model;

        public Skin()
        {
            Components = new Component[COMPONENT_LENGTH];
            Props = new Component[PROP_LENGTH];
            HeadData = new float[6];
            Features = new float[20];
            Overlays = new Component[13];
            Type = "DEFAULT";
            Model = PedModel.FreemodeMale01;
        }

        public bool Equals(Skin skin)
        {
            foreach (Component component in Components)
            {
                foreach (Component skinComponent in skin.Components)
                {
                    if (!component.Equals(skinComponent))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int code = 1;
            foreach (Component component in Components)
            {
                code *= component.Drawable + 1;
            }
            //Calculate the hash code for the product. 
            //Alt.Log(code + "");
            return code;
        }

        public void SetComponent(byte i, Component comp)
        {
            if (i < COMPONENT_LENGTH)
            {
                Components[i] = comp;
            }

        }


        public Component GetComponent(byte i)
        {
            if (i < COMPONENT_LENGTH)
            {
                return Components[i];
            }
            else return new Component();
        }

        public string HeadDataToString()
        {
            string str = "";
            foreach (float f in HeadData)
            {
                str += f + ",";
            }
            if (str.Length > 0)
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }

        public void SetHeadDataString(string str)
        {
            if (str.Length == 0) return;
            string[] vals = str.Split(',');
            for (int i = 0; i < HeadData.Length; i++)
            {
                if (i >= vals.Length) return;
                HeadData[i] = float.Parse(vals[i]);
            }

        }

        public string OverlaysToString()
        {
            string str = "";
            foreach (Component component in Overlays)
            {
                str += component.Drawable + ",";
                str += component.Texture + ",";
                str += component.Palette + ",";
            }
            if (str.Length > 0)
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }

        public void SetOverlays(string str)
        {
            if (str.Length == 0) return;
            string[] vals = str.Split(',');
            for (int i = 0; i < 13; i++)
            {
                if (i >= vals.Length) return;
                ushort drawable = ushort.Parse(vals[i * 3]);
                ushort texture = ushort.Parse(vals[i * 3 + 1]);
                ushort palette = ushort.Parse(vals[i * 3 + 2]);

                Component comp = new Component(drawable, texture, palette);
                Overlays[i] = comp;
            }
        }

        public void SetFeatures(string str)
        {
            if (str.Length == 0) return;
            string[] features = str.Split(',');
            for (int i = 0; i < features.Length; i++)
            {
                Features[i] = float.Parse(features[i]);
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (Component component in Components)
            {
                str += component.Drawable + ",";
                str += component.Texture + ",";
                str += component.Palette + ",";
            }

            foreach (Component prop in Props)
            {
                str += prop.Drawable + ",";
                str += prop.Texture + ",";
            }

            if (str.Length > 0)
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }

        public void SetString(string str)
        {
            if (str.Length == 0) return;
            string[] vals = str.Split(',');
            for (int i = 0; i < COMPONENT_LENGTH; i++)
            {
                if (i >= vals.Length) return;
                ushort drawable = ushort.Parse(vals[i * 3]);
                ushort texture = ushort.Parse(vals[i * 3 + 1]);
                ushort palette = ushort.Parse(vals[i * 3 + 2]);

                Component comp = new Component(drawable, texture, palette);
                SetComponent((byte)i, comp);
            }

            for (int i = 0; i < Props.Length; i++)
            {
                if (COMPONENT_LENGTH * 3 + (i * 2) >= vals.Length) return;
                Alt.Log(COMPONENT_LENGTH * 3 + "");
                ushort drawable = ushort.Parse(vals[(COMPONENT_LENGTH * 3) + (i * 2)]);
                ushort texture = ushort.Parse(vals[(COMPONENT_LENGTH * 3) + (i * 2 + 1)]);
                Component comp = new Component(drawable, texture);
                Props[i] = comp;
            }
        }

        public void SendModel(Player player)
        {
            //if (!Enum.TryParse(player.GetSkin().Model, true, out PedModel model)) model = PedModel.FreemodeMale01;
            //if (!Enum.IsDefined(typeof(PedModel), model)) model = PedModel.FreemodeMale01;
            player.Model = (uint)Model;
            Alt.Log("Model set en " + (uint)Model);
        }

        public void SendSkin(Player player)
        {
            List<uint> clothes = new List<uint>();
            for (byte i = 0; i <= COMPONENT_LENGTH; i++)
            {
                clothes.Add(GetComponent(i).Drawable);
                clothes.Add(GetComponent(i).Texture);
                clothes.Add(GetComponent(i).Palette);
            }
            //player.Game.DbSkin.Save(this);
            player.Emit("setComponents", clothes.ToArray());
            List<uint> props = new List<uint>();
            for (byte i = 0; i < Props.Length; i++)
            {
                props.Add(Props[i].Drawable);
                props.Add(Props[i].Texture);
            }
            player.Emit("setProps", props.ToArray());
            player.Emit("setHeadData", HeadData);
            player.Emit("setHairColor", HairColor, HairTaint);
            player.Emit("setEyeColor", EyeColor);
            player.Emit("setFaceFeatures", Features);
            List<uint> overlays = new List<uint>();
            for (byte i = 0; i < Overlays.Length; i++)
            {
                overlays.Add(Overlays[i].Drawable);
                overlays.Add(Overlays[i].Texture);
                overlays.Add(Overlays[i].Palette);
            }
            player.Emit("setHeadOverlays", overlays);
        }

        public Dictionary<string, string> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //data["ski_valid"] = (skin.Valid ? 1 : 0).ToString();
            data["ski_type"] = Type;
            data["ski_model"] = Model.ToString();
            data["ski_mask_drawable"] = GetComponent((int)Skin.ClothNumber.MASK).Drawable.ToString();
            data["ski_mask_texture"] = GetComponent((int)Skin.ClothNumber.MASK).Texture.ToString();
            data["ski_mask_palette"] = GetComponent((int)Skin.ClothNumber.MASK).Palette.ToString();
            data["ski_hair_drawable"] = GetComponent((int)Skin.ClothNumber.HAIR).Drawable.ToString();
            data["ski_hair_texture"] = GetComponent((int)Skin.ClothNumber.HAIR).Texture.ToString();
            data["ski_hair_palette"] = GetComponent((int)Skin.ClothNumber.HAIR).Palette.ToString();
            data["ski_torso_drawable"] = GetComponent((int)Skin.ClothNumber.TORSO).Drawable.ToString();
            data["ski_torso_texture"] = GetComponent((int)Skin.ClothNumber.TORSO).Texture.ToString();
            data["ski_torso_palette"] = GetComponent((int)Skin.ClothNumber.TORSO).Palette.ToString();
            data["ski_leg_drawable"] = GetComponent((int)Skin.ClothNumber.LEG).Drawable.ToString();
            data["ski_leg_texture"] = GetComponent((int)Skin.ClothNumber.LEG).Texture.ToString();
            data["ski_leg_palette"] = GetComponent((int)Skin.ClothNumber.LEG).Palette.ToString();
            data["ski_bag_drawable"] = GetComponent((int)Skin.ClothNumber.BAG).Drawable.ToString();
            data["ski_bag_texture"] = GetComponent((int)Skin.ClothNumber.BAG).Texture.ToString();
            data["ski_bag_palette"] = GetComponent((int)Skin.ClothNumber.BAG).Palette.ToString();
            data["ski_feet_drawable"] = GetComponent((int)Skin.ClothNumber.FEET).Drawable.ToString();
            data["ski_feet_texture"] = GetComponent((int)Skin.ClothNumber.FEET).Texture.ToString();
            data["ski_feet_palette"] = GetComponent((int)Skin.ClothNumber.FEET).Palette.ToString();
            data["ski_accessoiries_drawable"] = GetComponent((int)Skin.ClothNumber.ACCESSOIRIES).Drawable.ToString();
            data["ski_accessoiries_texture"] = GetComponent((int)Skin.ClothNumber.ACCESSOIRIES).Texture.ToString();
            data["ski_accessoiries_palette"] = GetComponent((int)Skin.ClothNumber.ACCESSOIRIES).Palette.ToString();
            data["ski_undershirt_drawable"] = GetComponent((int)Skin.ClothNumber.UNDERSHIRT).Drawable.ToString();
            data["ski_undershirt_texture"] = GetComponent((int)Skin.ClothNumber.UNDERSHIRT).Texture.ToString();
            data["ski_undershirt_palette"] = GetComponent((int)Skin.ClothNumber.UNDERSHIRT).Palette.ToString();
            data["ski_bodyarmor_drawable"] = GetComponent((int)Skin.ClothNumber.BODYARMOR).Drawable.ToString();
            data["ski_bodyarmor_texture"] = GetComponent((int)Skin.ClothNumber.BODYARMOR).Texture.ToString();
            data["ski_bodyarmor_palette"] = GetComponent((int)Skin.ClothNumber.BODYARMOR).Palette.ToString();
            data["ski_decal_drawable"] = GetComponent((int)Skin.ClothNumber.DECAL).Drawable.ToString();
            data["ski_decal_texture"] = GetComponent((int)Skin.ClothNumber.DECAL).Texture.ToString();
            data["ski_decal_palette"] = GetComponent((int)Skin.ClothNumber.DECAL).Palette.ToString();
            data["ski_top_drawable"] = GetComponent((int)Skin.ClothNumber.TOP).Drawable.ToString();
            data["ski_top_texture"] = GetComponent((int)Skin.ClothNumber.TOP).Drawable.ToString();
            data["ski_top_palette"] = GetComponent((int)Skin.ClothNumber.TOP).Drawable.ToString();
            return data;
        }

        public void SetData(Dictionary<string, string> data)
        {
            Model = (PedModel)Enum.Parse(typeof(PedModel), data["ski_model"]);
            SetComponent((int)Skin.ClothNumber.MASK, new Component(data, "ski_mask"));
            SetComponent((int)Skin.ClothNumber.HAIR, new Component(data, "ski_hair"));
            SetComponent((int)Skin.ClothNumber.TORSO, new Component(data, "ski_torso"));
            SetComponent((int)Skin.ClothNumber.LEG, new Component(data, "ski_leg"));
            SetComponent((int)Skin.ClothNumber.BAG, new Component(data, "ski_bag"));
            SetComponent((int)Skin.ClothNumber.FEET, new Component(data, "ski_feet"));
            SetComponent((int)Skin.ClothNumber.ACCESSOIRIES, new Component(data, "ski_accessoiries"));
            SetComponent((int)Skin.ClothNumber.UNDERSHIRT, new Component(data, "ski_undershirt"));
            SetComponent((int)Skin.ClothNumber.BODYARMOR, new Component(data, "ski_bodyarmor"));
            SetComponent((int)Skin.ClothNumber.DECAL, new Component(data, "ski_decal"));
            SetComponent((int)Skin.ClothNumber.TOP, new Component(data, "ski_top"));
        }

        public void Save()
        {

            long t = DateTime.Now.Ticks;
            DatabaseElement.Save(this);
            Alt.Log("Skin Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            long t = DateTime.Now.Ticks;
            await DatabaseElement.SaveAsync(this);
            Alt.Log("Skin Saved en " + (t / TimeSpan.TicksPerMillisecond) + " ms ");
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        /*
        public static Skin GetRandomSkin(Game game)
        {
            Skin skin = new Skin(game);
            Random random = new Random();
            for (byte i = 0; i < Component.MaxValues.Length; i++)
            {
                Component comp = new Component((ushort)random.Next((int)Component.MaxValues[i]));
                skin.SetComponent(i, comp);
            }

            return skin;
        }

        public static bool IsTested(Link link, List<Link> currents)
        {
            foreach (Link current in currents)
            {
                if (link.Component1.Item1 == current.Component1.Item1 &&
                    link.Component2.Item1 == current.Component2.Item1) return true;
            }

            return false;
        }

        public static List<Link> RemoveIncompatibleLinks(Link linkToTest, List<Link> links)
        {
            List<Link> newLinks = new List<Link>();
            foreach (Link link in links)
            {
                if (link.Component1.Item1 == linkToTest.Component1.Item1 &&
                    link.Component2.Item1 == linkToTest.Component2.Item1)
                {
                    continue;
                }
                if (link.Component1.Item1 == linkToTest.Component1.Item1)
                {
                    if (link.Component1.Item2 != linkToTest.Component1.Item2) continue;
                }
                if (link.Component2.Item1 == linkToTest.Component2.Item1)
                {
                    if (link.Component2.Item2 != linkToTest.Component2.Item2) continue;
                }
                newLinks.Add(link);
            }

            return newLinks;
        }

        public static List<Skin> GetCompatibleSkins(List<Link> links, int size)
        {
            if (links.Count > 33) Alt.Log(links.Count + " count ");
            List<Skin> skins = new List<Skin>();
            if (size >= Link.LinksPairs.Length)
            {
                skins.Add(new Skin());
                return skins;
            }
            if (links.Count == 0) return null;
            foreach (Link link in links)
            {
                List<Link> newLinks = RemoveIncompatibleLinks(link, links);
                if (newLinks.Count == links.Count)
                {
                    Alt.Log("probleme");
                }
                List<Skin> skinsList = GetCompatibleSkins(newLinks, size + 1);
                if (skinsList == null) continue;
                foreach (Skin skin in skinsList)
                {
                    skin.SetComponent(link.Component1.Item1, new Component(link.Component1.Item2));
                    skin.SetComponent(link.Component2.Item1, new Component(link.Component2.Item2));
                }

                skins.AddRange(skinsList);
            }

            return skins.Distinct().ToList();

        }

        public static Skin[] GetSampleGoodSkin(List<Link> links, int nbr = 100000)
        {

            /*links = links.Distinct().ToList();
            foreach (Link link in links)
            {
                foreach (Link link1 in links)
                {
                    if (link != link1)
                    {
                        if (link.Component1.Equals(link1.Component1) && link.Component2.Equals(link1.Component2))
                        {
                            Alt.Log("mdr");
                        }
                    }
                }
            }
            Skin[] skins = GetCompatibleSkins(links, 0).ToArray();
            Alt.Log(skins.Length + " skins trouvés ");
            return skins;
    }

    public static Skin GetRandomGoodSkin(Game game, int maxtry = 1000000)
    {

        Skin[] skins = GetSampleGoodSkin(game.GetLinks().ToList().GetRange(1, 50), 50);
        Random rnd = new Random();
        if (skins.Length > 0)
        {
            return skins[rnd.Next(skins.Length)];
        }
        return null;
    }

    public static Skin GetSkinToDiscover(Game game)
    {
        Skin skin = GetRandomGoodSkin(game);
        for (int i = 0; i < skin.components.Length; i++)
        {
            Component comp = skin.components[i];
            for (int j = 0; j < Component.MaxValues[j]; j++)
            {
                comp.Drawable = (ushort)j;
                if (!skin.IsValid()) return skin;
            }
        }

        return null;
    }
        */
        public static ushort COMPONENT_LENGTH = 12;
        public static ushort PROP_LENGTH = 5;


    }
}