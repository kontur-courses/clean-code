using System;

namespace Markdown
{
    class Tag
    {
        public string getTagString { get; } // символ по корторому определятся данный тег
        public bool isMultiple { get; } // состоит тег из одного символа или из нескольких подряд идущих
        public bool isPair { get; } // Парный тег или одиночный
        public bool isHearingCouples { get; } // Если True, то все парные теги до него считаются без пары, кроме его пары
        public string getOpenHtml { get; } // Соответствуюший ему открывающий тэг из html
        public string getCloseHtml { get; } // Соответствуюший ему закрывающий тэг из html

        public Tag(string tagString, bool multiple, bool pair, bool hearingCouples, string openHtml, string closeHtml)
        {
            getTagString = tagString;
            getOpenHtml = openHtml;
            getCloseHtml = closeHtml;
            isMultiple = multiple;
            isPair = pair;
            isHearingCouples = hearingCouples;
        }

        public bool PartiallyMatchTagAndToken(int index, int length, string text)
        {
            if (isMultiple)
                return PartiallyMatchMultipleTagAndToken(index, length, text);
            return PartiallyMatchNoMultipleTagAndToken(index, length, text);
        }

        public bool PartiallyMatchMultipleTagAndToken(int index, int length, string text)
        {
            return getTagString[getTagString.Length - 1] == text[index + length - 1];
        }

        public bool PartiallyMatchNoMultipleTagAndToken(int index, int length, string text)
        {
            if (length > getTagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (getTagString[i] != text[index + i])
                    return false;
            return true;
        }

        public bool MatchTagAndTokenCompletely(int index, int length, string text)
        {
            if (length != getTagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (getTagString[i] != text[index + i])
                    return false;
            return true;
        }
    }
}
