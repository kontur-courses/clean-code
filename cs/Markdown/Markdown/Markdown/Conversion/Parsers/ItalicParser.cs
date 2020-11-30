using System;
using System.Text;
using Markdown.Conversion.Parsers;

namespace Markdown.Parsers
{
    public class ItalicParser : IMarkParser
    {
        private Mark mark;
        private ParseHelper helper;
        private TokenMd resultToken;

        public TokenMd GetToken(string text, int index, out int finalIndex)
        {
            var isEnd = false;
            helper = new ParseHelper();
            mark = new ItalicMark();
            var builder = new StringBuilder();
            finalIndex = index;

            builder = helper.AppendMarkSymbols(builder, text, finalIndex, out finalIndex, mark.DefiningSymbol);

            while (finalIndex < text.Length && !helper.IsSymbols(finalIndex, text, Environment.NewLine)&& !isEnd)
            {
                isEnd = text[finalIndex] == '_'
                        && text[finalIndex - 1] != '_'
                        && (finalIndex + 1 >= text.Length 
                            || finalIndex + 1 < text.Length
                            && text[finalIndex + 1] != '_'
                            && !char.IsWhiteSpace(text[finalIndex - 1]));

                builder = helper.AppendSymbol(builder, text, finalIndex, out finalIndex);
            }
            
            return GetResultToken(builder.ToString(), isEnd);
        }


        private TokenMd GetResultToken(string tokenText, bool isEnd)
        {
            if (!isEnd)
            {
                resultToken = new TokenMd(tokenText, new EmptyMark());
                return resultToken;
            }

            resultToken = new TokenMd(tokenText, mark);

            return resultToken.TokenWithoutMark.Length == 0 
                ? new TokenMd("__", new EmptyMark()) 
                : resultToken;
        }
    
    }
}