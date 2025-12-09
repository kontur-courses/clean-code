using System.Text;
using MarkupConverter.AST.Inlines;

namespace MarkupConverter.Parsers.InlineParsers;

public class InlineParser : IInlineParser
{
    public List<Inline> Parse(string text)
    {
        var output = new List<Inline>();
        var delimiters = new List<Delimiter>();
        var sb = new StringBuilder();

        var i = 0;
        var n = text.Length;

        void FlushText()
        {
            if (sb.Length == 0) return;
            output.Add(new Text(sb.ToString()));
            sb.Clear();
        }

        while (i < n)
        {
            var c = text[i];

            if (c == '\\')
            {
                if (i + 1 < n)
                {
                    var nx = text[i + 1];
                    if (nx == '_' || nx == '\\')
                    {
                        sb.Append(nx);
                        i += 2;
                    }
                    else
                    {
                        sb.Append('\\');
                        i++;
                    }
                }
                else
                {
                    sb.Append('\\');
                    i++;
                }

                continue;
            }

            if (c == '_')
            {
                var start = i;
                var run = 0;
                while (i < n && text[i] == '_')
                {
                    run++;
                    i++;
                }

                var processed = 0;
                while (processed < run)
                {
                    var take = run - processed >= 2 ? 2 : 1;
                    var runStart = start + processed;

                    var leftPos = runStart - 1;
                    var rightPos = runStart + take;

                    var leftIsSpace = IsWhiteSpace(leftPos, text);
                    var rightIsSpace = IsWhiteSpace(rightPos, text);

                    var leftIsDigit = IsDigit(leftPos, text);
                    var rightIsDigit = IsDigit(rightPos, text);
                    var isInsideWord = IsInsideWord(leftPos, text) && IsInsideWord(rightPos, text);

                    var canOpen = !rightIsSpace && !rightIsDigit;
                    var canClose = !leftIsSpace && !leftIsDigit;

                    if (!canOpen && !canClose)
                    {
                        sb.Append(new string('_', take));
                    }
                    else
                    {
                        FlushText();
                        delimiters.Add(new Delimiter
                        {
                            NodeIndex = output.Count,
                            Length = take,
                            CanOpen = canOpen,
                            CanClose = canClose,
                            IsInsideWord = isInsideWord,
                            Matched = false
                        });
                    }

                    processed += take;
                }

                continue;
            }

            sb.Append(c);
            i++;
        }

        FlushText();

        for (var d = 0; d < delimiters.Count; d++)
        {
            var closer = delimiters[d];
            if (!closer.CanClose || closer.Matched) continue;

            var j = d - 1;
            while (j >= 0)
            {
                var opener = delimiters[j];
                if (opener.Matched)
                {
                    j--;
                    continue;
                }

                if (!opener.CanOpen)
                {
                    j--;
                    continue;
                }

                if (opener.Length != closer.Length)
                {
                    j--;
                    continue;
                }

                var startIdx = opener.NodeIndex;
                var endIdx = closer.NodeIndex;

                if (endIdx <= startIdx)
                {
                    j--;
                    continue;
                }

                var contentBuilder = new StringBuilder();
                for (var k = startIdx; k < endIdx; k++)
                    if (output[k] is Text t) contentBuilder.Append(t.Content);
                    else contentBuilder.Append(output[k]);

                var contentStr = contentBuilder.ToString();

                if (opener.IsInsideWord)
                    if (string.IsNullOrEmpty(contentStr) || contentStr.IndexOfAny([' ', '\t', '\r', '\n']) >= 0)
                    {
                        j--;
                        continue;
                    }

                var crossing = false;
                for (var m = j + 1; m < d; m++)
                    if (!delimiters[m].Matched && delimiters[m].Length != opener.Length)
                    {
                        crossing = true;
                        break;
                    }

                if (crossing)
                {
                    j--;
                    continue;
                }

                var children = new List<Inline>();
                for (var k = startIdx; k < endIdx; k++)
                    children.Add(output[k]);

                if (opener.Length == 1)
                {
                    output.RemoveRange(startIdx, endIdx - startIdx);
                    output.Insert(startIdx, new Italic(children));
                }
                else
                {
                    output.RemoveRange(startIdx, endIdx - startIdx);
                    output.Insert(startIdx, new Bold(children));
                }

                var removedCount = endIdx - startIdx - 1;
                if (removedCount != 0)
                    foreach (var delimiter in delimiters)
                        if (delimiter.NodeIndex > startIdx)
                            delimiter.NodeIndex -= removedCount;

                opener.Matched = true;
                closer.Matched = true;

                break;
            }
        }

        for (var idx = delimiters.Count - 1; idx >= 0; idx--)
        {
            var delim = delimiters[idx];
            if (!delim.Matched)
            {
                var literal = delim.Length == 1 ? "_" : "__";
                output.Insert(delim.NodeIndex, new Text(literal));
            }
        }

        return output;
    }
    
    private static bool IsWhiteSpace(int pos, string text)
    {
        return pos < 0 || pos >= text.Length || char.IsWhiteSpace(text[pos]);
    }

    private static bool IsDigit(int pos, string text)
    {
        return pos >= 0 && pos < text.Length && char.IsDigit(text[pos]);
    }

    private static bool IsInsideWord(int pos, string text)
    {
        return pos >= 0 && pos < text.Length && char.IsLetter(text[pos]);
    }
}