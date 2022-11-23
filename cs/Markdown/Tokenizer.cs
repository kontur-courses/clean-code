using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public class Tokenizer
    {
        private Dictionary<string, Mod> symbols;
        private string text;
        private Stack<Token> tokens;

        public Tokenizer(Dictionary<string, Mod> separatorSymbols)
        {
            symbols = separatorSymbols;
        }

        public List<Token> TikenizeText(string mdText)
        {
            text = mdText;
            tokens = new Stack<Token>();
            var curPos = 0;
            for (var i = 0; i < text.Length; i++)
            {
                var curSym = text[i];

                if (symbols.ContainsKey(curSym.ToString()))
                {
                    Token curToken;

                    if (i != curPos) AddCommonToken(curPos, i - 1);

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

                    RedefineStack(tokens, curToken);
                    curPos = i + 1;
                }
            }

            if (curPos < text.Length) AddCommonToken(curPos, text.Length);

            return tokens.Reverse().ToList();
        }

        private void AddCommonToken(int startPos, int endPos)
        {
            tokens.Push(new Token(startPos, endPos, Mod.Common, false));
        }

        private void RedefineStack(Stack<Token> tokens, Token curToken)
        {
            tokens.TryPop(out var lastToken);
            var redefTokens = RedefineTokens(lastToken, curToken, text);

            foreach (var redefTok in redefTokens)
            {
                tokens.Push(redefTok);
            }
        }

        private List<Token> RedefineTokens(Token last, Token current, string text)
        {
            var redefTokens = new List<Token>();

            return current.modType switch
            {
                Mod.Italic => GetRedefTokensWithItalicOrBold(last, current),
                Mod.Bold => GetRedefTokensWithItalicOrBold(last, current),
                Mod.Slash => GetRedefTokensWithSlash(last, current),
                Mod.Title => GetRedefTokensWithTitle(last, current, text),
                Mod.Common => GetRedefTokensWithCommon(last, current),
                _ => new List<Token>()
            };
        }

        private List<Token> GetRedefTokensWithCommon(Token last, Token current)
        {
            var redefTokens = new List<Token>();

            redefTokens.Add(last);
            redefTokens.Add(current);

            return redefTokens;
        }

        private List<Token> GetRedefTokensWithTitle(Token last, Token current, string text)
        {
            var redefTokens = new List<Token>();
            var endPos = current.StartInd;

            while (endPos != text.Length && text[endPos] != '\n')
            {
                endPos++;
            }

            if (last == null)
            {
                redefTokens.Add(new Token(current.StartInd, endPos, Mod.Title));
            }
            else if (last.modType == Mod.Slash)
            {
                redefTokens.Add(new Token(current.StartInd, current.EndInd, Mod.Common));
            }
            else
            {
                var redTok = new Token(current.StartInd, endPos, Mod.Title);

                if (last != null)
                {
                    redefTokens.Add(last);
                }

                redefTokens.Add(redTok);
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
                redefTokens.Add(new Token(current.StartInd, current.EndInd, Mod.Common, false));
            }
            else
            {
                redefTokens.Add(last);
                redefTokens.Add(current);
            }

            return redefTokens;
        }

        private List<Token> GetRedefTokensWithSlash(Token last, Token current)
        {
            var redefTokens = new List<Token>();

            if (last == null)
            {
                redefTokens.Add(current);
                return redefTokens;
            }
            else if (last.modType == Mod.Slash)
            {
                redefTokens.Add(new Token(current.StartInd, current.EndInd, Mod.Common, false));
                return redefTokens;
            }

            redefTokens.Add(last);
            redefTokens.Add(current);
            return redefTokens;
        }
    }
}
