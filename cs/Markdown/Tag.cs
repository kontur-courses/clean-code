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

        public bool isPairedWith(Tag tag) // Если тег парный, то проверяет является ли 2-й тег парой для текущего
        {
            throw new NotImplementedException();
        }
    }
}
