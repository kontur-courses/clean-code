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

        public static void PrepareBorders()
        {
            foreach (var descriptor in TagDescriptors)
            {
                Borders.Add(descriptor.Border);
                foreach (var character in descriptor.Border) AllowedTagCharacters.Add(character);
            }
        }

        public static string Render(string paragraph)
        {
            //TODO LINQ maybe
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
                        var rawToken = collectedToken.ToString();
                        //Check whether collected token is allowed token
                        if (Borders.Contains(rawToken))
                        {
                            if (IsWhitespaceOrStringBorderAt(paragraph, i - rawToken.Length) &&
                                !char.IsWhiteSpace(paragraph[i + 1]))
                                yield return new MdToken(i + 1 - rawToken.Length, rawToken, MdTokenMark.Left);
                            else if (IsWhitespaceOrStringBorderAt(paragraph, i + 1) &&
                                     !char.IsWhiteSpace(paragraph[i - rawToken.Length]))
                                yield return new MdToken(i + 1 - rawToken.Length, rawToken, MdTokenMark.Right);
                        }

                        collectedToken.Clear();
                    }

                    state = character == EscapeChar ? ExtractorState.SkippingTokens : ExtractorState.SkippingLetters;
                }

                switch (state)
                {
                    case ExtractorState.CollectingToken:
                        collectedToken.Append(character);
                        break;
                    case ExtractorState.SkippingLetters:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static MdTagDescriptor GetDescriptorForBorder(string border)
        {
            return TagDescriptors.SingleOrDefault(t => t.Border == border);
        }

        private static bool IsWhitespaceOrStringBorderAt(string paragraph, int position)
        {
            return position >= paragraph.Length || char.IsWhiteSpace(paragraph[position]);
        }

        private static IEnumerable<MdTag> FilterMdTags(IEnumerable<MdToken> tokens)
        {
            var leftTokenList = new List<MdToken>();
            foreach (var token in tokens)
                if (token.Mark == MdTokenMark.Left)
                    leftTokenList.Add(token);
                else
                    for (var i = leftTokenList.Count - 1; i >= 0; i--)
                        if (leftTokenList[i].Value == token.Value)
                        {
                            var descriptor = GetDescriptorForBorder(token.Value);
                            var tag = new MdTag(leftTokenList[i], token, descriptor);
                            if (!leftTokenList.Select(t => GetDescriptorForBorder(t.Value))
                                .Any(d => tag.ForbiddenInside(d)))
                                yield return tag;
                            leftTokenList.RemoveRange(i, leftTokenList.Count - i - 1);
                            break;
                        }
        }

        private static string RenderTags(IEnumerable<MdTag> tags, string paragraph)
        {
            var indices = GetTokenIndices(tags);
            var htmlBuilder = new StringBuilder();
            var shift = 0;
            for (var i = 0; i < paragraph.Length; i++)
                if (indices.Contains(i + shift))
                {
                    var tag = tags.First(t => t.LeftBorder.Pos == i + shift ||
                                              t.RightBorder.Pos == i + shift);
                    var tagDescriptor = tag.Descriptor;
                    var replacement = tag.LeftBorder.Pos == i + shift
                        ? tagDescriptor.LeftReplacement
                        : tagDescriptor.RightReplacement;
                    htmlBuilder.Append(replacement);
                    i += tagDescriptor.Border.Length - 1;
                    shift += replacement.Length - tagDescriptor.Border.Length;
                }
                else
                {
                    htmlBuilder.Append(paragraph[i]);
                }

            return htmlBuilder.ToString();
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
        public MdTokenMark Mark;
        public int Pos;

        public string Value;

        public MdToken(int position, string value, MdTokenMark mark)
        {
            Pos = position;
            Value = value;
            Mark = mark;
        }
    }

    public class MdTag
    {
        public MdTagDescriptor Descriptor;
        public MdToken LeftBorder;
        public MdToken RightBorder;

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

        public readonly List<MdTagDescriptor> ForbiddenTagContexts = new List<MdTagDescriptor>();

        public string LeftReplacement;
        public string RightReplacement;


        public MdTagDescriptor(string border, string leftReplacement, string rightReplacement)
        {
            Border = border;
            LeftReplacement = leftReplacement;
            RightReplacement = rightReplacement;
        }

        public void SetForbiddenInside(MdTagDescriptor tagDescriptor)
        {
            ForbiddenTagContexts.Add(tagDescriptor);
        }

        public bool ForbiddenInside(MdTagDescriptor parentTagDescriptor)
        {
            return ForbiddenTagContexts.Contains(parentTagDescriptor);
        }
    }


    /// <summary>
    ///     Mark used to mark token if it can behave as left or right border of selection
    ///     or if it can only be singular
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