using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        private const char EscapeChar = '\\';
        private static readonly List<MdTagDescriptor> TagDescriptors = new List<MdTagDescriptor>();
        private static HashSet<string> _borders;
        private static HashSet<char> _allowedTagCharacters;

        static Md()
        {
            var emphasizeTagDescr = new MdTagDescriptor("_", "<em>", "</em>");
            var strongTagDescr = new MdTagDescriptor("__", "<strong>", "</strong>");
            emphasizeTagDescr.SetInternalTokenProcessor(t => t.SetSkipped(t.Value == "__"));
            TagDescriptors.Add(emphasizeTagDescr);
            TagDescriptors.Add(strongTagDescr);
            PrepareBorders();
        }

        private static void PrepareBorders()
        {
            _borders = TagDescriptors.Select(d => d.Border).ToHashSet();
            _allowedTagCharacters = TagDescriptors.Select(d => d.Border)
                .SelectMany(b => b.ToCharArray()).Distinct()
                .ToHashSet();
        }

        public static string Render(string paragraph)
        {
            var tokens = ExtractMdTokensFromText(paragraph);
            var tags = FilterMdTags(tokens);
            return RenderTags(tags, paragraph);
        }

        private static IEnumerable<MdToken> ExtractMdTokensFromText(string paragraph)
        {
            var collector = new StringBuilder();
            var skipToken = false;
            for (var i = 0; i < paragraph.Length; i++)
            {
                var character = paragraph[i];

                if (_allowedTagCharacters.Contains(character))
                {
                    collector.Append(character);
                    continue;
                }

                if (!skipToken && TryExtractToken(paragraph, collector, i - collector.Length, out var token))
                    yield return token;

                skipToken = character is EscapeChar;
                collector.Clear();
            }

            if (TryExtractToken(paragraph, collector, paragraph.Length - collector.Length, out var residualToken))
                yield return residualToken;
        }


        private static List<MdTag> FilterMdTags(IEnumerable<MdToken> tokens)
        {
            var tokenList = tokens.ToList();
            var tagList = new List<MdTag>();
            var leftTokenList = new List<MdToken>();
            foreach (var token in tokenList)
                if (token.Mark == MdTokenMark.StartingToken)
                {
                    Console.WriteLine("left" + token.Value);
                    leftTokenList.Add(token);
                }
                else
                {
                    for (var i = leftTokenList.Count - 1; i >= 0; i--)
                    {
                        if (leftTokenList[i].Value != token.Value) continue;

                        var descriptor = GetDescriptorForBorder(token.Value);
                        var tag = new MdTag(leftTokenList[i], token, descriptor);
                        tagList.Add(tag);
                        Console.WriteLine("Border" + tag.Descriptor.Border);
                        descriptor.ProcessTokensInsideTag(tokenList, tag);

                        leftTokenList.RemoveRange(i, leftTokenList.Count - i);
                        break;
                    }
                }

            return tagList;
        }


        private static string RenderTags(List<MdTag> tags, string paragraph)
        {
            var indices = GetTokenIndices(tags);
            var htmlBuilder = new StringBuilder();
            for (var i = 0; i < paragraph.Length; i++)
                if (indices.Contains(i))
                {
                    var tag = tags.First(t => t.LeftBorder.Pos == i || t.RightBorder.Pos == i);
                    var replacement = GetReplacementForTagAtPos(i, tag);

                    htmlBuilder.Append(replacement);
                    i += tag.Descriptor.Border.Length - 1;
                }
                else
                {
                    if (paragraph[i] is EscapeChar && i + 1 < paragraph.Length) i++;

                    htmlBuilder.Append(paragraph[i]);
                }

            return htmlBuilder.ToString();
        }

        private static string GetReplacementForTagAtPos(int i, MdTag tag)
        {
            return i == tag.LeftBorder.Pos
                ? tag.Descriptor.LeftReplacement
                : tag.Descriptor.RightReplacement;
        }

        private static bool TryExtractToken(string paragraph, StringBuilder collectedToken,
            int tokenBeginning, out MdToken result)
        {
            var rawToken = collectedToken.ToString();
            result = null;

            if (rawToken.Length == 0 || !_borders.Contains(rawToken))
                return false;

            var tokenIsValid = TryGetTokenMarkBySurroundings(tokenBeginning - 1, tokenBeginning + rawToken.Length,
                paragraph, out var mark);
            if (tokenIsValid) result = new MdToken(tokenBeginning, rawToken, mark);

            return tokenIsValid;
        }

        private static bool TryGetTokenMarkBySurroundings(int leftNeighbourPos, int rightNeighbourPos,
            string paragraph, out MdTokenMark mark)
        {
            mark = MdTokenMark.Unmarked;

            var canBeStartingToken = IsWhitespaceOrStringEnd(leftNeighbourPos, paragraph);
            var canBeClosingToken = IsWhitespaceOrStringEnd(rightNeighbourPos, paragraph);

            if (canBeStartingToken && !canBeClosingToken) mark = MdTokenMark.StartingToken;
            if (!canBeStartingToken && canBeClosingToken) mark = MdTokenMark.ClosingToken;

            return mark != MdTokenMark.Unmarked;
        }

        private static MdTagDescriptor GetDescriptorForBorder(string border)
        {
            return TagDescriptors.SingleOrDefault(t => t.Border == border);
        }

        private static bool IsWhitespaceOrStringEnd(int position, string paragraph)
        {
            return position >= paragraph.Length || position < 0 || char.IsWhiteSpace(paragraph[position]);
        }

        private static HashSet<int> GetTokenIndices(IEnumerable<MdTag> tags)
        {
            return tags.Where(t => !t.Skipped).SelectMany(t => new[] {t.LeftBorder, t.RightBorder}).Select(t => t.Pos)
                .ToHashSet();
        }
    }
}