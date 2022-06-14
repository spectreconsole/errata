using Shouldly;
using Xunit;

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
}
