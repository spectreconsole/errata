using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Errata
{
    internal static class LineRenderer
    {
        public static void DrawAnchors(
            ReportRendererContext ctx,
            IReadOnlyList<LineLabel> lineLabels,
            int lineNumberMaxWidth)
        {
            if (ctx is null)
            {
                throw new System.ArgumentNullException(nameof(ctx));
            }

            if (lineLabels is null)
            {
                throw new System.ArgumentNullException(nameof(lineLabels));
            }

            // 🔎 ···(dot)
            ctx.AppendSpaces(lineNumberMaxWidth + 2);
            ctx.Append(Character.Dot, Color.Grey);
            ctx.Append(" ");

            // 🔎 ···········
            var startMargin = lineLabels[0].Start;
            ctx.AppendSpaces(startMargin);

            var lineLabelIndex = 0;
            var index = startMargin;
            foreach (var lineLabel in lineLabels)
            {
                if (index < lineLabel.Start)
                {
                    var diff = lineLabel.Start - index;
                    ctx.AppendSpaces(diff);
                }

                // 🔎 ──┬──
                for (var i = lineLabel.Start; i < lineLabel.End; i++)
                {
                    if (i == lineLabel.Anchor)
                    {
                        ctx.Append(Character.Anchor, lineLabel.Label.Color);
                    }
                    else
                    {
                        // ─
                        ctx.Append(Character.AnchorHorizontalLine, lineLabel.Label.Color);
                    }
                }

                // Not last label?
                if (lineLabelIndex != lineLabels.Count - 1)
                {
                    // 🔎 ·
                    ctx.AppendSpace();
                    index = lineLabel.End + 1;
                }

                lineLabelIndex++;
            }

            ctx.CommitLine();
        }

        public static void DrawLines(
            ReportRendererContext ctx,
            IReadOnlyList<LineLabel> lineLabels,
            int lineNumberMaxWidth)
        {
            if (ctx is null)
            {
                throw new System.ArgumentNullException(nameof(ctx));
            }

            if (lineLabels is null)
            {
                throw new System.ArgumentNullException(nameof(lineLabels));
            }

            var startMargin = lineLabels[0].Start;
            var endMargin = lineLabels.Count == 1
                ? lineLabels[0].Anchor + 2
                : lineLabels.Last().End + 2;

            var currentLineLabel = lineLabels.Count - 1;
            for (var rowIndex = 0; rowIndex < lineLabels.Count; rowIndex++)
            {
                // 🔎 ···(dot)
                ctx.AppendSpaces(lineNumberMaxWidth + 2);
                ctx.Append(Character.Dot, Color.Grey);
                ctx.Append(" ");
                ctx.AppendSpaces(startMargin);

                var lineLabelIndex = 0;
                var index = startMargin;
                foreach (var label in lineLabels)
                {
                    if (index < label.Start)
                    {
                        // 🔎 ···········
                        var diff = label.Start - index;
                        ctx.AppendSpaces(diff);
                    }

                    if (lineLabelIndex == currentLineLabel)
                    {
                        // 🔎 ···········╰
                        for (var i = label.Start; i <= label.Anchor; i++)
                        {
                            if (i == label.Anchor)
                            {
                                ctx.Append(Character.BottomLeftCornerRound, label.Label.Color);
                            }
                            else
                            {
                                ctx.AppendSpace();
                            }
                        }

                        // 🔎 ────── A label message
                        var diff = endMargin - label.Anchor;
                        ctx.AppendRepeated(Character.HorizontalLine, diff, label.Label.Color);
                        ctx.Append(" " + label.Label.Message, label.Label.Color);
                    }
                    else
                    {
                        // 🔎 ··|··
                        for (var i = label.Start; i < label.End; i++)
                        {
                            if (i == label.Anchor)
                            {
                                if (lineLabelIndex > currentLineLabel)
                                {
                                    ctx.AppendSpace();
                                }
                                else
                                {
                                    ctx.Append(Character.AnchorVerticalLine, label.Label.Color);
                                }
                            }
                            else
                            {
                                ctx.AppendSpace();
                            }
                        }
                    }

                    // Not last label?
                    if (lineLabelIndex != lineLabels.Count - 1)
                    {
                        // 🔎 ·
                        ctx.AppendSpace();
                        index = label.End + 1;
                    }

                    lineLabelIndex++;
                }

                ctx.CommitLine();
                currentLineLabel--;
            }
        }
    }
}
