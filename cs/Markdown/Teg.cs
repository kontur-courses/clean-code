using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Teg
    {
        public Rule Rule { get; private protected set; }
        public TextSeparator StartSeparator { get; private set; }
        private protected static Dictionary<string, Func<Teg>> registerTegs = new Dictionary<string, Func<Teg>>();

        static Teg()
        {
            registerTegs.Add("_", () => new TegEm());
            registerTegs.Add("__", () => new TegStrong());
        }

        public static Teg CreateTegOnTextSeparator(TextSeparator textSeparator)
        {
            var teg = registerTegs[textSeparator.separator]();
            teg.StartSeparator = textSeparator;
            return teg;
        }
    }
}
