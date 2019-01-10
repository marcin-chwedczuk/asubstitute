using System;
using System.Linq;
using Xunit;
using System.Reflection;
using Xunit.Abstractions;
using System.Threading;
using System.Collections.Generic;
using ASubstitute;
using ASubstitute.Test.DummyTypes;
using FluentAssertions;

namespace ASubstitute.Test {
    public class ASubstituteTest
    {
        public ASubstituteTest()
        {
            // Prevent failed tests from interfering with others.
            ASubstitute.Internal.ThreadLocalContext.Clear();
        }

        [Fact]
        public void Returns_default_values_for_actions_that_are_not_setup() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Assert
            Action returnsNothing = substitute.ReturnsNothing;
            returnsNothing
                .Should().NotThrow();

            substitute.ReturnsInt()
                .Should().Be(default(int));

            substitute.ReturnsValueType()
                .Should().Be(default(Point));
            
            substitute.ReturnsReferenceType()
                .Should().BeNull();
        }

        [Fact]
        public void Method_that_is_not_setup_can_be_called_many_times() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Assert
            substitute.ReturnsInt()
                .Should().Be(default(int));

            substitute.ReturnsInt()
                .Should().Be(default(int));

            substitute.ReturnsInt()
                .Should().Be(default(int));
        }

        [Fact]
        public void Method_that_was_setup_returns_value_specified_during_setup() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute
                .AddTwoIntegers(Arg.Is(3), Arg.Is(5))
                .Returns(8);

            // Assert
            substitute.AddTwoIntegers(3, 5)
                .Should().Be(8);

            substitute.AddTwoIntegers(3, 5)
                .Should().Be(8);

            substitute.AddTwoIntegers(5, 3)
                .Should().Be(default(int), because: "this method call was not setup");                
        }

        [Fact]
        public void Mocked_object_can_have_many_setups_for_different_methods() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute.ReturnsInt()
                .Returns(111);

            substitute.AddTwoIntegers(Arg.Is(22), Arg.Is(33))
                .Returns(55);

            // Assert
            substitute.ReturnsInt()
                .Should().Be(111);

            substitute.AddTwoIntegers(22, 33)
                .Returns(55);
        }

        [Fact]
        public void Bare_values_may_be_used_instead_of_matchers_for_non_default_values_of_arguments() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute.AddTwoIntegers(22, 33)
                .Returns(55);

            // Assert
            substitute.AddTwoIntegers(22, 33)
                .Returns(55);
        }

        [Fact]
        public void Missing_argument_matchers_are_detected_in_setup_calls() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            var exception = Record.Exception(() =>
            {
                substitute.ConcatenateStrings(
                        Arg.Is("foo"),
                        null, // missing matcher
                        Arg.Is("foo"))
                    .Returns("bar");
            });

            // Assert
            exception.Message.Should().Contain(
                $"{nameof(ITestInterface)}.{nameof(ITestInterface.ConcatenateStrings)}");

            exception.Message.Should().Contain("on position 2");

            exception.Message.Should().Contain("Missing argument matcher");
        }

        [Fact]
        public void Setup_methods_does_not_change_behaviour_of_non_setup_methods() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            // Setup using matchers
            substitute.AddTwoIntegers(Arg.Is(7), Arg.Is(15))
                .Returns(101);

            // Setup using non-default values
            substitute.AddTwoIntegers(8, 10)
                .Returns(102);

            // Mixed setup with 'any' matcher
            substitute.AddTwoIntegers(9, Arg.Any<int>())
                .Returns(103);

            // Assert
            substitute.AddTwoIntegers(10, 5)
                .Should().Be(default(int));

            substitute.AddTwoIntegers(8, 17)
                .Should().Be(default(int));
        }

        [Fact]
        public void Overloads_of_overloaded_methods_can_be_setup_independently() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute.OverloadedMethod(Arg.Any<int>())
                .Returns(1);

            substitute.OverloadedMethod(Arg.Any<int>(), Arg.Any<int>())
                .Returns(2);

            substitute.OverloadedMethod(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(3);

            substitute.OverloadedMethod(Arg.Any<string>())
                .Returns(4);

            // Assert
            substitute.OverloadedMethod(1)
                .Should().Be(1);

            substitute.OverloadedMethod(1, 2)
                .Should().Be(2);

            substitute.OverloadedMethod(1, 2, 3)
                .Should().Be(3);

            substitute.OverloadedMethod("foo")
                .Should().Be(4);
        }

        [Fact]
        public void Non_setup_properties_return_default_values() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Assert
            substitute.PropertyA
                .Should().Be(default(int));

            substitute.PropertyB
                .Should().Be(default(string));
        }

        [Fact]
        public void Property_that_is_setup_returns_values_set_during_setup() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute.PropertyA.Returns(101);
            substitute.PropertyB.Returns("foo");

            // Assert 
            substitute.PropertyA
                .Should().Be(101);

            substitute.PropertyB
                .Should().Be("foo");
        }

        [Fact]
        public void Many_behaviours_can_be_assigned_to_a_single_method() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Act
            substitute.ReturnsInt()
                .Returns(1)
                .Returns(2)
                .Returns(3);

            // Assert
            substitute.ReturnsInt()
                .Should().Be(1);

            substitute.ReturnsInt()
                .Should().Be(2);

            substitute.ReturnsInt()
                .Should().Be(3);

            // It should stay 3 forever
            substitute.ReturnsInt()
                .Should().Be(3);
        }

        // TODO: foo(1,2,3)return(7) + foo(arg.any, arg.any, 3)return(5)

        [Fact]
        public void Test1()
        {
            var calculator = Substitute.For<ITestInterface>();

            calculator
                .MixOfTypes(Arg.Any<int>(), Arg.Is("foo"), Arg.Any<object>())
                .Returns(13)
                .Returns(17)
                .Returns(18);

            // Should'ly? FluentAssertions?
            int r = calculator.MixOfTypes(3, "foo", null);
            Assert.Equal(13, r);

            int b1 = calculator.MixOfTypes(3, "bar", null);
            Assert.Equal(default(int), b1);

            int r2 = calculator.MixOfTypes(3, "foo", null);
            Assert.Equal(17, r2);

            int r3 = calculator.MixOfTypes(3, "foo", null);
            Assert.Equal(18, r3);

            int r4 = calculator.MixOfTypes(3, "foo", null);
            Assert.Equal(18, r3);

            int b2 = calculator.MixOfTypes(3, "bar", null);
            Assert.Equal(default(int), b2);
        }
    }
}
