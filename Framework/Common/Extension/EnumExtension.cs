using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extension
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<NameValue<int>> GetItems(this Type type)
        {
            if (!type.IsEnum)
            {
                throw new ValidationException("参数不属于枚举类型");
            }
            
            List<NameValue<int>> enumInfos = new List<NameValue<int>>();

            foreach (int val in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, val);
                string description = type.GetMember(name).Single().GetDescription();

                enumInfos.Add(new NameValue<int>()
                {
                    Name = name,
                    Value = val,
                    Description = description
                });
            }

            return enumInfos;
        }
    }
}
