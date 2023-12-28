using Markdown.Tokens;
using System.Data.SqlTypes;

namespace Markdown.Parsers
{
    public class InlineParser
    {
        private HashSet<TokenType> inlineTokens;

        private string text;

        public InlineParser()
        {
            inlineTokens = new HashSet<TokenType>()
            { TokenType.Bold, TokenType.Italic};
        }

        public List<MdTag> Parse(Token[] tokens, string text)
        {
            this.text = text;

            var correctToken = GetCorrectTokens(tokens);

            var boldOpened = false;
            var italicOpened = false;
            var prevIndex = 0;
            var tags = new List<MdTag>();
            NestedTag curTag = null;

            foreach (var token in correctToken)
            {
                AddTextToCurrenTag(text.Substring(prevIndex, token.Index - prevIndex));

                if (token.Type == TokenType.Italic)
                    AddItalic();
                if (token.Type == TokenType.Bold)
                    AddBold();

                prevIndex = token.Index + token.Offset;
            }

            tags.Add(new AtomicTag(text[prevIndex..], TagType.Text));

            return tags;


            void AddItalic()
            {
                if (italicOpened)
                {
                    if (boldOpened)
                        curTag = tags[^1] as NestedTag;
                    else
                        curTag = null;
                    italicOpened = false;
                }
                else
                {
                    var tag = new NestedTag(TagType.Italic);
                    if (boldOpened)
                        curTag.Tags.Add(tag);
                    else
                        tags.Add(tag);
                    curTag = tag;
                    italicOpened = true;
                }
            }

            void AddBold()
            {
                if (boldOpened)
                {
                    curTag = null;
                    boldOpened = false;
                }
                else
                {
                    var tag = new NestedTag(TagType.Bold);
                    tags.Add(tag);
                    curTag = tag;
                    boldOpened = true;
                }
            }

            void AddTextToCurrenTag(string text)
            {
                if (boldOpened || italicOpened)
                    curTag.Tags.Add(new AtomicTag(text, TagType.Text));
                else
                {
                    if (text.Length != 0)
                    {
                        var tag = new AtomicTag(text, TagType.Text);
                        tags.Add(tag);
                    }
                }
            }
        }

        private Token[] GetCorrectTokens(Token[] tokens)
        {
            tokens = tokens.Where(token => inlineTokens.Contains(token.Type)).ToArray();

            var correctTokens = new List<Token>();
            var stack = new Stack<Token>();
            var maybeCorrect = new List<Token>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (stack.Count == 0 || stack.Peek().Type != tokens[i].Type)
                    stack.Push(tokens[i]);
                else
                {
                    if (stack.Count == 3)
                    {
                        var second = stack.Pop();
                        stack.Pop();
                        AddIfCorrect(stack.Pop(), second);

                        stack.Push(tokens[i]);

                        continue;
                    }

                    var first = stack.Pop();
                    AddIfCorrect(first, tokens[i]);
                }

                if (stack.Count == 4)
                    stack.Clear();
            }

            correctTokens.AddRange(maybeCorrect);

            return correctTokens.OrderBy(x => x.Index).ToArray();

            void AddIfCorrect(Token fisrt, Token second)
            {
                if (AreCorrectPair(fisrt, second))
                {
                    if (fisrt.Type == TokenType.Bold &&
                        stack.Count != 0 &&
                        stack.Peek().Type == TokenType.Italic)
                    {
                        maybeCorrect.Add(fisrt);
                        maybeCorrect.Add(second);
                    }
                    else
                    {
                        if (fisrt.Type == TokenType.Italic)
                            maybeCorrect.Clear();
                        correctTokens.Add(fisrt);
                        correctTokens.Add(second);
                    }
                }
            }
        }

        private bool AreCorrectPair(Token first, Token second)
        {
            var firstInWord = IsInWord(first);
            var secondInWord = IsInWord(second);

            if (char.IsWhiteSpace(text[first.Index + first.Offset]))
                return false;

            for (int i = first.Index + first.Offset; i < second.Index; i++)
            {
                if (!char.IsLetter(text[i]) && (IsInWord(first) || IsInWord(second)))
                    return false;
            }

            return (second.Index  - (first.Index + first.Offset - 1)) > 1;
        }

        private bool IsInWord(Token token)
        {
            var lIndex = token.Index - 1;
            var rIndex = token.Index + token.Offset;

            var left = token.Index == 0 ? false : !char.IsWhiteSpace(text[lIndex]);
            var right = token.Index == text.Length - token.Offset ? false : 
                !char.IsWhiteSpace(text[rIndex]);

            return left && right;
        }
    }
}