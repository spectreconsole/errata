using Shouldly;
using Xunit;

namespace Errata.Tests
{
    public sealed class SourceTests
    {
        [Fact]
        public void Should_Return_Correct_Length()
        {
            // Given
            var source = new Source("Program.cs", "public void Main()\n{\n\tConsole.WriteLine(\"Hello\")\n}");

            // When
            var result = source.Length;

            // Then
            result.ShouldBe(50);
        }

        [Fact]
        public void Should_Return_Correct_Text()
        {
            // Given
            var source = new Source("Program.cs", "public void Main()\n{\n\tConsole.WriteLine(\"Hello\")\n}");

            // When
            var (line, _, _) = source.GetLineOffset(22);

            // Then
            line.Text.ShouldBe("\tConsole.WriteLine(\"Hello\")");
        }

        [Fact]
        public void Should_Return_Correct_Row()
        {
            // Given
            var source = new Source("Program.cs", "public void Main()\n{\n\tConsole.WriteLine(\"Hello\")\n}");

            // When
            var (_, row, _) = source.GetLineOffset(22);

            // Then
            row.ShouldBe(2);
        }

        [Fact]
        public void Should_Return_Correct_Column()
        {
            // Given
            var source = new Source("Program.cs", "public void Main()\n{\n\tConsole.WriteLine(\"Hello\")\n}");

            // When
            var (_, _, column) = source.GetLineOffset(22);

            // Then
            column.ShouldBe(1);
        }
    }
}
