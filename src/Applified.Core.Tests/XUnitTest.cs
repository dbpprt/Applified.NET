using System;
using Xunit;

namespace Applified.Core.Tests
{
    /// <summary>
    /// This class is really stupid but this are my first experiments with xUnit in visual studio 
    /// </summary>
    public class XUnitTest
    {
        [Fact]
        public void Ensure_xUnitIsWorking()
        {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Ensure_ThisTestShouldFail()
        {
            Assert.Equal(1, 0);
        }
    }
}
