﻿using System.Collections;
using System.Linq.Expressions;

namespace Markdown
{
    public class ModifierToken : Token
    {
        public ConcType type;

        public ModifierToken(ConcType type)
        {
            this.type = type;
            Length = (int) type;
        }
    }
}
