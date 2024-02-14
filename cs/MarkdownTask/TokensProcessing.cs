using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public class TokensProcessing
    {
        private static readonly Dictionary<TagType, List<TagType>> disallowedNesting = new()
        {
            {TagType.Italic, new(){ TagType.Strong } },
            {TagType.Link, new()
                {
                    TagType.Strong,
                    TagType.Italic,
                    TagType.Link
                }
            },
        };

        public static ICollection<Token> Process(ICollection<Token> tokens)
        {
            return SortAndCheckNesting(tokens);
        }

        private static ICollection<Token> SortAndCheckNesting(ICollection<Token> tokens)
        {
            var selected = new List<Token>();
            var stack = new Stack<Token>();

            foreach (var token in tokens.OrderBy(x => x.Position))
            {
                if (token.TagType == TagType.Empty)
                {
                    selected.Add(token);
                    continue;
                }

                if (token.Tag == Tag.Open)
                {
                    stack.Push(token);
                }
                else
                {
                    var pair = TryFormPair(stack, token);

                    selected.AddRange(pair);
                }
            }

            return selected.OrderBy(x => x.Position).ToList();
        }

        private static bool IsAllowedNesting(Token parent, Token nested)
        {
            if (parent.TagType == TagType.Empty)
            {
                return true;
            }

            List<TagType> disallowed;

            if (!disallowedNesting.TryGetValue(parent.TagType, out disallowed))
            {
                return true;
            }

            return !disallowed.Contains(nested.TagType);
        }

        private static List<Token> TryFormPair(Stack<Token> opened, Token closing)
        {
            if (!opened.Any())
                return new List<Token>();

            var opening = opened.Pop();

            if (opening.TagType != closing.TagType)
            {
                return new List<Token>();
            }

            if (!opened.Any() || IsAllowedNesting(opened.Peek(), opening))
            {
                return new List<Token> { opening, closing };
            }

            return new List<Token>();
        }
    }
}
