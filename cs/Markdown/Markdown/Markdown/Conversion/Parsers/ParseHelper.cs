using System;
using System.Text;

namespace Markdown.Conversion.Parsers
{
    public class ParseHelper
    {
        public bool IsSymbols(int index, string text, string symbols)
        {
            var count = 0;
            if (index + symbols.Length > text.Length) 
                return false;
            for (var i = 0; i < symbols.Length; i++)
            {
                if (text[index +i] == symbols[i])
                    count++;

                if (count == symbols.Length)
                    return true;
            }

            return false;
        }
        
        public StringBuilder AppendSymbol(StringBuilder builder, string text, int index, out int finalIndex)
        {
            var resultBuilder = builder;
            finalIndex = index;
            resultBuilder.Append(text[index]);
            finalIndex++;
                
            return resultBuilder;
        }
        
        public StringBuilder AppendMarkSymbols(StringBuilder builder, string text, int index, out int finalIndex,string markSymbol)
        {
            var resultBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            finalIndex = index;
            for (var i = 0; i < markSymbol.Length; i++)
            {
                resultBuilder.Append(text[index]);
                finalIndex++;
            }

            return resultBuilder;
        }
    }
}