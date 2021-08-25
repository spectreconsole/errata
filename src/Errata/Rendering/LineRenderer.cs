using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Errata
{
    internal static class LineRenderer
    {
        public static void DrawAnchors(DiagnosticContext ctx, IReadOnlyList<LineLabel> labels)
        {
            if (ctx is null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }

            if (labels is null)
            {
                throw new ArgumentNullException(nameof(labels));
            }

            // 🔎 ···(dot)
            ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
            ctx.Builder.Append(Character.Dot, Color.Grey);
            ctx.Builder.Append(" ");

            // 🔎 ···········
            var startMargin = labels[0].Start;
            ctx.Builder.AppendSpaces(startMargin);

            var labelIndex = 0;
            var index = startMargin;
            foreach (var label in labels)
            {
                if (index < label.Start)
                {
                    var diff = label.Start - index;
                    ctx.Builder.AppendSpaces(diff);
                }

                // 🔎 ──┬──
                label.DrawAnchor(ctx.Builder);

                // Not last label?
                if (labelIndex != labels.Count - 1)
                {
                    // 🔎 ·
                    ctx.Builder.AppendSpace();
                    index = label.End + 1;
                }

                labelIndex++;
            }

            ctx.Builder.CommitLine();
        }

        public static void DrawLines(DiagnosticContext ctx, IReadOnlyList<LineLabel> labels)
        {
            if (ctx is null)
            {
                throw new ArgumentNullException(nameof(ctx));
            }

            if (labels is null)
            {
                throw new ArgumentNullException(nameof(labels));
            }

            var startMargin = labels[0].Start;
            var endMargin = labels.Count == 1
                ? labels[0].Anchor + 2
                : labels.Last().End + 2;

            var currentLineLabel = labels.Count - 1;
            for (var rowIndex = 0; rowIndex < labels.Count; rowIndex++)
            {
                // 🔎 ···(dot)
                ctx.Builder.AppendSpaces(ctx.LineNumberWidth + 2);
                ctx.Builder.Append(Character.Dot, Color.Grey);
                ctx.Builder.Append(" ");
                ctx.Builder.AppendSpaces(startMargin);

                var labelIndex = 0;
                var index = startMargin;
                foreach (var label in labels)
                {
                    if (index < label.Start)
                    {
                        // 🔎 ···········
                        var diff = label.Start - index;
                        ctx.Builder.AppendSpaces(diff);
                    }

                    if (labelIndex == currentLineLabel)
                    {
                        // 🔎 ···········╰
                        for (var i = label.Start; i <= label.Anchor; i++)
                        {
                            if (i == label.Anchor)
                            {
                                ctx.Builder.Append(Character.BottomLeftCornerRound, label.Label.Color);
                            }
                            else
                            {
                                ctx.Builder.AppendSpace();
                            }
                        }

                        // 🔎 ────── A label message
                        var diff = endMargin - label.Anchor;
                        ctx.Builder.AppendRepeated(Character.HorizontalLine, diff, label.Label.Color);
                        ctx.Builder.Append(" " + label.Label.Message, label.Label.Color);
                    }
                    else
                    {
                        // 🔎 ··|··
                        for (var i = label.Start; i < label.End; i++)
                        {
                            if (i == label.Anchor)
                            {
                                if (labelIndex > currentLineLabel)
                                {
                                    ctx.Builder.AppendSpace();
                                }
                                else
                                {
                                    ctx.Builder.Append(Character.AnchorVerticalLine, label.Label.Color);
                                }
                            }
                            else
                            {
                                ctx.Builder.AppendSpace();
                            }
                        }
                    }

                    // Not last label?
                    if (labelIndex != labels.Count - 1)
                    {
                        // 🔎 ·
                        ctx.Builder.AppendSpace();
                        index = label.End + 1;
                    }

                    labelIndex++;
                }

                ctx.Builder.CommitLine();
                currentLineLabel--;
            }
        }
    }
}
