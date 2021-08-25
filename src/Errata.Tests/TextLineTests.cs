using Shouldly;
using Xunit;

namespace Errata.Tests
{
    public sealed class TextLineTests
    {
        [Fact]
        public void Should_Return_Correct_Index()
        {
            // Given, When
            var span = new TextLine(12, "HELLO WORLD", 13);

            // Then
            span.Index.ShouldBe(12);
        }

        [Fact]
        public void Should_Return_Correct_Text()
        {
            // Given, When
            var span = new TextLine(0, "HELLO WORLD", 13);

            // Then
            span.Text.ShouldBe("HELLO WORLD");
        }

        [Fact]
        public void Should_Return_Correct_Offset()
        {
            // Given, When
            var span = new TextLine(0, "HELLO WORLD", 13);

            // Then
            span.Offset.ShouldBe(13);
        }

        [Fact]
        public void Should_Return_Correct_Length()
        {
            // Given, When
            var span = new TextLine(0, "HELLO WORLD", 13);

            // Then
            span.Length.ShouldBe(11);
        }

        [Fact]
        public void Should_Return_Correct_Range()
        {
            // Given, When
            var span = new TextLine(0, "HELLO WORLD", 13);

            // Then
            span.Span.Start.ShouldBe(13);
            span.Span.End.ShouldBe(24);
        }
    }
}
