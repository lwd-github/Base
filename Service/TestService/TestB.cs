using System;
using System.Collections.Generic;
using System.Text;

namespace TestService
{
    public class TestB : ITest
    {
        public int Type { get; set; } = 2;

        public string GetValue()
        {
            return "TestB";
        }
    }
}
