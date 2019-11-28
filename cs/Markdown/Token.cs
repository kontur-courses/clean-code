using System;

namespace Markdown
{
    public class Token
    {
        public TextType tokenTextType;
        public string innerText;
        public int startTagPosition;
        public int closeTagPosition;        
        public Tuple<TextType, int> outerToken;

        public Token(int _startTagPosition, int _closeTagPosition, string text, TextType type, Tuple<TextType, int> _outerToken)
        {
            startTagPosition = _startTagPosition;
            closeTagPosition = _closeTagPosition;
            tokenTextType = type;
            var shift = 1;
            if (tokenTextType == TextType.Strong)
                shift = 2;
            innerText = text.Substring(startTagPosition, closeTagPosition - startTagPosition + shift);
            outerToken = _outerToken;                     
        }                
    }
}
