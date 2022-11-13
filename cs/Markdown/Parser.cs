using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        List<Token> curComTokens = new List<Token>();
        Stack<Concatination> openConcs;
        Token curToken;

        private int charCount;
        private int curPos;

        string text;

        public Parser(string markdownText)
        {
            text = markdownText;
            charCount = markdownText.Length;
            openConcs = new Stack<Concatination>();
            openConcs.Push(new Concatination(ConcType.main));
        }

        public Stack<Concatination> Parse()
        {
            GrabConcat();
            return openConcs;
        }

        void GrabConcat()
        {
            while (curPos != charCount)
            {
                NextToken();
                var token = curToken;

                if (token is ModifierToken)
                {
                    var lastModifierToken = openConcs.Peek();
                    var tok = (ModifierToken)token;

                    if(lastModifierToken.concType == tok.type)
                    {
                        var curConc = openConcs.Pop();
                        var parentConc = openConcs.Peek();
                        parentConc.innerConcs.Add(curConc);
                    }
                    else
                    {
                        var newModifier = new Concatination(tok.type);
                        openConcs.Push(newModifier);
                    }
                }
                else if (token is CommonToken)
                {
                    var currentConc = openConcs.Peek();
                    currentConc.AddTokens(token);
                }
            }
        }

        void NextToken()
        {
            if (this.text[curPos] == '_' && 
                this.text[curPos + 1] == '_')
            {
                curToken = new ModifierToken(ConcType.bold);
                MovePosition(2);
                return;
            }

            if (this.text[curPos] == '_')
            {
                curToken = new ModifierToken(ConcType.italic);
                MovePosition(1);
                return;
            }

            if (this.text[curPos] == '#')
            {
                curToken = new ModifierToken(ConcType.title);
                MovePosition(1);
                return;
            }

            string tokenText = string.Empty;

            while (curPos != charCount && this.text[curPos] != '_' && this.text[curPos] != '#')
            {
                tokenText += this.text[curPos];
                curPos++;
            }

            curToken = new CommonToken(tokenText, curPos - tokenText.Length);
        }

        void MovePosition(int steps)
        {
            curPos += steps;
        }
    }

}
