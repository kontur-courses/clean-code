using Markdown.TokenizerLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class OldTokenizer
    {
        public IEnumerable<Token> ProcessMarkdown(string markdownText)
        {
            //По сути тот же токенайзер, но без декомпозиции
            //Декомпозированный вариант в грубой отметке на 78% медленнее
            var rawTokens = ToRawTokens(markdownText);
            var filteredTokens = FilterRawTokens(rawTokens);
            var pairedTokens = PairFilteredTokens(filteredTokens);

            return pairedTokens;
        }

        private IEnumerable<Token> PairFilteredTokens(IEnumerable<Token> filteredTokens)
        {
            var toPair = new Stack<PairedTokenInfo>();
            var isWord = false;
            var withDigits = false;
            var isLastPaired = false;

            foreach (var token in filteredTokens)
            {
                switch (token)
                {
                    case NewlineToken nl:
                        {
                            if (toPair.Count > 0
                                && toPair.Peek().CanOpen
                                && toPair.Peek().CanClose)
                                toPair.Peek().DisapleOpen();
                            isLastPaired = false;
                            isWord = false;
                            withDigits = false;
                            foreach (var unpaired in toPair)
                                unpaired.Token.Escape();
                            toPair.Clear();
                            break;
                        }
                    case UnorderedListToken ul:
                        {
                            if (toPair.Count > 0
                                && toPair.Peek().CanOpen
                                && toPair.Peek().CanClose)
                                toPair.Peek().DisapleOpen();
                            isLastPaired = false;
                            isWord = false;
                            withDigits = false;
                            foreach (var unpaired in toPair)
                                unpaired.Token.Escape();
                            toPair.Clear();
                            break;
                        }
                    case ListItemToken li:
                        {
                            if (toPair.Count > 0
                                && toPair.Peek().CanOpen
                                && toPair.Peek().CanClose)
                                toPair.Peek().DisapleOpen();
                            isLastPaired = false;
                            isWord = false;
                            withDigits = false;
                            foreach (var unpaired in toPair)
                                unpaired.Token.Escape();
                            toPair.Clear();
                            break;
                        }
                    case HeaderToken h:
                        {
                            if (toPair.Count > 0
                                && toPair.Peek().CanOpen
                                && toPair.Peek().CanClose)
                                toPair.Peek().DisapleOpen();
                            isLastPaired = false;
                            isWord = false;
                            withDigits = false;
                            break;
                        }
                    case SpaceToken s:
                        {
                            if (toPair.Count > 0
                                && toPair.Peek().CanOpen
                                && toPair.Peek().CanClose)
                                toPair.Peek().DisapleOpen();
                            isLastPaired = false;
                            isWord = false;
                            withDigits = false;
                            break;
                        }
                    case EscapeToken e:
                        {
                            if (isLastPaired)
                            {
                                toPair.Peek().Open();
                                isLastPaired = false;
                            }
                            isWord = true;
                            break;
                        }
                    case TextToken t:
                        {
                            if (isLastPaired)
                            {
                                toPair.Peek().Open();
                                isLastPaired = false;
                            }
                            isWord = true;
                            withDigits = t.WithDigits ? true : withDigits;
                            break;
                        }
                    case ItalicToken i:
                        {
                            if (i.IsEscaped)
                                break;

                            if (isWord)
                            {
                                if (toPair.Count > 0
                                && toPair.Peek().Token is ItalicToken
                                && toPair.Peek().CanOpen)
                                {
                                    var pair = toPair.Pop();

                                    if (withDigits)
                                    {
                                        pair.Token.Escape();
                                        i.Escape();
                                    }
                                    else
                                    {
                                        i.Close();
                                        isWord = false;
                                    }
                                    isLastPaired = false;
                                }
                                else
                                {
                                    var newPair = new PairedTokenInfo(i);
                                    newPair.Close();
                                    toPair.Push(newPair);

                                    isLastPaired = true;
                                }
                            }
                            else
                            {
                                isLastPaired = true;
                                var newPair = new PairedTokenInfo(i);
                                newPair.Open();
                                toPair.Push(newPair);
                            }
                            break;
                        }
                    case BoldToken b:
                        {
                            if (b.IsEscaped)
                                break;

                            if (isWord)
                            {
                                if (toPair.Count > 0
                                && !(toPair.Peek().Token is ItalicToken)
                                && toPair.Peek().CanOpen)
                                {
                                    var pair = toPair.Pop();

                                    if (withDigits)
                                    {
                                        pair.Token.Escape();
                                        b.Escape();
                                    }
                                    else
                                    {
                                        if (toPair.Count > 0
                                            && toPair.Peek().Token is ItalicToken
                                            && toPair.Peek().CanOpen)
                                        {
                                            pair.Token.Escape();
                                            b.Escape();
                                        }
                                        else
                                        {
                                            b.Close();
                                            isWord = false;
                                        }
                                    }
                                    isLastPaired = false;
                                }
                                else
                                {
                                    var newPair = new PairedTokenInfo(b);
                                    newPair.Close();
                                    toPair.Push(newPair);

                                    isWord = false;
                                    withDigits = false;
                                    isLastPaired = true;
                                }
                            }
                            else
                            {
                                isLastPaired = true;
                                var newPair = new PairedTokenInfo(b);
                                newPair.Open();
                                toPair.Push(newPair);
                            }
                            break;
                        }
                    default:
                        throw new ArgumentException("Unknown token received");
                }
            }

            foreach (var unpaired in toPair)
                unpaired.Token.Escape();

            return filteredTokens;
        }

        private IEnumerable<Token> FilterRawTokens(IEnumerable<Token> rawTokens)
        {
            var tokens = new LinkedList<Token>();
            var isEscaped = false;
            var isNewline = true;
            var isList = false;
            var isHeader = false;

            foreach (var token in rawTokens)
            {
                switch (token)
                {
                    case TextToken t:
                        if (isNewline && isList)
                        {
                            isList = false;
                            var listEnd = new UnorderedListToken();
                            listEnd.Close();
                            tokens.AddLast(listEnd);
                        }
                        isNewline = false;
                        if (isEscaped)
                        {
                            tokens.AddLast(new EscapeToken());
                            isEscaped = false;
                        }
                        tokens.AddLast(t);
                        break;
                    case NewlineToken nl:
                        if (isEscaped)
                        {
                            tokens.AddLast(new EscapeToken());
                            isEscaped = false;
                        }
                        if (isHeader)
                        {
                            var headerEnd = new HeaderToken();
                            headerEnd.Close();
                            tokens.AddLast(headerEnd);
                            isHeader = false;
                        }
                        if (isList)
                        {
                            var itemEnd = new ListItemToken();
                            itemEnd.Close();
                            tokens.AddLast(itemEnd);
                        }
                        else
                            tokens.AddLast(nl);
                        isNewline = true;
                        break;
                    case SpaceToken s:
                        if (isNewline && isList)
                        {
                            isList = false;
                            var listEnd = new UnorderedListToken();
                            listEnd.Close();
                            tokens.AddLast(listEnd);
                        }
                        isNewline = false;
                        if (isEscaped)
                        {
                            tokens.AddLast(new EscapeToken());
                            isEscaped = false;
                        }
                        tokens.AddLast(s);
                        break;
                    case EscapeToken e:
                        if (isNewline && isList)
                        {
                            isList = false;
                            var listEnd = new UnorderedListToken();
                            listEnd.Close();
                            tokens.AddLast(listEnd);
                        }
                        isNewline = false;
                        if (isEscaped)
                        {
                            tokens.AddLast(new EscapeToken());
                            isEscaped = false;
                        }
                        else
                            isEscaped = true;
                        break;
                    case HeaderToken h:
                        if (isNewline && isList)
                        {
                            isList = false;
                            var listEnd = new UnorderedListToken();
                            listEnd.Close();
                            tokens.AddLast(listEnd);
                        }

                        if (isEscaped)
                        {
                            h.Escape();
                            tokens.AddLast(h);
                            isEscaped = false;
                        }
                        else if (isNewline)
                        {
                            tokens.AddLast(h);
                            isHeader = true;
                        }
                        else
                        {
                            tokens.AddLast(new TextToken("#"));
                            tokens.AddLast(new SpaceToken());
                        }
                        isNewline = false;
                        break;
                    case ListItemToken li:
                        if (isEscaped)
                        {
                            li.Escape();
                            tokens.AddLast(li);
                            isEscaped = false;
                        }
                        else if (isNewline)
                        {
                            if (!isList)
                            {
                                tokens.AddLast(new UnorderedListToken());
                                isList = true;
                            }

                            tokens.AddLast(li);
                        }
                        else
                        {
                            tokens.AddLast(new TextToken("-"));
                            tokens.AddLast(new SpaceToken());
                        }
                        isNewline = false;
                        break;
                    case ItalicToken i:
                        if (isNewline && isList)
                        {
                            isList = false;
                            var listEnd = new UnorderedListToken();
                            listEnd.Close();
                            tokens.AddLast(listEnd);
                        }
                        isNewline = false;
                        if (isEscaped)
                        {
                            i.Escape();
                            tokens.AddLast(i);
                            isEscaped = false;
                        }
                        else if (tokens.Count > 0
                            && tokens.Last.Value is ItalicToken last)
                        {
                            var bold = new BoldToken();
                            if (last.IsEscaped)
                                bold.Escape();
                            tokens.RemoveLast();
                            tokens.AddLast(bold);
                        }
                        else
                            tokens.AddLast(i);
                        break;
                    default:
                        throw new ArgumentException("Unknown token received");
                }
            }

            if (isEscaped)
                tokens.AddLast(new EscapeToken());
            if (isHeader)
            {
                var headerEnd = new HeaderToken();
                headerEnd.Close();
                tokens.AddLast(headerEnd);
            }
            if (isList)
            {
                var itemEnd = new ListItemToken();
                itemEnd.Close();
                tokens.AddLast(itemEnd);
                var listEnd = new UnorderedListToken();
                listEnd.Close();
                tokens.AddLast(listEnd);
            }

            return tokens;
        }

        private IEnumerable<Token> ToRawTokens(string markdownText)
        {
            var builder = new StringBuilder();
            var isHeader = false;
            var isListItem = false;

            foreach (var ch in markdownText)
            {
                switch (ch)
                {
                    case '\\':
                        if (isHeader)
                        {
                            isHeader = false;
                            builder.Append('#');
                        }
                        if (isListItem)
                        {
                            isListItem = false;
                            builder.Append('-');
                        }
                        if (builder.Length > 0)
                        {
                            yield return new TextToken(builder.ToString());
                            builder.Clear();
                        }
                        yield return new EscapeToken();
                        break;
                    case '\n':
                        if (isHeader)
                        {
                            isHeader = false;
                            builder.Append('#');
                        }
                        if (isListItem)
                        {
                            isListItem = false;
                            builder.Append('-');
                        }
                        if (builder.Length > 0)
                        {
                            yield return new TextToken(builder.ToString());
                            builder.Clear();
                        }
                        yield return new NewlineToken();
                        break;
                    case ' ':
                        if (builder.Length > 0)
                        {
                            yield return new TextToken(builder.ToString());
                            builder.Clear();
                        }
                        if (isHeader)
                        {
                            yield return new HeaderToken();
                            isHeader = false;
                            break;
                        }
                        if (isListItem)
                        {
                            yield return new ListItemToken();
                            isListItem = false;
                            break;
                        }
                        yield return new SpaceToken();
                        break;
                    case '_':
                        if (isHeader)
                        {
                            isHeader = false;
                            builder.Append('#');
                        }
                        if (isListItem)
                        {
                            isListItem = false;
                            builder.Append('-');
                        }
                        if (builder.Length > 0)
                        {
                            yield return new TextToken(builder.ToString());
                            builder.Clear();
                        }
                        yield return new ItalicToken();
                        break;
                    case '#':
                        if (isHeader)
                            builder.Append('#');
                        isHeader = true;
                        break;
                    case '-':
                        if (isListItem)
                            builder.Append('-');
                        isListItem = true;
                        break;
                    default:
                        if (isHeader)
                        {
                            isHeader = false;
                            builder.Append('#');
                        }
                        if (isListItem)
                        {
                            isListItem = false;
                            builder.Append('-');
                        }
                        builder.Append(ch);
                        break;
                }
            }

            if (builder.Length > 0)
            {
                yield return new TextToken(builder.ToString());
                builder.Clear();
            }
        }
    }
}
