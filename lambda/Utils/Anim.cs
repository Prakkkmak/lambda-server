using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using AltV.Net;

namespace Lambda.Utils
{
    class Anim
    {
        public string Dictionary;
        public string Animation;

        public Anim(string dic, string anim)
        {
            Dictionary = dic;
            Animation = anim;
        }

        public static void RegisterAnims()
        {
            //string animText = Properties.Resources.Anims;
            string animText = File.ReadAllText("./resources/lambda/Resources/Anims");
            string[] animsStrings = animText.Split(new[] { '\r', '\n' });
            List<Anim> anims = new List<Anim>();

            foreach (string animsString in animsStrings)
            {
                if (string.IsNullOrWhiteSpace(animsString)) continue;
                string[] splited = animsString.Split(' ');

                Anim anim = new Anim(splited[0], splited[1]);
                anims.Add(anim);
            }

            Anims = anims.ToArray();
        }
        public static Anim[] Anims = new Anim[0];
    }
}
