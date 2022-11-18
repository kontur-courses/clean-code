using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Parser
    {
        private int charCount;
        private Dictionary<string, Mod> symbols;
        private List<Token> allTokens;
        private Dictionary<Token, Mod> tokensWithModify;
        public string text;

        public Parser()
        {
            symbols = new Dictionary<string, Mod>()
            {
                [""] = Mod.Common,
                ["#"] = Mod.Title,
                ["__"] = Mod.Bold,
                ["_"] = Mod.Italic,
                ["\\"] = Mod.Slash
            };

            allTokens = new List<Token>();
            tokensWithModify = new Dictionary<Token, Mod>();
        }

        public string ParseMdToHTML(string markdownText)
        {
            text = markdownText;
            charCount = markdownText.Length;
            var concs = TikenizeText(markdownText);
            var htmlText = HtmlBuilder.ConvertConcsToHTML(concs);
            return htmlText;
        }

        private List<Token> TikenizeText(string text)
        {
            var tokens = new Stack<Token>();
            var curPos = 0;
            for (var i = 0; i < text.Length; i++)
            {
                var curSym = text[i];

                if (symbols.ContainsKey(curSym.ToString()))
                {
                    Token curToken;

                    if (i != curPos)
                    {
                        tokens.Push(new Token(curPos, i - 1, Mod.Common, false));
                    }

                    if (curPos + 1 != text.Length && text[i] == '_' && text[i + 1] == '_')
                    {
                        curToken = new Token(i, i + 1, Mod.Bold);
                        i++;
                    }
                    else if (text[i] == '\\')
                    {
                        curToken = new Token(i, i, Mod.Slash, false);
                    }
                    else
                    {
                        curToken = new Token(i, i, symbols[curSym.ToString()]);
                    }

                    tokens.TryPop(out var lastToken);
                    var redefTokens = RedefineTokens(lastToken, curToken, text);

                    foreach (var redefTok in redefTokens)
                    {
                        tokens.Push(redefTok);
                    }

                    curPos = i + 1;
                }
            }

            if (curPos < text.Length)
            {
                tokens.Push(new Token(curPos, text.Length, Mod.Common, false));
            }

            return tokens.Reverse().ToList();
        }

        private List<Token> RedefineTokens(Token last, Token current, string text)
        {
            var redefTokens = new List<Token>();

            switch (current.modType)
            {
                case Mod.Italic:
                    {
                        redefTokens = GetRedefTokensWithItalicOrBold(last, current);
                        return redefTokens;
                    }

                case Mod.Bold:
                    {
                        redefTokens = GetRedefTokensWithItalicOrBold(last, current);
                        return redefTokens;
                    }

                case Mod.Slash:
                    {
                        if (last == null)
                        {
                            redefTokens.Add(current);
                            return redefTokens;
                        }
                        else if(last.modType == Mod.Slash)
                        {
                            redefTokens.Add(new Token(current.startInd, current.endInd, Mod.Common, false));
                            return redefTokens;
                        }

                        redefTokens.Add(last);
                        redefTokens.Add(current);
                        return redefTokens;
                    }

                case Mod.Title:
                    {
                        if (last != null)
                        {
                            redefTokens.Add(current);
                        }
                        else if (last.modType == Mod.Slash)
                        {
                            redefTokens.Add(new Token(current.startInd, current.endInd, Mod.Common, false));
                        }
                        else
                        {
                            var endPos = current.startInd;

                            while (endPos != text.Length && text[endPos] != '\n')
                            {
                                endPos++;
                            }

                            var redTok = new Token(current.startInd, endPos, Mod.Title, false);

                            if (last != null)
                            {
                                redefTokens.Add(last);
                            }

                            redefTokens.Add(redTok);
                        }

                        return redefTokens;
                    }

                case Mod.Common:
                    {
                        redefTokens.Add(last);
                        redefTokens.Add(current);
                        return redefTokens;
                    }

                default:
                        break;
            }

            return redefTokens;
        }

        public List<Token> GetRedefTokensWithItalicOrBold(Token last, Token current)
        {
            var redefTokens = new List<Token>();

             if (last == null)
            {
                redefTokens.Add(current);
            }
            else if (last.modType == Mod.Slash)
            {
                redefTokens.Add(new Token(current.startInd, current.endInd, Mod.Common, false));
            }
            else
            {
                redefTokens.Add(last);
                redefTokens.Add(current);
            }

            return redefTokens;
        }
    }
}
