﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagH1 : TagConverterBase
    {
        public override bool IsSingleTag => true;
        public override string Html => TagHtml.h1;

        public override string Md => TagMd.sharp;

        public override HashSet<string> TagInside => TagsAssociation.tags;
        public override bool IsTag(string text, int pos) => pos == 0;

        public override bool CanClose(StringBuilder text, int pos) => pos == 0;
        public override bool CanOpen(StringBuilder text, int pos) => CanCloseBase(text, pos);
    }
}
