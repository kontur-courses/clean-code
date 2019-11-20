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
        private readonly List<(int, LexType)> Tokens;
        private readonly string Text;
        private int Counter;
        private List<(int, Tag)> Tags;
        public TokenParser(List<(int, LexType)> tokens, string text)
        {
            OpenTags = new HashSet<Tag>();
            TagStack = new Stack<(int, Tag)>();
            Tokens = tokens;
            Text = text;
            Counter = 0;
            Tags = new List<(int, Tag)>();
        }

        public List<(int, Tag)> Parse()
        {
            while (Counter < Tokens.Count)
            {
                var pair = Tokens[Counter];
                switch (pair.Item2)
                {
                    case LexType.Underscore:
                        ProcessUnderscore();
                        break;
                    case LexType.DoubleUnderscore:
                        ProcessTwoUnderscores();
                        break;
                    case LexType.Text:
                        break;
                    case LexType.TextWithBackslash:
                        Tags.Add((pair.Item1, Tag.Backslash));
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
                Counter++;
            }

            return Tags;
        }


        private bool CheckDigits()
        {
            if (Counter > 0 && Tokens[Counter-1].Item2==LexType.Text)
                for (int i= Tokens[Counter - 1].Item1; i< Tokens[Counter].Item1;i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            if (Counter < Tokens.Count-1 && Tokens[Counter + 1].Item2 == LexType.Text)
                for (int i = Tokens[Counter].Item1; i < Tokens[Counter + 1].Item1; i++)
                    if (Char.IsDigit(Text[i]))
                        return false;
            return true;
        }

        private void ProcessUnderscore()
        {
            if (CheckDigits())
                if (!OpenTags.Contains(Tag.Em))
                {
                    if (Counter == Tokens.Count - 1 || Tokens[Counter + 1].Item2 != LexType.Space)
                    {
                        OpenTags.Add(Tag.Em);
                        TagStack.Push((Tokens[Counter].Item1, Tag.Em));
                    }
                }
                else
                {
                    if (Tokens[Counter - 1].Item2 != LexType.Space)
                    {
                        while (TagStack.Peek().Item2 != Tag.Em)
                        {
                            OpenTags.Remove(TagStack.Peek().Item2);
                            TagStack.Pop();
                        }
                        OpenTags.Remove(Tag.Em);
                        Tags.Add(TagStack.Pop());
                        Tags.Add((Tokens[Counter].Item1, Tag.EmClose));
                    }
                }
        }

        private void ProcessSquareBracketOpen()
        {
            OpenTags.Add(Tag.A);
            TagStack.Push((Tokens[Counter].Item1, Tag.A));
        }

        private void ProcessSquareBracketClose()
        {
            while (TagStack.Peek().Item2 != Tag.A)
            {
                OpenTags.Remove(TagStack.Peek().Item2);
                TagStack.Pop();
            }
            OpenTags.Remove(Tag.A);
            if (Tokens[Counter + 1].Item2 == LexType.BracketOpen)// проверили что это не просто текст в скобках
            {
                Tags.Add(TagStack.Pop());
                Tags.Add((Tokens[Counter].Item1, Tag.AClose));
            }
            else
            {
                TagStack.Pop();
            }
        }

        private void ProcessBracketOpen()
        {
            if (Tokens[Counter - 1].Item2 == LexType.SquareBracketClose)
            {
                OpenTags.Add(Tag.LinkBracket);
                TagStack.Push((Tokens[Counter].Item1, Tag.LinkBracket));
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
                Tags.Add((Tokens[Counter].Item1, Tag.LinkBracketClose));
            }
        }

        private void ProcessTwoUnderscores()
        {
            if (!OpenTags.Contains(Tag.Strong))
            {
                if (Counter == Tokens.Count - 1 || Tokens[Counter + 1].Item2 != LexType.Space)
                {
                    OpenTags.Add(Tag.Strong);
                    TagStack.Push((Tokens[Counter].Item1, Tag.Strong));
                }
            }
            else
            {
                if (Tokens[Counter - 1].Item2 != LexType.Space)
                {
                    while (TagStack.Peek().Item2 != Tag.Strong)
                    {
                        OpenTags.Remove(TagStack.Peek().Item2);
                        TagStack.Pop();
                    }

                    OpenTags.Remove(Tag.Strong);
                    Tags.Add(TagStack.Pop());
                    Tags.Add((Tokens[Counter].Item1, Tag.StrongClose));
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
        LinkBracketClose

    }
}
