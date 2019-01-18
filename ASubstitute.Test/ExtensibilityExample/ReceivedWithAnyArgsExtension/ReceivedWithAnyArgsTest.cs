using System;
using ASubstitute.Internal;
using ASubstitute.Test.DummyTypes;
using FluentAssertions;
using Xunit;

namespace ASubstitute.Test.ExtensibilityExample.ReceivedWithAnyArgs {
    // TODO: Move to a separate assembly - this assembly has internalsVisible: true

    public class ReceivedWithAnyArgsTest {
        [Fact]
        public void Assertion_passes_when_method_was_called() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.AddTwoIntegers(1, 2);

            // Assert
            Action assertion = () => {
                substitute.ReceivedWithAnyArgs()
                    .AddTwoIntegers(0, 0);
            };

            assertion.Should().NotThrow();
        }

        [Fact]
        public void Assertion_throws_when_method_was_not_called() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            // Some other calls
            substitute.ReturnsInt();
            substitute.ReturnsNothing();

            // Assert
            Action assertion = () => {
                substitute.ReceivedWithAnyArgs()
                    .AddTwoIntegers(0, 0);
            };

            assertion.Should().Throw<SubstituteException>();
        }

        // TODO: Test na received AddTwoIntegers(0,0) and matcherplaceholder.
    }
}