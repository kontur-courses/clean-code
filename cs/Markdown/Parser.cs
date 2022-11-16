using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private int charCount;
        private int curPos;
        private Dictionary<ConcType, int> lettersQuantity;
        private List<Token> allTokens;
        private Dictionary<Token, ConcType> tokensWithModify;
        public string text;

        public Parser()
        {
            lettersQuantity = new Dictionary<ConcType, int>()
            {
                [ConcType.Main] = 0,
                [ConcType.Title] = 1,
                [ConcType.Bold] = 2,
                [ConcType.Italic] = 1
            };

            allTokens = new List<Token>();
            tokensWithModify = new Dictionary<Token, ConcType>();
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
                var token = NextToken();
                tokensWithModify.Add(token, ConcType.Main);

                if (token is ModifierToken mergeToken)
                {
                    var lastModifierToken = openConcs.Peek();

                    if (lastModifierToken.concType == mergeToken.type)
                    {
                        var curConc = openConcs.Pop();
                        curConc.tokens.Add(token);
                        curConc.IsClosed = true;

                        var parentConc = openConcs.Peek();
                        parentConc.innerConcs.Add(curConc);

                        foreach (var tok in curConc.tokens)
                        {
                            tokensWithModify[tok] = mergeToken.type;
                        }
                    }
                    else
                    {
                        var newConc = new Concatination(mergeToken.type, curPos);
                        newConc.tokens.Add(token);
                        openConcs.Push(newConc);
                    }

                }
                else if (token is CommonToken)
                {
                    var typeOfToken = openConcs.Peek().concType;
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
                        if (ModifierInWord() && 
                            WordHaveSameMod() && 
                            curPos + 1 != charCount && text[curPos + 1] == '_')
                        {
                            curToken = new ModifierToken(ConcType.Bold, curPos);
                            MovePosition(2);
                            return curToken;
                        }
                        else if (ModifierInWord() && !WordHaveSameMod())
                        {
                            break;
                        }

                        curToken = new ModifierToken(ConcType.Italic, curPos);
                        MovePosition(1);
                        return curToken;
                    }

                case '#':
                    {
                        curToken = new ModifierToken(ConcType.Title, curPos);
                        MovePosition(1);
                        return curToken;
                    }

                default:
                        break;
            }

            string tokenText = string.Empty;

            while (curPos != charCount && this.text[curPos] != '_' && text[curPos] != '#')
            {
                tokenText += text[curPos];
                curPos++;
            }

            curToken = new CommonToken(tokenText, curPos - tokenText.Length);

            return curToken;
        }

        private bool ModifierInWord()
        {
            return (text[curPos + 1] != ' ' && text[curPos + 2] != ' ');
        }

        private bool WordHaveSameMod()
        {
            var index = 1;
            var currentChar = text[curPos];

            while(curPos + index != charCount && currentChar != ' ')
            {
                currentChar = text[curPos + index];

                if (currentChar == text[curPos])
                {
                    return true;
                }

                index++;
            }

            return false;
        }

        private void MovePosition(int steps)
        {
            curPos += steps;
        }
    }
}
