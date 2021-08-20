using System;
using Errata;
using Spectre.Console;

namespace Example
{
    public static class Program
    {
        public static void Main()
        {
            // Create a new report
            var report = new Report();
            var repository = new EmbeddedResourceRepository(typeof(Program).Assembly);

            // C#
            report.AddDiagnostic(
                Diagnostic.Error("Operator '/' cannot be applied to operands of type 'string' and 'int'")
                    .WithCode("CS0019")
                    .WithNote("Try changing the type")
                    .WithLabel(new Label("Example/Files/Program.cs", new Location(15, 23), "This is of type 'int'")
                        .WithLength(3)
                        .WithColor(Color.Yellow))
                    .WithLabel(new Label("Example/Files/Program.cs", new Location(15, 27), "Division is not possible")
                        .WithColor(Color.Red))
                    .WithLabel(new Label("Example/Files/Program.cs", new Location(15, 29), "This is of type 'string'")
                        .WithLength(3)
                        .WithColor(Color.Blue)));

            report.AddDiagnostic(
                Diagnostic.Warning("Fix formatting")
                    .WithCode("IDE0055"))
                    .WithLabel(new Label("Example/Files/Program.cs", 174..176, "Code should not contain trailing whitespace").WithColor(Color.Yellow));

            // Markdown
            report.AddDiagnostic(
                Diagnostic.Error("There were markdown errors")
                    .WithCode("MARKDOWN001")
                    .WithLabel(new Label("Example/Files/Example.md", 24..27, "Did you mean 'just'?")
                        .WithColor(Color.Yellow))
                    .WithLabel(new Label("Example/Files/Example.md", 31..41, "Invalid markdown")
                        .WithColor(Color.Red))
                    .WithLabel(new Label("Example/Files/Example.md", 251..270, "Did you mean 'Yabba dabba doo'?")
                        .WithColor(Color.Yellow)));

            // C++
            report.AddDiagnostic(
                Diagnostic.Error("Compiler error")
                    .WithCode("C2084")
                    .WithNote("Overloaded member not found")
                    .WithLabel(new Label("Example/Files/Foo.cpp", 22..37, " 'void Foo::bar(float)': overloaded member function not found in 'Foo'")
                        .WithColor(Color.Red)
                        .WithNote("See declaration of 'Foo' in Foo.h"))
                    .WithLabel(new Label("Example/Files/Foo.h", 24..38, "See declaration of 'Foo'")
                        .WithColor(Color.Blue)));

            // Render the report
            report.Render(AnsiConsole.Console, repository);
        }
    }
}
