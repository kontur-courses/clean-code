using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        private Dictionary<string, Mod> symbols;

        public Tokenizer(Dictionary<string, Mod> separatorSymbols)
        {
            symbols = separatorSymbols;
        }

        public List<Token> TikenizeText(string text)
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

                    if (i + 1 != text.Length && text[i] == '_' && text[i + 1] == '_')
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
                        var endPos = current.startInd;

                        while (endPos != text.Length && text[endPos] != '\n')
                        {
                            endPos++;
                        }

                        if (last == null)
                        {
                            redefTokens.Add(new Token(current.startInd, endPos, Mod.Title));
                        }
                        else if (last.modType == Mod.Slash)
                        {
                            redefTokens.Add(new Token(current.startInd, current.endInd, Mod.Common));
                        }
                        else
                        {
                            var redTok = new Token(current.startInd, endPos, Mod.Title);

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
            }

            return redefTokens;
        }

        private List<Token> GetRedefTokensWithItalicOrBold(Token last, Token current)
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
