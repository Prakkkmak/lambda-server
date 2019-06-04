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
                if (permission.Equals(perm) || permission.StartsWith(perm + "_")) return true;

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
            List<string> arr = str.Split(",").ToList();
            foreach (string s in arr)
            {
                if (!string.IsNullOrWhiteSpace(s)) permissions.Add(s);
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (string permission in permissions)
            {
                str += permission + ",";
            }
            if (str.Length > 0) str = str.Remove(str.Length - 1);
            return str;
        }

        public static bool PermissionExist(string permission)
        {
            foreach (string s in CommandsPermissions)
            {
                if (s.ToUpper().StartsWith(permission.ToUpper() + "_") || s.ToUpper().Equals(permission.ToUpper())) return true;
            }
            return false;
        }
        public static List<string> CommandsPermissions = new List<string>();
    }
}
