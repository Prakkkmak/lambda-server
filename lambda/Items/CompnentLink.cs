using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using AltV.Net.Data;
using Items;
using Lambda.Administration;
using Lambda.Database;

namespace Lambda.Items
{
    public class ComponentLink : IDBElement
    {
        public enum Valid
        {
            UNKNOW,
            TRUE,
            FALSE
        }
        public uint Id { get; set; }
        public Valid Validity;
        public Link Link;
        public uint DrawableA;
        public uint DrawableB;

        public ComponentLink()
        {

        }

        public ComponentLink(Link link, uint drawableA, uint drawableB, Valid isValid) : this()
        {
            Link = link;
            DrawableA = drawableA;
            DrawableB = drawableB;
            Validity = isValid;
        }



        public static bool Equals(ComponentLink componentLinkA, ComponentLink componentLinkB)
        {
            if (!Link.Equals(componentLinkA.Link, componentLinkB.Link)) return false;
            if (componentLinkA.DrawableA != componentLinkB.DrawableA) return false;
            if (componentLinkA.DrawableB != componentLinkB.DrawableB) return false;
            return true;
        }






        /*public static void GenerateFalseComponentLink(Skin badSkin, bool isSetByPlayer = false)
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

        



        #endregion

        public static List<ComponentLink> ComponentLinks = new List<ComponentLink>();
        public static string TableName = "t_link_lin";*/


    }
}
