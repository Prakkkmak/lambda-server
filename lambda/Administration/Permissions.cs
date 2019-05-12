using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Lambda.Administration
{
    public class Permissions
    {
        private List<string> permissions = new List<string>();



        public void Add(string permission)
        {
            permission = permission.ToUpper();
            Remove(permission);
            permissions.Add(permission);
        }
        public List<string> Get()
        {
            return permissions;
        }
        public void Set(List<string> perms)
        {
            if (perms.Count == 1 && string.IsNullOrWhiteSpace(perms[0])) return;
            permissions = perms;
        }
        public bool Contains(string permission)
        {
            foreach (string perm in permissions)
            {
                if (perm.Equals(permission) || perm.StartsWith(permission + "_")) return true;

            }

            return false;
        }

        public void Remove(string permission)
        {
            for (int index = 0; index < permissions.Count; index++)
            {
                string s = permissions[index];
                if (!s.StartsWith(permission)) continue;
                permissions.Remove(s);
                index--;
            }
        }

        public void Set(string str)
        {
            permissions = str.Split(",").ToList();
        }

        public override string ToString()
        {
            string str = "";
            foreach (string permission in permissions)
            {
                str += permission + ",";
            }
            return str;
        }

        public static bool PermissionExist(string permission)
        {
            foreach (string s in CommandsPermissions)
            {
                if (s.StartsWith(s + "_") || s.Equals(permission)) return true;
            }
            return false;
        }
        public static List<string> CommandsPermissions = new List<string>();
    }
}
