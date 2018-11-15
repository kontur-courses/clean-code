using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Teg
    {
        public TegRule TegRule { get; private protected set; }
        public TextSeparator StartSeparator { get; private set; }
        private protected static Dictionary<string, Func<Teg>> RegisterTegs = new Dictionary<string, Func<Teg>>();

        static Teg()
        {
            // todo get Teg children (container)
            RegisterTegs.Add("_", () => new TegEm());
            RegisterTegs.Add("__", () => new TegStrong());
        }

        public static Teg CreateTegOnTextSeparator(TextSeparator textSeparator)
        {
            var teg = RegisterTegs[textSeparator.Separator]();
            teg.StartSeparator = textSeparator;
            return teg;
        }
    }
}
