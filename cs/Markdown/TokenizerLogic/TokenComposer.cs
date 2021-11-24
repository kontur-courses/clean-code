using System;
using System.Collections.Generic;

namespace Markdown.TokenizerLogic
{
    public class TokenComposer
    {
        private IEnumerable<Token> tokens;
        private LinkedList<Token> composed;
        private bool isEscaped;
        private bool isNewline;
        private bool isList;
        private bool isHeader;

        private TokenComposer(IEnumerable<Token> rawTokens)
        {
            tokens = rawTokens;
            composed = new LinkedList<Token>();
            isNewline = true;
        }

        public static IEnumerable<Token> FilterRawTokens(IEnumerable<Token> rawTokens)
        {
            var filter = new TokenComposer(rawTokens);
            filter.Apply();
            return filter.composed;
        }

        private void Apply()
        {
            foreach (var token in tokens)
                HandleToken(token);

            HandleFailedEscape();
            HandleHeaderClosing();
            HandleTextEndListClosing();
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
            HandleFailedEscape();
            HandleHeaderClosing();
            if (isList)
            {
                var itemEnd = new ListItemToken();
                itemEnd.Close();
                composed.AddLast(itemEnd);
            }
            else
                composed.AddLast(nl);
            isNewline = true;
        }

        private void HandleEscapeToken()
        {
            HandleListClosing();
            isNewline = false;
            if (!HandleFailedEscape())
                isEscaped = true;
        }

        private void HandleHeaderToken(HeaderToken h)
        {
            HandleListClosing();
            if (isEscaped)
                HandleEscapedToken(h);
            else if (isNewline)
            {
                composed.AddLast(h);
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
                    composed.AddLast(new UnorderedListToken());
                    isList = true;
                }
                composed.AddLast(li);
            }
            else
                HandleFailedToken('-');
            isNewline = false;
        }

        private void HandleItalicToken(ItalicToken i)
        {
            HandleListClosing();
            isNewline = false;
            if (isEscaped)
                HandleEscapedToken(i);
            else if (composed.Count > 0
                && composed.Last.Value is ItalicToken last)
                AddBoldToken(last);
            else
                composed.AddLast(i);
        }

        private void HandleSimpleToken(SingleToken token)
        {
            HandleListClosing();
            isNewline = false;
            HandleFailedEscape();
            composed.AddLast(token);
        }

        private void AddBoldToken(ItalicToken last)
        {
            var bold = new BoldToken();
            if (last.IsEscaped)
                bold.Escape();
            composed.RemoveLast();
            composed.AddLast(bold);
        }

        private void HandleFailedToken(char failedChar)
        {
            composed.AddLast(new TextToken(failedChar.ToString()));
            composed.AddLast(new SpaceToken());
        }

        private void HandleEscapedToken(PairedToken token)
        {
            token.Escape();
            composed.AddLast(token);
            isEscaped = false;
        }

        private void HandleHeaderClosing()
        {
            if (isHeader)
            {
                var headerEnd = new HeaderToken();
                headerEnd.Close();
                composed.AddLast(headerEnd);
                isHeader = false;
            }
        }

        private bool HandleFailedEscape()
        {
            if (isEscaped)
            {
                composed.AddLast(new EscapeToken());
                isEscaped = false;
                return true;
            }
            return false;
        }

        private void HandleTextEndListClosing()
        {
            if (isList)
            {
                var itemEnd = new ListItemToken();
                itemEnd.Close();
                composed.AddLast(itemEnd);
                isNewline = true;
                HandleListClosing();
            }
        }

        private void HandleListClosing()
        {
            if (isNewline && isList)
            {
                isList = false;
                var listEnd = new UnorderedListToken();
                listEnd.Close();
                composed.AddLast(listEnd);
            }
        }
    }
}
