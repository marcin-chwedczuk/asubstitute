using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System.Collections.ObjectModel;
using System.Collections.Immutable;

namespace NSubstitute.Test
{
    public class UnitTest1
    {
        // [Fact]
        public void test() {
            var s = Substitute.For<IServiceX>();

            s.Foo(null, Arg.Is("foo"), null).Returns("bar");

            Console.WriteLine(s.Foo(null, "foo", null));
        }


    }

    public interface IServiceX {
        string Foo(string a, string b, string c);
    }
}
