using System;

namespace Markdown.Tag.SpecificTags
{
    public class StyleTagData : TagData
    {
        public StyleTagData(TagBorder incomingBorder, TagBorder outgoingBorder, 
            params ITagData[] notAllowedNestedTags) 
            : base(incomingBorder, outgoingBorder, EndOfLineAction.Cancel, notAllowedNestedTags)
        { }
        

        public override bool IsValidAtOpen(string data, int startPos)
        {
            if (data.Length <= startPos + IncomingBorder.Open.Length)
                return false;
            var isBorderTouchDigit = IsBorderTouchDigit(data, IncomingBorder.Open, startPos);
            return !isBorderTouchDigit && data[startPos + IncomingBorder.Open.Length] != ' ';
        }

        public override bool IsValidAtClose(string data, int startPos, int endPos)
        {
            if (startPos + IncomingBorder.Open.Length >= endPos)
                return false;
            if (IsBorderTouchDigit(data, IncomingBorder.Close, endPos))
                return false;
            if (data[endPos - 1] == ' ')
                return false;
            if (!IsStartsWithWhitespace(data, startPos) || !IsEndsWithWhitespace(data, endPos))
                for (var i = startPos + IncomingBorder.Open.Length; i < endPos; i++)
                    if (Char.IsWhiteSpace(data[i]))
                        return false;
            return true;
        }

        private bool IsStartsWithWhitespace(string data, int startPos)
        {
            return startPos == 0 || Char.IsWhiteSpace(data[startPos - 1]);
        }
        
        private bool IsEndsWithWhitespace(string data, int endPos)
        {
            return endPos + IncomingBorder.Close.Length == data.Length 
                   || Char.IsWhiteSpace(data[endPos + IncomingBorder.Close.Length]);
        }

        private static bool IsBorderTouchDigit(string data, string border, int startPos)
        {
            var isBorderTouchDigit = false;
            if (startPos > 0)
                isBorderTouchDigit = isBorderTouchDigit || Char.IsDigit(data[startPos - 1]);
            if (startPos < data.Length - border.Length)
                isBorderTouchDigit = isBorderTouchDigit || Char.IsDigit(data[startPos + border.Length]);
            return isBorderTouchDigit;
        }
    }
}