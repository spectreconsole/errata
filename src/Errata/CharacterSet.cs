using System;
using Spectre.Console;

namespace Errata
{
    /// <summary>
    /// Represents a character set.
    /// </summary>
    public abstract class CharacterSet
    {
        /// <summary>
        /// Gets a renderable representation of `:`.
        /// </summary>
        public virtual char Colon { get; } = ':';

        /// <summary>
        /// Gets a renderable representation of `┌`.
        /// </summary>
        public virtual char TopLeftCornerHard { get; } = '┌';

        /// <summary>
        /// Gets a renderable representation of `└`.
        /// </summary>
        public virtual char BottomLeftCornerHard { get; } = '└';

        /// <summary>
        /// Gets a renderable representation of `├`.
        /// </summary>
        public virtual char LeftConnector { get; } = '├';

        /// <summary>
        /// Gets a renderable representation of `─`.
        /// </summary>
        public virtual char HorizontalLine { get; } = '─';

        /// <summary>
        /// Gets a renderable representation of `│`.
        /// </summary>
        public virtual char VerticalLine { get; } = '│';

        /// <summary>
        /// Gets a renderable representation of `·`.
        /// </summary>
        public virtual char Dot { get; } = '·';

        /// <summary>
        /// Gets a renderable representation of `┬`.
        /// </summary>
        public virtual char Anchor { get; } = '┬';

        /// <summary>
        /// Gets a renderable representation of `─`.
        /// </summary>
        public virtual char AnchorHorizontalLine { get; } = '─';

        /// <summary>
        /// Gets a renderable representation of `│`.
        /// </summary>
        public virtual char AnchorVerticalLine { get; } = '│';

        /// <summary>
        /// Gets a renderable representation of `╰`.
        /// </summary>
        public virtual char BottomLeftCornerRound { get; } = '╰';

        /// <summary>
        /// Gets a Unicode compatible character set.
        /// </summary>
        public static UnicodeCharacterSet Unicode => UnicodeCharacterSet.Shared;

        /// <summary>
        /// Gets an ASCII compatible character set.
        /// </summary>
        public static AsciiCharacterSet Ascii => AsciiCharacterSet.Shared;

        /// <summary>
        /// Gets a renderable representation of a <see cref="Character"/>.
        /// </summary>
        /// <param name="character">The character to get a renderable representation of.</param>
        /// <returns>A renderable representation of a <see cref="Character"/>.</returns>
        public char Get(Character character)
        {
            return character switch
            {
                Character.Colon => Colon,
                Character.TopLeftCornerHard => TopLeftCornerHard,
                Character.BottomLeftCornerHard => BottomLeftCornerHard,
                Character.LeftConnector => LeftConnector,
                Character.HorizontalLine => HorizontalLine,
                Character.VerticalLine => VerticalLine,
                Character.Dot => Dot,
                Character.Anchor => Anchor,
                Character.AnchorHorizontalLine => AnchorHorizontalLine,
                Character.AnchorVerticalLine => AnchorVerticalLine,
                Character.BottomLeftCornerRound => BottomLeftCornerRound,
                _ => throw new NotSupportedException($"Unknown character '{character}'"),
            };
        }

        /// <summary>
        /// Creates a <see cref="CharacterSet"/> that is compatible with the specified <see cref="IAnsiConsole"/>.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <returns>A <see cref="CharacterSet"/> that is compatible with the specified <see cref="IAnsiConsole"/>.</returns>
        public static CharacterSet Create(IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            return console.Profile.Capabilities.Unicode
                ? UnicodeCharacterSet.Shared
                : AsciiCharacterSet.Shared;
        }
    }

    /// <summary>
    /// Represents a Unicode compatible character set.
    /// </summary>
    public class UnicodeCharacterSet : CharacterSet
    {
        internal static UnicodeCharacterSet Shared { get; } = new UnicodeCharacterSet();
    }

    /// <summary>
    /// Represents an ASCII compatible character set.
    /// </summary>
    public class AsciiCharacterSet : CharacterSet
    {
        internal static AsciiCharacterSet Shared { get; } = new AsciiCharacterSet();

        /// <inheritdoc/>
        public override char Anchor { get; } = '┬';

        /// <inheritdoc/>
        public override char AnchorHorizontalLine { get; } = '─';

        /// <inheritdoc/>
        public override char BottomLeftCornerRound { get; } = '└';
    }
}
