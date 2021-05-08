using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Framework.Common.Extension
{
    public static class TypeExtension
    {
        /// <summary>
        /// 判断类型是否为可空类型，如int?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;

        /// <summary>
        /// 获取可空类型的基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.FirstOrDefault() as T;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        /// <summary>
        /// 获取类型的Description特性描述信息
        /// </summary>
        /// <param name="type">类型对象</param>
        /// <param name="inherit">是否搜索类型的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回类型的全名</returns>
        public static string GetDescription(this Type type, bool inherit = false)
        {
            var attribute = type.GetAttribute<DescriptionAttribute>(inherit);
            return attribute == null ? type.FullName : attribute.Description;
        }

        /// <summary>
        /// 获取成员元数据的Description特性描述信息
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string GetDescription(this MemberInfo member, bool inherit = false)
        {
            var attribute = member.GetAttribute<DescriptionAttribute>(inherit);
            if (attribute != null)
            {
                return attribute.Description;
            }

            var displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (displayName != null)
            {
                return displayName.DisplayName;
            }

            //var display = member.GetAttribute<DisplayAttribute>(inherit);
            //if (display != null)
            //{
            //    return display.Name;
            //}
            return member.Name;
        }
    }
}
