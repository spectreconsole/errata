using System;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Errata
{
    internal sealed class RenderConsole : IAnsiConsole
    {
        private readonly IAnsiConsole _console;

        public Profile Profile { get; }
        public IAnsiConsoleCursor Cursor => _console.Cursor;
        public IAnsiConsoleInput Input => _console.Input;
        public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;
        public RenderPipeline Pipeline => _console.Pipeline;

        public RenderConsole(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));

            Profile = new Profile(_console.Profile.Out, _console.Profile.Encoding)
            {
                Capabilities = _console.Profile.Capabilities,
                Width = int.MaxValue,
                Height = _console.Profile.Height,
            };
        }

        public void Clear(bool home)
        {
            _console.Clear();
        }

        public void Write(IRenderable renderable)
        {
            _console.Write(renderable);
        }
    }
}
