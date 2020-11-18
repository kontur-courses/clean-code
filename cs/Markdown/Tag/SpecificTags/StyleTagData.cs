using System;

namespace Markdown.Tag.SpecificTags
{
    public class StyleTagData : TagData
    {
        public override bool IsBreaksWhenNestedNotComplete => true;

        private Func<string, int, int, bool> additionalValidationCheck;

        public StyleTagData(TagBorder incomingBorder, TagBorder outgoingBorder,
            Func<string, int, int, bool> additionalValidationCheck = null,
            params ITagData[] notAllowedNestedTags)
            : base(incomingBorder, outgoingBorder, EndOfLineAction.Cancel, 
                notAllowedNestedTags:notAllowedNestedTags)
        {
            this.additionalValidationCheck = additionalValidationCheck;
        }
        
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
            if (IsLetterBeforeTag(data, startPos) || IsLetterAfterTag(data, endPos))
                for (var i = startPos + IncomingBorder.Open.Length; i < endPos; i++)
                    if (Char.IsWhiteSpace(data[i]))
                        return false;
            if (additionalValidationCheck != null)
                return additionalValidationCheck(data, startPos, endPos);
            return true;
        }

        private bool IsLetterBeforeTag(string data, int startPos)
        {
            return startPos != 0 && Char.IsLetter(data[startPos - 1]);
        }
        
        private bool IsLetterAfterTag(string data, int endPos)
        {
            return endPos + IncomingBorder.Close.Length < data.Length 
                   && Char.IsLetter(data[endPos + IncomingBorder.Close.Length]);
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