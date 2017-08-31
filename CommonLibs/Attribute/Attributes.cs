using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibs
{
    public enum AuthTypeEnum
    {
        None = 0,
        Member,
        Admin,
        SuperAdmin,
    }
    public enum AuthIDTypeEnum
    {
        None = 0,
        AccountId,
        RoleId,
        TeamId,
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ApiAttribute : System.Attribute
    {
        public int ActionId;
        public Type ReturnType = null;
        public bool IsGet = false;
        public bool Encrypt = false;
        public bool IsValidToken = false;
        public bool RegPushData = false;
        public AuthTypeEnum AuthType = AuthTypeEnum.None;
        public AuthIDTypeEnum AuthIDType = AuthIDTypeEnum.None;
        public string Tips;

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WebApiAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WebSocketAttribute : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class GodDataAttribute : System.Attribute
    {
        public string Tips = string.Empty;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class XslToDbAttribute : System.Attribute
    {
    }
}
