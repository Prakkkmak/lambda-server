using System;
using System.Collections.Generic;
using System.Text;
using Lambda.Enums;
namespace Items
{
    public class Licenses
    {
        public uint Id { get; set; }
        public uint[] Values;

        public Licenses()
        {
            Values = new uint[Enum.GetNames(typeof(LicenseType)).Length];
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i] = 0;
            }
        }
        public Licenses(string str) : this()
        {
            string[] strs = str.Split(',');
            for (int i = 0; i < Values.Length; i++)
            {
                if (i < strs.Length)
                {

                    if (uint.TryParse(strs[i], out uint res)) Values[i] = res;
                    else Values[i] = 0;


                }
                else Values[i] = 0;
            }
        }

        public override string ToString()
        {
            string str = string.Join(',', Values);
            return str;
        }

        public void SetValue(LicenseType type, uint value)
        {
            Values[(int)type] = value;
        }
        public uint GetValue(LicenseType type)
        {
            return Values[(int)type];
        }
    }
}
