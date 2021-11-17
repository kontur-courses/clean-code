using System;

namespace MarkDown
{
    public class Md
    {
        public string Render(string input)
        {
            var rawTokenString = Tokenizer.GetRawTokenArrayFromString(input);
            var tokenString = Tokenizer.GetTokenArrayWithTypes(rawTokenString);
            return HtmlTagger.GetHtmlTaggedStringFromTokenString(tokenString);
        }
    }
}
