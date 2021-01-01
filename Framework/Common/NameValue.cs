using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class NameValue<T>
    {
        public string Name { get; set; }

        public T Value { get; set; }

        public string Description { get; set; }
    }

    /// <summary>
    /// NameValue类型：值为Int
    /// </summary>
    public class NameValue : NameValue<int>
    { 
    
    }
}
