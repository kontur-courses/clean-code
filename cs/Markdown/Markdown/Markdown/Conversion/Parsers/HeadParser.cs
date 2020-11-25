using System;
using System.Linq;
using System.Text;

namespace Markdown.Conversion.Parsers
{
    public class HeadParser : IMarkParser
    {
        private Mark mark;
        private ParseHelper helper;

        public TokenMd GetToken(string text, int index, out int finalIndex)
        {
            helper = new ParseHelper();
            mark = new HeadMark();
            var builder = new StringBuilder();
            finalIndex = index;
            
            builder = helper.AppendMarkSymbols(builder, text, finalIndex, out finalIndex, mark.DefiningSymbol);
            
            while (!helper.IsSymbols(finalIndex, text, Environment.NewLine))
            {
                if(finalIndex>=text.Length || helper.IsSymbols(finalIndex, text, mark.AllSymbols.Last()))
                    return new TokenMd(builder.ToString(), mark);
                builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
            }

            return new TokenMd(builder.ToString(), mark);
        }
    }
}