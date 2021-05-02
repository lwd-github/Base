using System;

namespace TestService
{
    public interface ITest
    {
        int Type { get; set; }

        string GetValue();
    }
}
