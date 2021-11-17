using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly IMdSpecification _specification;

        public MarkdownParser(IMdSpecification specification)
        {
            _specification = specification;
        }

        public List<Token> ParseToTokens(string mdText)
        {
            throw new NotImplementedException();
        }

        private int FindPairTagIndex(string mdText, int openInd)
        {
            throw new NotImplementedException();
        }        
    }
}
