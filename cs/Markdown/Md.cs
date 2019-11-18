using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        private const char EscapeChar = '\\';
        private static readonly List<MdTagDescriptor> TagDescriptors = new List<MdTagDescriptor>();
        private static readonly HashSet<string> Borders = new HashSet<string>();
        private static readonly HashSet<char> AllowedTagCharacters = new HashSet<char>();

        static Md()
        {
            var emphasizeTagDescr = new MdTagDescriptor("_", "<em>", "</em>");
            var strongTagDescr = new MdTagDescriptor("__", "<strong>", "</strong>");
            strongTagDescr.SetForbiddenInside(emphasizeTagDescr);
            TagDescriptors.Add(emphasizeTagDescr);
            TagDescriptors.Add(strongTagDescr);
            PrepareBorders();
        }

        private static void PrepareBorders()
        {
            foreach (var descriptor in TagDescriptors)
            {
                Borders.Add(descriptor.Border);
                foreach (var character in descriptor.Border) AllowedTagCharacters.Add(character);
            }
        }

        public static string Render(string paragraph)
        {
            var tokens = ExtractMdTokensFromText(paragraph);
            var tags = FilterMdTags(tokens);
            return RenderTags(tags, paragraph);
        }

        private static IEnumerable<MdToken> ExtractMdTokensFromText(string paragraph)
        {
            var state = ExtractorState.CollectingToken;
            var collectedToken = new StringBuilder();
            for (var i = 0; i < paragraph.Length; i++)
            {
                var character = paragraph[i];
                if (state is ExtractorState.SkippingTokens && AllowedTagCharacters.Contains(character))
                    continue;

                if (AllowedTagCharacters.Contains(character))
                {
                    state = ExtractorState.CollectingToken;
                }
                else
                {
                    if (collectedToken.Length > 0)
                    {
                        var mdToken = CheckCollectedToken(paragraph, collectedToken, i);
                        if (mdToken != null) yield return mdToken;
                        collectedToken.Clear();
                    }

                    state = character == EscapeChar
                        ? ExtractorState.SkippingTokens
                        : ExtractorState.SkippingLetters;
                }

                if (state == ExtractorState.CollectingToken)
                    collectedToken.Append(character);
            }

            if (collectedToken.Length > 0)
            {
                var mdToken = CheckCollectedToken(paragraph, collectedToken, paragraph.Length);
                if (mdToken != null) yield return mdToken;
                collectedToken.Clear();
            }
        }


        private static IEnumerable<MdTag> FilterMdTags(IEnumerable<MdToken> tokens)
        {
            var leftTokenList = new List<MdToken>();
            foreach (var token in tokens)
                if (token.Mark == MdTokenMark.Left)
                    leftTokenList.Add(token);
                else
                    for (var i = leftTokenList.Count - 1; i >= 0; i--)
                    {
                        if (leftTokenList[i].Value != token.Value) continue;

                        var descriptor = GetDescriptorForBorder(token.Value);
                        var tag = new MdTag(leftTokenList[i], token, descriptor);

                        if (IsAllowedInsideAny(leftTokenList, tag))
                            yield return tag;

                        leftTokenList.RemoveRange(i, leftTokenList.Count - i);
                        break;
                    }
        }

        private static string RenderTags(IEnumerable<MdTag> tags, string paragraph)
        {
            var tagList = tags.ToList();
            var indices = GetTokenIndices(tagList);
            var htmlBuilder = new StringBuilder();
            for (var i = 0; i < paragraph.Length; i++)
                if (indices.Contains(i))
                {
                    var tag = tagList.First(t => t.LeftBorder.Pos == i || t.RightBorder.Pos == i);

                    var replacement = i == tag.LeftBorder.Pos
                        ? tag.Descriptor.LeftReplacement
                        : tag.Descriptor.RightReplacement;

                    htmlBuilder.Append(replacement);

                    i += tag.Descriptor.Border.Length - 1;
                }
                else
                {
                    if (paragraph[i] is EscapeChar && i + 1 < paragraph.Length)
                    {
                        htmlBuilder.Append(paragraph[i + 1]);
                        i++;
                    }
                    else
                    {
                        htmlBuilder.Append(paragraph[i]);
                    }
                }

            return htmlBuilder.ToString();
        }

        private static MdToken CheckCollectedToken(string paragraph, StringBuilder collectedToken, int i)
        {
            var rawToken = collectedToken.ToString();

            //Check whether collected token is allowed token
            if (!Borders.Contains(rawToken)) return null;

            if (IsWhitespaceOrStringBorderAt(paragraph, i - 1 - rawToken.Length) &&
                !IsWhitespaceOrStringBorderAt(paragraph, i))
                return new MdToken(i - rawToken.Length, rawToken, MdTokenMark.Left);

            if (IsWhitespaceOrStringBorderAt(paragraph, i) &&
                !IsWhitespaceOrStringBorderAt(paragraph, i - 1 - rawToken.Length))
                return new MdToken(i - rawToken.Length, rawToken, MdTokenMark.Right);

            return null;
        }

        private static MdTagDescriptor GetDescriptorForBorder(string border)
        {
            return TagDescriptors.SingleOrDefault(t => t.Border == border);
        }

        private static bool IsWhitespaceOrStringBorderAt(string paragraph, int position)
        {
            return position >= paragraph.Length || position < 0 || char.IsWhiteSpace(paragraph[position]);
        }


        private static bool IsAllowedInsideAny(IEnumerable<MdToken> leftTokenList, MdTag tag)
        {
            return !leftTokenList.Select(t => GetDescriptorForBorder(t.Value)).Any(tag.ForbiddenInside);
        }


        private static HashSet<int> GetTokenIndices(IEnumerable<MdTag> tags)
        {
            return tags.SelectMany(t => new[] {t.LeftBorder, t.RightBorder}).Select(t => t.Pos).ToHashSet();
        }
    }

    /// <summary>
    ///     Token data container
    /// </summary>
    public class MdToken
    {
        public readonly MdTokenMark Mark;
        public readonly int Pos;

        public readonly string Value;

        public MdToken(int position, string value, MdTokenMark mark)
        {
            Pos = position;
            Value = value;
            Mark = mark;
        }
    }

    public class MdTag
    {
        public readonly MdTagDescriptor Descriptor;
        public readonly MdToken LeftBorder;
        public readonly MdToken RightBorder;

        public MdTag(MdToken leftBorder, MdToken rightBorder, MdTagDescriptor descriptor)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Descriptor = descriptor;
        }

        public bool ForbiddenInside(MdTagDescriptor parentTagDescriptor)
        {
            return Descriptor.ForbiddenInside(parentTagDescriptor);
        }
    }

    public class MdTagDescriptor
    {
        public readonly string Border;

        private readonly List<MdTagDescriptor> forbiddenTagContexts = new List<MdTagDescriptor>();

        public readonly string LeftReplacement;
        public readonly string RightReplacement;


        public MdTagDescriptor(string border, string leftReplacement, string rightReplacement)
        {
            Border = border;
            LeftReplacement = leftReplacement;
            RightReplacement = rightReplacement;
        }

        public void SetForbiddenInside(MdTagDescriptor tagDescriptor)
        {
            forbiddenTagContexts.Add(tagDescriptor);
        }

        public bool ForbiddenInside(MdTagDescriptor parentTagDescriptor)
        {
            return forbiddenTagContexts.Contains(parentTagDescriptor);
        }
    }


    /// <summary>
    ///     Mark used to mark token if it can behave as left or right border of selection
    /// </summary>
    public enum MdTokenMark
    {
        Left,
        Right
    }

    public enum ExtractorState
    {
        CollectingToken,
        SkippingLetters,
        SkippingTokens
    }
}