namespace Errata.Tests;

public sealed class LabelTests
{
    public sealed class TheWithNoteMethod
    {
        [Fact]
        public void Should_Set_Note_To_Provided_Value()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");

            // When
            label.WithNote("The note");

            // Then
            label.Note.ShouldBe("The note");
        }

        [Fact]
        public void Should_Set_Note_To_Null_If_Null_Is_Provided()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");
            label.WithNote("The Note");

            // When
            label.WithNote(null);

            // Then
            label.Note.ShouldBeNull();
        }
    }

    public sealed class TheWithContextMethod
    {
        [Fact]
        public void Should_Set_ContextLines_To_Provided_Value()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");

            // When
            label.WithContextLines(3);

            // Then
            label.ContextLines.ShouldBe(3);
        }

        [Fact]
        public void Should_Return_Same_Instance_For_Chaining()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");

            // When
            var result = label.WithContextLines(2);

            // Then
            result.ShouldBeSameAs(label);
        }

        [Fact]
        public void Should_Throw_If_Negative_Value_Is_Provided()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");

            // When, Then
            Should.Throw<System.ArgumentOutOfRangeException>(() => label.WithContextLines(-1));
        }

        [Fact]
        public void Should_Allow_Zero_Context_Lines()
        {
            // Given
            var label = new Label("Program.cs", new Location(1, 2), "The message");

            // When
            label.WithContextLines(0);

            // Then
            label.ContextLines.ShouldBe(0);
        }
    }
}
