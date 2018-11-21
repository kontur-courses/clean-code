using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public abstract class TagInfo
    {
        public abstract Predicate<Window> StartCondition { get; }
        public abstract Predicate<Window> EndCondition { get; }
        public abstract Action<TagReader> OnTagStart { get; }
        public abstract Action<TagReader> OnTagEnd { get; }
        public abstract string TagText { get; }
        public abstract int TagLength { get; }
        public abstract Token GetNewToken(int position);
    }
}
