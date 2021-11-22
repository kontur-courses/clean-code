using System;
using System.Collections.Generic;

namespace Markdown.TokenizerLogic
{
    public class TokenFilter
    {
        private IEnumerable<Token> toFilter;
        private LinkedList<Token> filtered;
        private bool isEscaped;
        private bool isNewline;
        private bool isList;
        private bool isHeader;

        private TokenFilter(IEnumerable<Token> rawTokens)
        {
            toFilter = rawTokens;
            filtered = new LinkedList<Token>();
            isNewline = true;
        }

        public static IEnumerable<Token> FilterRawTokens(IEnumerable<Token> rawTokens)
        {
            var filter = new TokenFilter(rawTokens);
            filter.Apply();
            return filter.filtered;
        }

        private void Apply()
        {
            foreach (var token in toFilter)
                HandleToken(token);

            TryFailedEscape();
            TryCloseHeader();
            TryCloseEndList();
        }

        private void HandleToken(Token token)
        {
            switch (token)
            {
                case EscapeToken:
                    HandleEscapeToken();
                    break;
                case SingleToken s:
                    HandleSimpleToken(s);
                    break;
                case NewlineToken nl:
                    HandleNewlineToken(nl);
                    break;
                case HeaderToken h:
                    HandleHeaderToken(h);
                    break;
                case ListItemToken li:
                    HandleListItemToken(li);
                    break;
                case ItalicToken em:
                    HandleItalicToken(em);
                    break;
                default:
                    throw new ArgumentException("Unknown token received");
            }
        }

        private void HandleNewlineToken(NewlineToken nl)
        {
            TryFailedEscape();
            TryCloseHeader();
            if (isList)
            {
                var itemEnd = new ListItemToken();
                itemEnd.Close();
                filtered.AddLast(itemEnd);
            }
            else
                filtered.AddLast(nl);
            isNewline = true;
        }

        private void HandleEscapeToken()
        {
            TryCloseList();
            isNewline = false;
            if (!TryFailedEscape())
                isEscaped = true;
        }

        private void HandleHeaderToken(HeaderToken h)
        {
            TryCloseList();
            if (isEscaped)
                HandleEscapedToken(h);
            else if (isNewline)
            {
                filtered.AddLast(h);
                isHeader = true;
            }
            else
                HandleFailedToken('#');
            isNewline = false;
        }

        private void HandleListItemToken(ListItemToken li)
        {
            if (isEscaped)
                HandleEscapedToken(li);
            else if (isNewline)
            {
                if (!isList)
                {
                    filtered.AddLast(new UnorderedListToken());
                    isList = true;
                }
                filtered.AddLast(li);
            }
            else
                HandleFailedToken('-');
            isNewline = false;
        }

        private void HandleItalicToken(ItalicToken i)
        {
            TryCloseList();
            isNewline = false;
            if (isEscaped)
                HandleEscapedToken(i);
            else if (filtered.Count > 0
                && filtered.Last.Value is ItalicToken last)
                AddBoldToken(last);
            else
                filtered.AddLast(i);
        }

        private void HandleSimpleToken(SingleToken token)
        {
            TryCloseList();
            isNewline = false;
            TryFailedEscape();
            filtered.AddLast(token);
        }

        private void AddBoldToken(ItalicToken last)
        {
            var bold = new BoldToken();
            if (last.IsEscaped)
                bold.Escape();
            filtered.RemoveLast();
            filtered.AddLast(bold);
        }

        private void HandleFailedToken(char failedChar)
        {
            filtered.AddLast(new TextToken(failedChar.ToString()));
            filtered.AddLast(new SpaceToken());
        }

        private void HandleEscapedToken(PairedToken token)
        {
            token.Escape();
            filtered.AddLast(token);
            isEscaped = false;
        }

        private void TryCloseHeader()
        {
            if (isHeader)
            {
                var headerEnd = new HeaderToken();
                headerEnd.Close();
                filtered.AddLast(headerEnd);
                isHeader = false;
            }
        }

        private bool TryFailedEscape()
        {
            if (isEscaped)
            {
                filtered.AddLast(new EscapeToken());
                isEscaped = false;
                return true;
            }
            return false;
        }

        private void TryCloseEndList()
        {
            if (isList)
            {
                var itemEnd = new ListItemToken();
                itemEnd.Close();
                filtered.AddLast(itemEnd);
                isNewline = true;
                TryCloseList();
            }
        }

        private void TryCloseList()
        {
            if (isNewline && isList)
            {
                isList = false;
                var listEnd = new UnorderedListToken();
                listEnd.Close();
                filtered.AddLast(listEnd);
            }
        }
    }
}
