using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Testing;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Errata.Tests
{
    [ExpectationPath("Report")]
    public sealed class ReportTests
    {
        [ExpectationPath("Rendering")]
        public sealed class TheRenderMethod
        {
            [UsesVerify]
            [ExpectationPath("Spans")]
            public sealed class UsingSpans
            {
                [Fact]
                [Expectation("SingleLabel")]
                public Task Should_Render_Diagnostic_With_Single_Label_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("There are spelling errors")
                            .WithCode("SPELLING001")
                            .WithLabel(new Label("Example.md", 251..270, "Did you mean 'Yabba dabba doo'?").WithColor(Color.Red)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("MultipleLabels")]
                public Task Should_Render_Diagnostic_With_Multiple_Labels_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Operator '/' cannot be applied to operands of type 'string' and 'int'")
                            .WithCode("CS0019")
                            .WithNote("Try changing the type")
                            .WithLabel(new Label("Program.cs", 303..306, "This is of type 'int'").WithColor(Color.Yellow))
                            .WithLabel(new Label("Program.cs", 307..308, "Division is not possible").WithColor(Color.Red))
                            .WithLabel(new Label("Program.cs", 309..312, "This is of type 'string'").WithColor(Color.Blue)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("LabelsWithMultipleFiles")]
                public Task Should_Render_Diagnostic_With_Multiple_Labels_In_Multiple_Files_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Compiler error")
                            .WithCode("C2084")
                            .WithNote("Overloaded member not found")
                            .WithLabel(new Label("Foo.cpp", 22..37, " 'void Foo::bar(float)': overloaded member function not found in 'Foo'")
                                .WithColor(Color.Red).WithNote("See declaration of 'Foo' in Foo.h"))
                            .WithLabel(new Label("Foo.h", 24..38, "See declaration of 'Foo'").WithColor(Color.Blue)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("MultipleDiagnostics")]
                public Task Should_Render_Multiple_Diagnostics_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Operator '/' cannot be applied to operands of type 'string' and 'int'")
                            .WithCode("CS0019")
                            .WithNote("Try changing the type")
                            .WithLabel(new Label("Program.cs", 303..306, "This is of type 'int'").WithColor(Color.Yellow))
                            .WithLabel(new Label("Program.cs", 307..308, "Division is not possible").WithColor(Color.Red))
                            .WithLabel(new Label("Program.cs", 309..312, "This is of type 'string'").WithColor(Color.Blue)));

                    report.AddDiagnostic(
                        Diagnostic.Warning("Fix formatting")
                            .WithCode("IDE0055"))
                            .WithLabel(new Label("Program.cs", 174..176, "Code should not contain trailing whitespace").WithColor(Color.Yellow));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }
            }

            [UsesVerify]
            [ExpectationPath("Locations")]
            public sealed class UsingLocations
            {
                [Fact]
                [Expectation("SingleLabel")]
                public Task Should_Render_Diagnostic_With_Single_Label_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("There are spelling errors")
                            .WithCode("SPELLING001")
                            .WithLabel(new Label("Example.md", new Location(11, 29), "Did you mean 'Yabba dabba doo'?")
                                .WithColor(Color.Red)
                                .WithLength(19)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("MultipleLabels")]
                public Task Should_Render_Diagnostic_With_Multiple_Labels_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Operator '/' cannot be applied to operands of type 'string' and 'int'")
                            .WithCode("CS0019")
                            .WithNote("Try changing the type")
                            .WithLabel(new Label("Program.cs", new Location(15, 23), "This is of type 'int'")
                                .WithColor(Color.Yellow).WithLength(3))
                            .WithLabel(new Label("Program.cs", new Location(15, 27), "Division is not possible")
                                .WithColor(Color.Red).WithLength(1))
                            .WithLabel(new Label("Program.cs", new Location(15, 29), "This is of type 'string'")
                                .WithColor(Color.Blue).WithLength(3)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("LabelsWithMultipleFiles")]
                public Task Should_Render_Diagnostic_With_Multiple_Labels_In_Multiple_Files_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Compiler error")
                            .WithCode("C2084")
                            .WithNote("Overloaded member not found")
                            .WithLabel(new Label("Foo.cpp", new Location(2, 6), "'void Foo::bar(float)': overloaded member function not found in 'Foo'")
                                .WithLength(15)
                                .WithColor(Color.Red)
                                .WithNote("See declaration of 'Foo' in Foo.h"))
                            .WithLabel(new Label("Foo.h", new Location(3, 5), "See declaration of 'Foo'")
                                .WithLength(14)
                                .WithColor(Color.Blue)));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }

                [Fact]
                [Expectation("MultipleDiagnostics")]
                public Task Should_Render_Multiple_Diagnostics_Correctly()
                {
                    // Given
                    var console = new TestConsole().Width(80);
                    var report = new Report(new EmbeddedResourceRepository());

                    report.AddDiagnostic(
                        Diagnostic.Error("Operator '/' cannot be applied to operands of type 'string' and 'int'")
                            .WithCode("CS0019")
                            .WithNote("Try changing the type")
                            .WithLabel(new Label("Program.cs", new Location(15, 23), "This is of type 'int'")
                                .WithColor(Color.Yellow).WithLength(3))
                            .WithLabel(new Label("Program.cs", new Location(15, 27), "Division is not possible")
                                .WithColor(Color.Red).WithLength(1))
                            .WithLabel(new Label("Program.cs", new Location(15, 29), "This is of type 'string'")
                                .WithColor(Color.Blue).WithLength(3)));

                    report.AddDiagnostic(
                        Diagnostic.Warning("Fix formatting")
                            .WithCode("IDE0055"))
                            .WithLabel(new Label("Program.cs", new Location(9, 32), "Code should not contain trailing whitespace")
                                .WithLength(2)
                                .WithColor(Color.Yellow));

                    // When
                    report.Render(console);

                    // Then
                    return Verifier.Verify(console.Output);
                }
            }
        }
    }
}
