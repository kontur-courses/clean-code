using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MdTags
{
    abstract class MdPairTagBase : MdTagBase
    {
        public abstract Tag Open { get; }
        public abstract Tag Close { get; }
        public abstract bool IsTokenOpenTag(Token token);
        public abstract bool IsTokenCloseTag(Token token);
    }
}