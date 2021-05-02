using System;
using System.Collections.Generic;
using System.Text;

namespace TestService
{
    public class TestA : ITest
    {
        public int Type { get; set; } = 1;

        public string GetValue()
        {
            return "TestA";
        }
    }
}
