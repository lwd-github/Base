using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTest.Enum;

namespace XUnitTest.Model
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public ESex Sex { get; set; }

        public Dictionary<string, int> Attribute { get; set; }

        public List<int> Test;
    }
}
