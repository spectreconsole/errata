using Shouldly;
using Xunit;

namespace Errata.Tests
{
    public sealed class TextSpanTests
    {
        [Fact]
        public void Should_Return_Correct_Start_Position()
        {
            // Given, When
            var span = new TextSpan(13, 19);

            // Then
            span.Start.ShouldBe(13);
        }

        [Fact]
        public void Should_Return_Correct_End_Position()
        {
            // Given, When
            var span = new TextSpan(13, 19);

            // Then
            span.End.ShouldBe(19);
        }

        [Fact]
        public void Should_Return_Correct_Length()
        {
            // Given, When
            var span = new TextSpan(13, 19);

            // Then
            span.Length.ShouldBe(6);
        }
    }
}
