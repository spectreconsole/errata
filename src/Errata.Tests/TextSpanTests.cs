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

        public sealed class TheContainsMethod
        {
            [Theory]
            [InlineData(10)]
            [InlineData(15)]
            [InlineData(19)]
            public void Should_Return_True_If_Span_Contains_Offset(int offset)
            {
                // Given
                var span = new TextSpan(10, 20);

                // When
                var result = span.Contains(offset);

                // Then
                result.ShouldBeTrue();
            }

            [Theory]
            [InlineData(9)]
            [InlineData(20)]
            public void Should_Return_False_If_Span_Do_Not_Contain_Offset(int offset)
            {
                // Given
                var span = new TextSpan(10, 20);

                // When
                var result = span.Contains(offset);

                // Then
                result.ShouldBeFalse();
            }
        }
    }
}
