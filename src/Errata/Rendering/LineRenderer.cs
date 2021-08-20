using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Errata
{
    internal static class LineRenderer
    {
        public static void DrawAnchors(
            ReportRendererContext builder,
            IReadOnlyList<LineLabel> lineLabels,
            int lineNumberMaxWidth)
        {
            if (builder is null)
            {
                throw new System.ArgumentNullException(nameof(builder));
            }

            if (lineLabels is null)
            {
                throw new System.ArgumentNullException(nameof(lineLabels));
            }

            // 🔎 ···(dot)
            builder.AppendSpaces(lineNumberMaxWidth + 2);
            builder.Append(Character.Dot, Color.Grey);
            builder.Append(" ");

            // 🔎 ···········
            var startMargin = lineLabels[0].Start;
            builder.AppendSpaces(startMargin);

            var lineLabelIndex = 0;
            var index = startMargin;
            foreach (var lineLabel in lineLabels)
            {
                if (index < lineLabel.Start)
                {
                    var diff = lineLabel.Start - index;
                    builder.AppendSpaces(diff);
                }

                // 🔎 ──┬──
                for (var i = lineLabel.Start; i < lineLabel.End; i++)
                {
                    if (i == lineLabel.Anchor)
                    {
                        builder.Append(Character.Anchor, lineLabel.Label.Color);
                    }
                    else
                    {
                        // ─
                        builder.Append(Character.AnchorHorizontalLine, lineLabel.Label.Color);
                    }
                }

                // Not last label?
                if (lineLabelIndex != lineLabels.Count - 1)
                {
                    // 🔎 ·
                    builder.AppendSpace();
                    index = lineLabel.End + 1;
                }

                lineLabelIndex++;
            }

            builder.CommitLine();
        }

        public static void DrawLines(
            ReportRendererContext builder,
            IReadOnlyList<LineLabel> lineLabels,
            int lineNumberMaxWidth)
        {
            if (builder is null)
            {
                throw new System.ArgumentNullException(nameof(builder));
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
                builder.AppendSpaces(lineNumberMaxWidth + 2);
                builder.Append(Character.Dot, Color.Grey);
                builder.Append(" ");
                builder.AppendSpaces(startMargin);

                var lineLabelIndex = 0;
                var index = startMargin;
                foreach (var label in lineLabels)
                {
                    if (index < label.Start)
                    {
                        // 🔎 ···········
                        var diff = label.Start - index;
                        builder.AppendSpaces(diff);
                    }

                    if (lineLabelIndex == currentLineLabel)
                    {
                        // 🔎 ···········╰
                        for (var i = label.Start; i <= label.Anchor; i++)
                        {
                            if (i == label.Anchor)
                            {
                                builder.Append(Character.BottomLeftCornerRound, label.Label.Color);
                            }
                            else
                            {
                                builder.AppendSpace();
                            }
                        }

                        // 🔎 ────── A label message
                        var diff = endMargin - label.Anchor;
                        builder.AppendRepeated(Character.HorizontalLine, diff, label.Label.Color);
                        builder.Append(" " + label.Label.Message, label.Label.Color);
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
                                    builder.AppendSpace();
                                }
                                else
                                {
                                    builder.Append(Character.AnchorVerticalLine, label.Label.Color);
                                }
                            }
                            else
                            {
                                builder.AppendSpace();
                            }
                        }
                    }

                    // Not last label?
                    if (lineLabelIndex != lineLabels.Count - 1)
                    {
                        // 🔎 ·
                        builder.AppendSpace();
                        index = label.End + 1;
                    }

                    lineLabelIndex++;
                }

                builder.CommitLine();
                currentLineLabel--;
            }
        }
    }
}
