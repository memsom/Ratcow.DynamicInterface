using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ratcow.DynamicInterface
{
    public static class MethodInfoExtension
    {
        static IEnumerable<MethodInfo> GetPublicMethodsImpl(Type type)
        {
            if (!type.IsInterface)
                return type.GetMethods();

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetMethods());
        }

        public static IEnumerable<MethodInfo> GetPublicMethods(this Type type)
        {
            return GetPublicMethodsImpl(type);
        }
    }
}
