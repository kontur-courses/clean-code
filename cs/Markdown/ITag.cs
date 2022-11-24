﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITag
    {
        public Tag Tag { get; set; }

        public bool IsEnd(char prevCh);
        public bool IsBegin(char nextCh);

        public Tag GetClosedTag(char nextCh = ' ');

        public Tag GetOpenTag(char prevCh);
        public bool ShouldClose { get; }
        public void SetIsSpaceBetween();
    }
}
