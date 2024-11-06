﻿using System.Reflection;
using System.Runtime.CompilerServices;

namespace Reddit.Api.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetCustomAttributes(typeof(T), true).Length != 0;
        }

        public static bool IsRequired(this PropertyInfo propertyInfo)
        {
            return propertyInfo.HasCustomAttribute<RequiredMemberAttribute>();
        }
    }
}