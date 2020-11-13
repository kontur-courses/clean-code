using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Md
    {
        public string Render(string input)
        {
            //SplitToTokens(string line)
            //ConvertTokensToString(List<Token> tokens)
            throw new NotImplementedException();
        }

        private List<Token> SplitToTokens(string line)
        {
            //SkipWhiteSpaces(string line, int startPos)
            //ReadToken(string line, int startPos)
            throw new NotImplementedException();
        }

        private int SkipWhiteSpaces(string line, int startPos)
        {
            //ReadToken(string line, int startPos)
            throw new NotImplementedException();
        }

        private Token ReadToken(string line, int startPos)
        {
            //ReadSimpleToken(string line, int startPos)
            //ReadStrongToken(string line, int startPos)
            //ReadEmToken(string line, int startPos)
            //ReadHeaderToken(string line, int startPos)
            throw new NotImplementedException();
        }

        private Token ReadSimpleToken(string line, int startPos)
        {
            throw new NotImplementedException();
        }

        private Token ReadStrongToken(string line, int startPos)
        {
            throw new NotImplementedException();
        }

        private Token ReadEmToken(string line, int startPos)
        {
            throw new NotImplementedException();
        }

        private Token ReadHeaderToken(string line, int startPos)
        {
            throw new NotImplementedException();
        }

        private string ConvertTokensToString(List<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
