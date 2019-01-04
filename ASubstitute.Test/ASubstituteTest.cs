using System;
using System.Linq;
using Xunit;
using System.Reflection;
using Xunit.Abstractions;
using System.Threading;
using System.Collections.Generic;
using ASubstitute;

namespace ASubstitute.Test
{
    public class ASubstituteTest
    {
        [Fact]
        public void Test1()
        {
            var calculator = Substitute.For<ICalculator>();

            int dummyResult = calculator.Compute(1, "foo", null);

            calculator
                .Compute(Arg.Any<int>(), Arg.Is("foo"), Arg.Any<object>())
                .Returns(13)
                .Returns(17)
                .Returns(18);

            int r = calculator.Compute(3, "foo", null);
            Assert.Equal(13, r);

            int b1 = calculator.Compute(3, "bar", null);
            Assert.Equal(default(int), b1);

            int r2 = calculator.Compute(3, "foo", null);
            Assert.Equal(17, r2);

            int r3 = calculator.Compute(3, "foo", null);
            Assert.Equal(18, r3);

            int r4 = calculator.Compute(3, "foo", null);
            Assert.Equal(18, r3);

            int b2 = calculator.Compute(3, "bar", null);
            Assert.Equal(default(int), b2);
        }
    }

    public interface ICalculator {
        int Compute(int a, string b, object c);
    }
}
