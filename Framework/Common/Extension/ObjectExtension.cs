
namespace Common.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 判断对象是否为null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断是否不为null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 将对象序列化为Json格式的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将一个对象转换为另一个对象
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(this object obj)
        {
            string json = obj.ToJson();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TDestination>(json);
        }
    }
}
