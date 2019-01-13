using ASubstitute.Test.DummyTypes;
using FluentAssertions;
using Xunit;

namespace ASubstitute.Test.ExtensibilityExample.DoExtension {
    public class DoMethodBehaviourTest {
        [Fact]
        public void Method_should_return_values_generated_by_callback() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            var next = 0;
            substitute.ReturnsInt()
                .Do(args => next++);
                
            // Assert
            substitute.ReturnsInt()
                .Should().Be(0);

            substitute.ReturnsInt()
                .Should().Be(1);

            substitute.ReturnsInt()
                .Should().Be(2);

            substitute.ReturnsInt()
                .Should().Be(3);
        }

        [Fact]
        public void Method_arguments_should_be_available_in_callback() {
            // Arrange
            var substitute = Substitute.For<ITestInterface>();

            substitute.AddTwoIntegers(Arg.Any<int>(), Arg.Any<int>())
                .Do(args => (int)args[0] + (int)args[1]);
 
            // Assert
            substitute.AddTwoIntegers(1, 5)
                .Should().Be(6);

            substitute.AddTwoIntegers(15, 0)
                .Should().Be(15);
        }
    }
}