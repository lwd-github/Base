using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.Model
{
    public class Student : Person
    {
        public DateTime AdmissionDate { get; set; }

        public new string Id { get; set; } //类型不一致，测试转换

        public new string Birthday { get; set; } //类型不一致，测试转换
    }
}
