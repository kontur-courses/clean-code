using System.Collections.Generic;

namespace MarkdownProcessing
{
    public enum TokenType
    {
        PlainText,
        Bold,
        Italic,
        HeadLine1
        //.....
    }
    public class Token
    {
        public static Dictionary<string, TokenType> TokensDictionary;
        public Token ParentToken;
        public string InnerText;
        public TokenType TokenType;
    }
}