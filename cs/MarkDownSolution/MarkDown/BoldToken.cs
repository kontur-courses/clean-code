﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class BoldToken : Token
    {
        public BoldToken(int start, int length) : base(start, length)
        {
        }
    }
}
