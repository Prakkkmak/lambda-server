using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.Commands
{
    public class PermissionAttribute : Attribute
    {
        public string Permission;
        public PermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
