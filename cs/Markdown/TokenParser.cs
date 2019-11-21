using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Markdown
{
    public class TokenParser
    {
        private readonly HashSet<Tag> OpenTags;
        private readonly Stack<(int, Tag)> TagStack;
        //private readonly List<(int, LexType)> Tokens;
        private readonly string Text;
        //private int Counter;
        private List<(int, Tag)> Tags;
        private Dictionary<Tag, HashSet<Tag>> ProhibitedOuterTags;
        private readonly TokenReader Reader;
        public TokenParser(List<(int, LexType)> tokens, string text)
        {
            OpenTags = new HashSet<Tag>();
            TagStack = new Stack<(int, Tag)>();
            //Tokens = tokens;
            Text = text;
            //Counter = 0;
            Tags = new List<(int, Tag)>();
            Reader = new TokenReader(tokens);

            ProhibitedOuterTags = new Dictionary<Tag, HashSet<Tag>>();

            ProhibitedOuterTags.Add(Tag.Strong, new HashSet<Tag> {Tag.Em });
        }

        public List<(int, Tag)> Parse()
        {
            while (!Reader.EndReached())
            {
                var token = Reader.Current();
                switch (token.Item2)
                {
                    case LexType.Underscore:
                        ProcessTagWithUnderscoreLikeRules(Tag.Em, Tag.EmClose);
                        break;
                    case LexType.DoubleUnderscore:
                        ProcessTagWithUnderscoreLikeRules(Tag.Strong, Tag.StrongClose);
                        break;
                    case LexType.DoubleTilda:
                        ProcessTagWithUnderscoreLikeRules(Tag.S, Tag.SClose);
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

            //now we should load closed tags from stack
            //нужно пройти по стеку и найти парные теги

            return Tags;
        }


        private bool CheckDigits()
        {
            if (Reader.Previous().Item2==LexType.Text)
                for (int i= Reader.Previous().Item1; i< Reader.Current().Item1;i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            if (Reader.PeekNext().Item2 == LexType.Text)
                for (int i = Reader.Current().Item1; i < Reader.PeekNext().Item1; i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            return true;
        }

        private void ProcessTextWithBackslash()
        {
            Tags.Add((Reader.Current().Item1, Tag.Backslash));
        }

        private void ProcessSquareBracketOpen()
        {
            OpenTags.Add(Tag.A);
            TagStack.Push((Reader.Current().Item1, Tag.A));
        }

        private void ProcessSquareBracketClose()
        {
            while (TagStack.Peek().Item2 != Tag.A)
            {
                OpenTags.Remove(TagStack.Peek().Item2);
                TagStack.Pop();
            }
            OpenTags.Remove(Tag.A);
            if (Reader.PeekNext().Item2 == LexType.BracketOpen)// проверили что это не просто текст в квадратных скобках
            {
                Tags.Add(TagStack.Pop());
                Tags.Add((Reader.Current().Item1, Tag.AClose));
            }
            else
            {
                TagStack.Pop();
            }
        }

        private void ProcessBracketOpen()
        {
            if (Reader.Previous().Item2 == LexType.SquareBracketClose)// проверили что в скобках: ссылка или просто текст
            {
                OpenTags.Add(Tag.LinkBracket);
                TagStack.Push((Reader.Current().Item1, Tag.LinkBracket));
            }
        }

        private void ProcessBracketClose()
        {
            if (OpenTags.Contains(Tag.LinkBracket))
            {
                while (TagStack.Peek().Item2 != Tag.LinkBracket)
                {
                    OpenTags.Remove(TagStack.Peek().Item2);
                    TagStack.Pop();
                }

                OpenTags.Remove(Tag.LinkBracket);
                Tags.Add(TagStack.Pop());
                Tags.Add((Reader.Current().Item1, Tag.LinkBracketClose));
            }
        }


        /* simple tag не взаимодействует с другими тегами
         * не имеет особых условий открытия или закрытия
         */

        private void ProcessSimpleTag(Tag tag, Tag tagClose)
        {
            if (!OpenTags.Contains(tag))
            {
                OpenTags.Add(tag);
                TagStack.Push((Reader.Current().Item1, tag));
            }
            else
            {
                while (TagStack.Peek().Item2 != tag)
                {
                    OpenTags.Remove(TagStack.Peek().Item2);
                    TagStack.Pop();
                }
                OpenTags.Remove(tag);
                Tags.Add(TagStack.Pop());
                Tags.Add((Reader.Current().Item1, tagClose));
            }
        }

        private void ProcessTagWithUnderscoreLikeRules(Tag tag, Tag tagClose)
        {
            if (!CheckDigits())
                return;
            if (!OpenTags.Contains(tag))
            {
                if (Reader.PeekNext().Item2 != LexType.Space)
                {
                    OpenTags.Add(tag);
                    TagStack.Push((Reader.Current().Item1, tag));
                }
            }
            else
            {
                if (Reader.Previous().Item2 != LexType.Space)
                {
                    if (ProhibitedOuterTags.ContainsKey(tag) && OpenTags.Select(x => ProhibitedOuterTags[tag].Contains(x)).Contains(true))
                    {
                        OpenTags.Remove(tag);
                        TagStack.Push((Reader.Current().Item1, tag));// здесь надо пушить закрывающий тег
                    }
                    else
                    {
                        while (TagStack.Peek().Item2 != tag)
                        {
                            if (TagStack.Peek().Item2==Tag.A)//в ссылке тег не должен закрываться (костыль, надо пофиксить)
                                return;
                            OpenTags.Remove(TagStack.Peek().Item2);
                            TagStack.Pop();
                        }

                        OpenTags.Remove(tag);
                        Tags.Add(TagStack.Pop());
                        Tags.Add((Reader.Current().Item1, tagClose));
                    }
                }
            }
        }
    }

    public enum Tag
    {
        Em,
        EmClose,
        Strong,
        StrongClose,
        Backslash,
        A,
        AClose,
        LinkBracket,
        LinkBracketClose,
        S,
        SClose

    }
}
