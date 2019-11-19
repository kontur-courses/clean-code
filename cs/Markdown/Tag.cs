using System;

namespace Markdown
{
    class Tag
    {
        public string TagString { get; } // символ по корторому определятся данный тег
        public bool IsMultiple { get; } // состоит тег из одного символа или из нескольких подряд идущих
        public bool IsPair { get; } // Парный тег или одиночный
        public bool IsHearingCouples { get; } // Если True, то все парные теги до него считаются без пары, кроме его пары
        public string OpenHtml { get; } // Соответствуюший ему открывающий тэг из html
        public string CloseHtml { get; } // Соответствуюший ему закрывающий тэг из html

        public Tag(string tagString, bool multiple, bool pair, bool hearingCouples, string openHtml, string closeHtml)
        {
            TagString = tagString;
            OpenHtml = openHtml;
            CloseHtml = closeHtml;
            IsMultiple = multiple;
            IsPair = pair;
            IsHearingCouples = hearingCouples;
        }

        public bool PartiallyMatchTagAndToken(int index, int length, string text)
        {
            if (IsMultiple)
                return PartiallyMatchMultipleTagAndToken(index, length, text);
            return PartiallyMatchNoMultipleTagAndToken(index, length, text);
        }

        public bool PartiallyMatchMultipleTagAndToken(int index, int length, string text)
        {
            return TagString[TagString.Length - 1] == text[index + length - 1];
        }

        public bool PartiallyMatchNoMultipleTagAndToken(int index, int length, string text)
        {
            if (length > TagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (TagString[i] != text[index + i])
                    return false;
            return true;
        }

        public bool MatchTagAndTokenCompletely(int index, int length, string text)
        {
            if (length != TagString.Length)
                return false;
            for (var i = 0; i < length; i++)
                if (TagString[i] != text[index + i])
                    return false;
            return true;
        }
    }
}
