using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Tag
    {
        public string getTagString { get; } // символ по корторому определятся данный тег
        public bool isMultiple { get; } // состоит тег из одного символа или из нескольких подряд идущих
        public bool isPair { get; } // Парный тег или одиночный
        public bool isHearingCouples { get; } // Если True, то все парные теги до него считаются без пары, кроме его пары
        public string getHtml { get; } // Соответствуюший ему тэг из html

        public Tag(string tagString, string html, bool multiple, bool pair, bool hearingCouples)
        {
            getTagString = tagString;
            getHtml = html;
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
