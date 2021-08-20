using System;
using Spectre.Console;

namespace Errata
{
    public abstract class CharacterSet
    {
        public virtual char Colon { get; set; } = ':';
        public virtual char TopLeftCornerHard { get; set; } = '┌';
        public virtual char BottomLeftCornerHard { get; set; } = '└';
        public virtual char LeftConnector { get; set; } = '├';
        public virtual char HorizontalLine { get; set; } = '─';
        public virtual char VerticalLine { get; set; } = '│';
        public virtual char Dot { get; set; } = '·';
        public virtual char Anchor { get; set; } = '┬';
        public virtual char AnchorHorizontalLine { get; set; } = '─';
        public virtual char AnchorVerticalLine { get; set; } = '│';
        public virtual char BottomLeftCornerRound { get; set; } = '╰';

        public static AnsiCharacterSet Ansi => AnsiCharacterSet.Shared;
        public static AsciiCharacterSet Ascii => AsciiCharacterSet.Shared;

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

        public static CharacterSet Create(IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            return console.Profile.Capabilities.Unicode
                ? AnsiCharacterSet.Shared
                : AsciiCharacterSet.Shared;
        }
    }

    public class AnsiCharacterSet : CharacterSet
    {
        internal static AnsiCharacterSet Shared { get; } = new AnsiCharacterSet();
    }

    public class AsciiCharacterSet : CharacterSet
    {
        internal static AsciiCharacterSet Shared { get; } = new AsciiCharacterSet();

        public override char Anchor { get; set; } = '^';
        public override char AnchorHorizontalLine { get; set; } = '^';
        public override char BottomLeftCornerRound { get; set; } = '└';
    }
}
