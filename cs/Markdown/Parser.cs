using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private int charCount;
        private int curPos;

        public string text;

        public Parser()
        {
        }

        public string ParseMdToHTML(string markdownText)
        {
            text = markdownText;
            charCount = markdownText.Length;
            var concs = GrabConcat();
            var htmlText = HtmlBuilder.ConvertConcsToHTML(concs);
            return htmlText;
        }

        private Stack<Concatination> GrabConcat()
        {
            curPos = 0;
            var openConcs = new Stack<Concatination>();
            openConcs.Push(new Concatination(ConcType.Main, curPos));

            while (curPos != charCount)
            {
                var token = NextToken(); ;

                if (token is ModifierToken mergeToken)
                {
                    var lastModifierToken = openConcs.Peek();

                    if (lastModifierToken.concType == mergeToken.type)
                    {
                        var curConc = openConcs.Pop();
                        var parentConc = openConcs.Peek();
                        parentConc.innerConcs.Add(curConc);
                    }
                    else
                    {
                        var newModifier = new Concatination(mergeToken.type, curPos);
                        openConcs.Push(newModifier);
                    }
                }
                else if (token is CommonToken)
                {
                    var currentConc = openConcs.Peek();
                    currentConc.AddTokens(token);
                }
            }

            return openConcs;
        }

        private Token NextToken()
        {
            Token curToken;

            switch (text[curPos])
            {
                case '_':
                    {
                        if (text[curPos + 1] == '_')
                        {
                            curToken = new ModifierToken(ConcType.Bold);
                            MovePosition(2);
                            return curToken;
                        }
                        curToken = new ModifierToken(ConcType.Italic);
                        MovePosition(1);
                        return curToken;
                    }

                case '#':
                    {
                        curToken = new ModifierToken(ConcType.Title);
                        MovePosition(1);
                        return curToken;
                    }

                default:
                    {
                        string tokenText = string.Empty;

                        while (curPos != charCount && this.text[curPos] != '_' && text[curPos] != '#')
                        {
                            tokenText += text[curPos];
                            curPos++;
                        }

                        curToken = new CommonToken(tokenText, curPos - tokenText.Length);
                        break;
                    }
            }

            return curToken;
        }

        private void MovePosition(int steps)
        {
            curPos += steps;
        }
    }
}
