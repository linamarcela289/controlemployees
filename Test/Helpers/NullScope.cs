using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Helpers
{
    public class NullScope : IDisposable
    {
        public static NullScope Intance { get; } = new NullScope();

        public void Dispose() { }

        private NullScope() { }
    }
}
