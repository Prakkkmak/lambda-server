using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AltV.Net.Data;
using Items;
using Lambda.Administration;
using Lambda.Database;

namespace Lambda.Items
{
    public class ComponentLink
    {
        public enum Valid
        {
            UNKNOW,
            TRUE,
            FALSE
        }

        private uint id;
        public Valid Validity;
        public readonly Link Link;
        public readonly uint DrawableA;
        public readonly uint DrawableB;

        public ComponentLink(Link link, uint drawableA, uint drawableB, Valid isValid)
        {
            Link = link;
            DrawableA = drawableA;
            DrawableB = drawableB;
            Validity = isValid;
        }

        public ComponentLink(Dictionary<string, string> datas)
        {
            id = uint.Parse(datas["lin_id"]);
            Link = new Link(uint.Parse(datas["lin_from"]), uint.Parse(datas["lin_to"]));
            DrawableA = uint.Parse(datas["lin_fromdrawable"]);
            DrawableB = uint.Parse(datas["lin_todrawable"]);
            Validity = (Valid)int.Parse(datas["lin_valid"]);
        }


        public static bool Equals(ComponentLink componentLinkA, ComponentLink componentLinkB)
        {
            if (!Link.Equals(componentLinkA.Link, componentLinkB.Link)) return false;
            if (componentLinkA.DrawableA != componentLinkB.DrawableA) return false;
            if (componentLinkA.DrawableB != componentLinkB.DrawableB) return false;
            return true;
        }

        public static ComponentLink GetComponentLink(ComponentLink componentLink)
        {
            foreach (ComponentLink _componentLink in ComponentLinks)
            {
                if (Equals(componentLink, _componentLink)) return _componentLink;
            }

            return null;
        }

        public static ComponentLink[] FindExistedComponentLinks(ComponentLink[] componentLinks)
        {
            List<ComponentLink> similarities = new List<ComponentLink>();
            foreach (ComponentLink componentLink in componentLinks)
            {
                ComponentLink comp = GetComponentLink(componentLink);

                if (comp != null) similarities.Add(comp);
            }

            return similarities.ToArray();
        }

        public static void AddSkinToComponentsLinks(Skin skin, Valid validity)
        {
            ComponentLink[] componentLinks = ExtractSkinLinks(skin, validity);
            foreach (ComponentLink componentLink in componentLinks)
            {
                ComponentLink comp = GetComponentLink(componentLink);
                if (comp != null)
                {
                    comp.Validity = validity;
                }
                else
                {
                    ComponentLinks.Add(componentLink);
                }
            }


        }

        public static ComponentLink[] ExtractSkinLinks(Skin skin, Valid validity)
        {
            List<ComponentLink> links = new List<ComponentLink>();
            foreach (Link link in Link.Links)
            {
                links.Add(new ComponentLink(link, skin.GetComponent(link.From).Drawable, skin.GetComponent(link.To).Drawable, validity));
            }
            return links.ToArray();
        }

        public void Save()
        {
            id = (uint)Insert();
        }



        public static void SaveAll()
        {
            //Delete();
            foreach (ComponentLink componentLink in ComponentLinks)
            {
                if (componentLink.id == 0)
                {
                    componentLink.Save();
                }
                else
                {
                    componentLink.Update();
                }
            }
        }

        public static void GenerateFalseComponentLink(Skin badSkin, bool isSetByPlayer = false)
        {
            ComponentLink[] componentLink = ExtractSkinLinks(badSkin, Valid.UNKNOW);
            List<ComponentLink> badComponentLinks = new List<ComponentLink>();
            foreach (ComponentLink comp in componentLink)
            {
                ComponentLink storedComp = GetComponentLink(comp);
                if (storedComp == null)
                {
                    badComponentLinks.Add(comp);
                    ComponentLinks.Add(comp);

                }
                else if (storedComp.Validity != Valid.TRUE)
                {
                    badComponentLinks.Add(storedComp);
                }
            }

            if (badComponentLinks.Count == 1) // Case where all components are true except this one
            {
                badComponentLinks[0].Validity = Valid.FALSE;

            }

            if (badComponentLinks.Count == 0)
            {
                foreach (ComponentLink comp in componentLink)
                {
                    comp.Validity = Valid.UNKNOW;
                }

            }
        }

        public static void GenerateFalseComponentLinks()
        {
            foreach (Skin badSkin in Skin.BadSkins)
            {
                GenerateFalseComponentLink(badSkin);

            }
        }

        #region database
        private Dictionary<string, string> GetComponentLinkData()
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            int i = 0;
            datas["lin_valid"] = ((uint)Validity).ToString();
            datas["lin_from"] = Link.From.ToString();
            datas["lin_to"] = Link.To.ToString();
            datas["lin_fromdrawable"] = DrawableA.ToString();
            datas["lin_todrawable"] = DrawableB.ToString();
            return datas;

        }

        private static void Delete()
        {
            //DbConnect.Delete(TableName, new Dictionary<string, string>());
        }

        private long Insert()
        {
            /*Dictionary<string, string> datas = GetComponentLinkData();
            DBConnect dbConnect = DBConnect.DbConnect;
            return dbConnect.Insert(TableName, datas);*/
            return 0;
        }

        private void Update()
        {
            /*Dictionary<string, string> datas = GetComponentLinkData();
            Dictionary<string, string> wheres = new Dictionary<string, string>();
            wheres["lin_id"] = id.ToString();
            DBConnect dbConnect = DBConnect.DbConnect;
            dbConnect.Update(TableName, datas, wheres);*/
        }

        public static void LoadAllComponentLinks()
        {
            /*DBConnect dbConnect = DBConnect.DbConnect;
            List<Dictionary<string, string>> results = dbConnect.Select(TableName, new Dictionary<string, string>());
            foreach (Dictionary<string, string> result in results)
            {
                ComponentLink componentLink = new ComponentLink(result);
                ComponentLinks.Add(componentLink);

            }*/
        }



        #endregion

        public static List<ComponentLink> ComponentLinks = new List<ComponentLink>();
        public static string TableName = "t_link_lin";
    }
}
