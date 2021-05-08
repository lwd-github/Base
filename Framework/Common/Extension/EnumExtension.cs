using Framework.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Common.Extension
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<NameValue> GetEnumItems(this Type type)
        {
            if (!type.IsEnum)
            {
                throw new ValidationException("参数不属于枚举类型");
            }
            
            List<NameValue> enumInfos = new List<NameValue>();

            foreach (int val in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, val);
                string description = type.GetMember(name).Single().GetDescription();

                enumInfos.Add(new NameValue()
                {
                    Name = name,
                    Value = val,
                    Description = description
                });
            }

            return enumInfos;
        }

        /// <summary>
        /// 获取枚举值的Description特性描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            Type type = value.GetType().GetUnderlyingType();

            MemberInfo member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.GetDescription() : value.ToString();
        }

        /// <summary>
        /// 获取枚举值的信息
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static NameValue GetNameValue(this Enum @enum)
        {
            if (@enum == null)
            {
                return null;
            }

            var type = @enum.GetType();

            if (!type.IsEnum)
            {
                throw new System.Exception("参数不属于枚举类型");
            }

            return new NameValue()
            {
                Name = Enum.GetName(type, @enum),
                Value = @enum.ToInt(),
                Description = @enum.GetDescription()
            };
        }

        /// <summary>
        /// 枚举转换整型，转换失败返回默认值
        /// </summary>
        /// <param name="obj">枚举对象</param>
        /// <returns></returns>
        public static int ToInt(this Enum @enum)
        {
            return Convert.ToInt32(@enum);
        }
    }
}
