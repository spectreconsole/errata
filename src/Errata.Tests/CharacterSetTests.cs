using Shouldly;
using Xunit;

namespace Errata.Tests
{
    public sealed class CharacterSetTests
    {
        public sealed class Ansi
        {
            [Theory]
            [InlineData(Character.Anchor, '┬')]
            [InlineData(Character.AnchorHorizontalLine, '─')]
            [InlineData(Character.AnchorVerticalLine, '│')]
            [InlineData(Character.BottomLeftCornerHard, '└')]
            [InlineData(Character.BottomLeftCornerRound, '╰')]
            [InlineData(Character.Colon, ':')]
            [InlineData(Character.Dot, '·')]
            [InlineData(Character.HorizontalLine, '─')]
            [InlineData(Character.LeftConnector, '├')]
            [InlineData(Character.TopLeftCornerHard, '┌')]
            [InlineData(Character.VerticalLine, '│')]
            public void Should_Return_Expected_Characters(Character character, char expected)
            {
                // Given, When
                var result = CharacterSet.Unicode.Get(character);

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class Ascii
        {
            [Theory]
            [InlineData(Character.Anchor, '^')]
            [InlineData(Character.AnchorHorizontalLine, '^')]
            [InlineData(Character.AnchorVerticalLine, '│')]
            [InlineData(Character.BottomLeftCornerHard, '└')]
            [InlineData(Character.BottomLeftCornerRound, '└')]
            [InlineData(Character.Colon, ':')]
            [InlineData(Character.Dot, '·')]
            [InlineData(Character.HorizontalLine, '─')]
            [InlineData(Character.LeftConnector, '├')]
            [InlineData(Character.TopLeftCornerHard, '┌')]
            [InlineData(Character.VerticalLine, '│')]
            public void Should_Return_Expected_Characters(Character character, char expected)
            {
                // Given, When
                var result = CharacterSet.Ascii.Get(character);

                // Then
                result.ShouldBe(expected);
            }
        }
    }
}
