using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenParser
    {
        private readonly HashSet<TagType> OpenTags;
        private readonly Stack<Tag> TagStack;
        private readonly string Text;
        private readonly List<Tag> Tags;
        private readonly TokenReader Reader;
        public TokenParser(List<(int, LexType)> tokens, string text)
        {
            OpenTags = new HashSet<TagType>();
            TagStack = new Stack<Tag>();
            Text = text;
            Tags = new List<Tag>();
            Reader = new TokenReader(tokens);
        }

        public List<Tag> Parse()
        {
            while (!Reader.EndReached())
            {
                var token = Reader.Current();
                switch (token.Item2)
                {
                    case LexType.Underscore:
                        ProcessTagWithUnderscoreLikeRules(TagType.Em);
                        break;
                    case LexType.DoubleUnderscore:
                        ProcessTagWithUnderscoreLikeRules(TagType.Strong);
                        break;
                    case LexType.DoubleTilda:
                        ProcessTagWithUnderscoreLikeRules(TagType.S);
                        break;
                    case LexType.Text:
                        break;
                    case LexType.TextWithBackslash:
                        ProcessTextWithBackslash();
                        break;
                    case LexType.SquareBracketOpen:
                        ProcessSquareBracketOpen();
                        break;
                    case LexType.SquareBracketClose:
                        ProcessSquareBracketClose();
                        break;
                    case LexType.BracketOpen:
                        ProcessBracketOpen();
                        break;
                    case LexType.BracketClose:
                        ProcessBracketClose();
                        break;
                    default:
                        break;
                }
                Reader.Next();
            }
            GetClosedTagsFromStack();

            return Tags;
        }


        private bool CheckDigits()
        {
            if (Reader.Previous().Item2 == LexType.Text)
                for (int i = Reader.Previous().Item1; i < Reader.CurrentValue(); i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            if (Reader.PeekNext().Item2 == LexType.Text)
                for (int i = Reader.CurrentValue(); i < Reader.PeekNext().Item1; i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            return true;
        }

        private void PopStackUntilFind(TagType tagType, Stack<Tag> stack)
        {
            while (stack.Peek().Type != tagType)
            {
                OpenTags.Remove(stack.Peek().Type);
                stack.Pop();
            }
        }

        private void ProcessTextWithBackslash()
        {
            Tags.Add(new Tag(TagType.Backslash, Reader.CurrentValue()));
        }

        private void ProcessSquareBracketOpen()
        {
            OpenTags.Add(TagType.A);
            TagStack.Push(new Tag(TagType.A, Reader.CurrentValue()));
        }

        private void ProcessSquareBracketClose()
        {
            if (OpenTags.Contains(TagType.A))
            {
                PopStackUntilFind(TagType.A, TagStack);
                OpenTags.Remove(TagType.A);
                if (Reader.PeekNext().Item2 == LexType.BracketOpen) // проверили что дальше будет ссылка
                {
                    Tags.Add(TagStack.Pop());
                    Tags.Add(new Tag(TagType.AClose, Reader.CurrentValue()));
                }
                else
                {
                    TagStack.Pop();
                }
            }
        }

        private void ProcessBracketOpen()
        {
            if (Reader.Previous().Item2 == LexType.SquareBracketClose
                && Tags.Count > 0 && Tags[Tags.Count - 1].Type == TagType.AClose)// проверили что в скобках: ссылка или просто текст
            {
                OpenTags.Add(TagType.LinkBracket);
                TagStack.Push(new Tag(TagType.LinkBracket, Reader.CurrentValue()));
            }
        }

        private void ProcessBracketClose()
        {
            if (OpenTags.Contains(TagType.LinkBracket))
            {
                PopStackUntilFind(TagType.LinkBracket, TagStack);
                OpenTags.Remove(TagType.LinkBracket);
                Tags.Add(TagStack.Pop());
                Tags.Add(new Tag(TagType.LinkBracketClose, Reader.CurrentValue()));
            }
        }

        private void ProcessTagWithUnderscoreLikeRules(TagType tagType)
        {
            if (!CheckDigits())
                return;

            if (!OpenTags.Contains(tagType))
            {
                if (Reader.PeekNext().Item2 == LexType.Space)
                    return;

                OpenTags.Add(tagType);
                TagStack.Push(new Tag(tagType, Reader.CurrentValue()));
            }
            else
            {
                if (Reader.Previous().Item2 == LexType.Space)
                    return;

                if (CheckProhibitedOuterTags(tagType))
                {
                    OpenTags.Remove(tagType);
                    TagStack.Push(new Tag(Tag.Closing(tagType), Reader.CurrentValue()));
                }
                else
                {
                    while (TagStack.Peek().Type != tagType)
                    {
                        if (TagStack.Peek().Type == TagType.A)//в ссылке тег не должен закрываться
                            return;
                        OpenTags.Remove(TagStack.Peek().Type);
                        TagStack.Pop();
                    }
                    OpenTags.Remove(tagType);
                    Tags.Add(TagStack.Pop());
                    Tags.Add(new Tag(Tag.Closing(tagType), Reader.CurrentValue()));
                }
            }
        }

        private bool CheckProhibitedOuterTags(TagType tagType)
        {
            if (!Tag.ProhibitedOuterTags.ContainsKey(tagType))
                return false;
            return OpenTags.Any(x => Tag.ProhibitedOuterTags[tagType].Contains(x));
        }

        private void GetClosedTagsFromStack()
        {
            var reversedStack = new Stack<Tag>();
            var openTags = new HashSet<TagType>();
            while (TagStack.Count != 0)
            {
                var tag = TagStack.Pop();
                if (!openTags.Contains(tag.Opposite()))
                {
                    reversedStack.Push(tag);
                    openTags.Add(tag.Type);
                }
                else
                {
                    Tags.Add(tag);
                    PopStackUntilFind(tag.Opposite(), reversedStack);
                    Tags.Add(reversedStack.Pop());
                }
            }
        }
    }

}
