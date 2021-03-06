﻿using System;
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

        [Fact]
        public void Method_can_be_setup_to_throw_exceptions() {
            // Arrange
            var EXCEPTION_MESSAGE = "test-message";
            var substitute = Substitute.For<ITestInterface>();

            substitute.ReturnsInt()
                .Throws(new InvalidOperationException(EXCEPTION_MESSAGE));

            // Act
            var ex = Record.Exception(() => {
                substitute.ReturnsInt();
            });

            // Assert
            ex.Should().NotBeNull();
            ex.Should().BeAssignableTo<InvalidOperationException>();

            ex.Message.Should().Be(EXCEPTION_MESSAGE);
        }

        [Fact]
        public void Method_behaviours_can_be_mixed() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.ReturnsInt()
                .Returns(3)
                .Throws(new InvalidOperationException())
                .Returns(5);

            // Assert
            substitute.ReturnsInt()
                .Should().Be(3);

            ((Func<int>)substitute.ReturnsInt)
                .Should().Throw<InvalidOperationException>();
            
            substitute.ReturnsInt()
                .Should().Be(5);
        }

        [Fact]
        public void Any_matcher_does_not_collide_with_Is_matcher() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.AddTwoIntegers(Arg.Is(1), Arg.Is(2))
                .Returns(1);

            substitute.AddTwoIntegers(Arg.Any<int>(), Arg.Is(3))
                .Returns(2);

            // Assert
            substitute.AddTwoIntegers(1, 2)
                .Returns(1);

            substitute.AddTwoIntegers(1, 3)
                .Returns(2);

            // non setup call
            substitute.AddTwoIntegers(100, 100)
                .Returns(default(int));

            // Check nothing changed after first series of calls
            substitute.AddTwoIntegers(1, 2)
                .Returns(1);

             substitute.AddTwoIntegers(1, 3)
                .Returns(2);
        }

        [Fact]
        public void Receive_can_be_used_to_check_if_a_method_was_called_specified_number_of_times() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.ReturnsNothing();

            // Assert
            substitute.Received(1)
                .ReturnsNothing();

            var ex = Record.Exception(() => {
                substitute.Received(2)
                    .ReturnsNothing();
            });

            ex.Should().NotBeNull();
        }

        [Fact]
        public void Setup_calls_are_not_considered_real_method_calls_in_Receive() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // setup methods calls
            substitute.ReturnsInt()
                .Returns(101);

            substitute.AddTwoIntegers(1, 2)
                .Returns(3);

            substitute.AddTwoIntegers(2, 3)
                .Returns(5);

            // real methods calls
            substitute.ReturnsInt();
            substitute.ReturnsInt();
            substitute.ReturnsInt();

            substitute.AddTwoIntegers(2, 3);

            // Assert
            substitute.Received(3).ReturnsInt();

            substitute.Received(1).AddTwoIntegers(2, 3);
        }

        [Fact]
        public void DidNotReceive_can_be_used_to_check_that_a_method_was_not_called() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.ReturnsReferenceType()
                .Returns(new List<string> { "foo" })
                .Returns(new List<string> { "bar" });

            // call a method without earlier setup
            substitute.ReturnsNothing();

            // Assert
            substitute.DidNotReceive()
                .ReturnsInt();

            substitute.DidNotReceive()
                .ReturnsReferenceType();

            var ex = Record.Exception(() => {
                substitute.DidNotReceive()
                    .ReturnsNothing();
            });

            ex.Should().NotBeNull();
        }

        [Fact]
        public void Argument_matchers_can_be_used_with_Receive_to_check_received_calls() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.AddTwoIntegers(1, 2);
            substitute.AddTwoIntegers(1, 3);
            substitute.AddTwoIntegers(1, 4);

            substitute.AddTwoIntegers(10, 12345);

            // Assert
            substitute.Received(4)
                .AddTwoIntegers(Arg.Any<int>(), Arg.Any<int>());

            substitute.Received(3)
                .AddTwoIntegers(1, Arg.Any<int>());

            substitute.Received(1)
                .AddTwoIntegers(10, Arg.Any<int>());
        }

        [Fact]
        public void A_predicate_can_be_used_with_ArgIs_matcher() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.AddTwoIntegers(
                    Arg.Is<int>(n => n % 2 == 0), 
                    Arg.Is<int>(n => n > 10))
                .Returns(1)
                .Returns(2)
                .Returns(3);

            // Assert
            substitute.AddTwoIntegers(8, 11)
                .Should().Be(1);

            substitute.AddTwoIntegers(10, 101010)
                .Should().Be(2);

            substitute.AddTwoIntegers(12, 32)
                .Should().Be(3);

            substitute.AddTwoIntegers(11, 32)
                .Should().Be(default(int));

            substitute.AddTwoIntegers(8, 1)
                .Should().Be(default(int));
        }

        [Fact]
        public void Setup_works_with_methods_that_have_parameters_default_values() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.MethodWithDefaultParameter()
                .Returns(1);

            substitute.MethodWithDefaultParameter(100)
                .Returns(2);

            // Assert
            substitute.MethodWithDefaultParameter()
                .Should().Be(1);

            substitute.MethodWithDefaultParameter(100)
                .Should().Be(2);
        }

        [Fact]
        public void Mock_can_be_named() {
            // Arrange
            var unnamed = Substitute.For<ITestInterface>();
            var foo = Substitute.For<ITestInterface>("foo");

            // Assert
            void failingAssertion(ITestInterface mock) {
                mock.Received(1)
                    .AddTwoIntegers(Arg.Any<int>(), Arg.Any<int>());
            };

            var unnamedException = Record.Exception(() => failingAssertion(unnamed));
            unnamedException.Message
                .Should().Contain($"{nameof(ITestInterface)}.{nameof(ITestInterface.AddTwoIntegers)}(any<int>, any<int>)");

            var fooException = Record.Exception(() => failingAssertion(foo));
            fooException.Message
                .Should().Contain($"foo.{nameof(ITestInterface.AddTwoIntegers)}(any<int>, any<int>)");
        }

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
