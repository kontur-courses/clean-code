using System;
using System.Linq;
using System.Text;

namespace Markdown.Conversion.Parsers
{
    public class StrongParser : IMarkParser
    {
        private Mark mark;
        private ParseHelper helper;
        private TokenMd resultToken;

        public TokenMd GetToken(string text, int index, out int finalIndex)
        {
            helper = new ParseHelper();
            mark = new StrongMark();
            var builder = new StringBuilder();
            finalIndex = index;
            
            builder = helper.AppendMarkSymbols(builder, text, finalIndex, out finalIndex, mark.DefiningSymbol);

            while (!helper.IsSymbols(finalIndex, text, Environment.NewLine))
            {
                var count = 0;
                
                if (helper.IsSymbols(finalIndex, text, mark.AllSymbols.Last()))
                {
                    builder = helper.AppendMarkSymbols(builder, text, finalIndex, out finalIndex, mark.AllSymbols.Last());
                    return GetResultToken(builder.ToString());
                }
              
                if(finalIndex>=text.Length)
                    return new TokenMd(builder.ToString(), null);
                
                builder.Append(text[finalIndex]);
                finalIndex++;
            }

            return GetResultToken(builder.ToString());
        }
        
        private TokenMd GetResultToken(string tokenText)
        {
            resultToken = new TokenMd(tokenText, mark);
            if(resultToken.TokenWithoutMark.Length==0)
                return new TokenMd("____", null);
            
            return resultToken;
        }
        
    }
}