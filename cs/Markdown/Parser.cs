using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private int charCount;
        private int curPos;
        private Dictionary<ConcType, string> lettersQuantity;
        private List<Token> allTokens;
        private Dictionary<Token, ConcType> tokensWithModify;
        public string text;

        public Parser()
        {
            lettersQuantity = new Dictionary<ConcType, string>()
            {
                [ConcType.Title] = "#",
                [ConcType.Bold] = "__",
                [ConcType.Italic] = "_"
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
            var ch = text[curPos];

            switch (ch)
            {
                case '_':
                    {
                        if (curPos + 1 != charCount && text[curPos + 1] == ch)
                        {
                            if ((WordStartsWithModifier() || ModifierInWord(ConcType.Bold)) &&
                            WordHaveSameMod(ConcType.Bold))
                            {
                                curToken = new ModifierToken(ConcType.Bold, curPos);
                                MovePosition(2);
                                return curToken;
                            }
                            else if (WordStartsWithModifier() && !WordHaveSameMod(ConcType.Bold))
                            {
                                curToken = new ModifierToken(ConcType.Bold, curPos);
                                MovePosition(2);
                                return curToken;
                            }
                            if (ModifierInWord(ConcType.Bold) && !WordHaveSameMod(ConcType.Bold))
                            {
                                break;
                            }
                        }
                        else
                        {
                            if ((WordStartsWithModifier() || ModifierInWord(ConcType.Italic)) &&
                            WordHaveSameMod(ConcType.Italic))
                            {
                                curToken = new ModifierToken(ConcType.Italic, curPos);
                                MovePosition(1);
                                return curToken;
                            }
                            else if (WordStartsWithModifier() && !WordHaveSameMod(ConcType.Italic))
                            {
                                curToken = new ModifierToken(ConcType.Italic, curPos);
                                MovePosition(1);
                                return curToken;
                            }
                            if (ModifierInWord(ConcType.Italic) && !WordHaveSameMod(ConcType.Italic))
                            {
                                break;
                            }
                        }

                        break;
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
        private bool WordStartsWithModifier()
        {
            return (curPos == 0 || text[curPos - 1] == ' ');
        }

        private bool ModifierInWord(ConcType type)
        {
            if (WordStartsWithModifier())
            {
                return false;
            }

            if (type is ConcType.Bold)
            {
                return (text[curPos + 2] != ' ');
            }

            return (text[curPos + 1] != ' ');
        }

        private bool WordHaveSameMod(ConcType type)
        {
            var currentChar = text[curPos];
            var modLetters = lettersQuantity[type];
            var startIndex = curPos;
            var endIndex = curPos;

            while (startIndex != 0 && text[startIndex] != ' ')
            {
                startIndex--;
            }

            while (text[endIndex] != ' ')
            {
                endIndex++;
            }

            var word = text.Substring(startIndex, endIndex - startIndex);

            var inCount = word.Split(modLetters);
            return (inCount.Length > 2);
        }

        private void MovePosition(int steps)
        {
            curPos += steps;
        }
    }
}
