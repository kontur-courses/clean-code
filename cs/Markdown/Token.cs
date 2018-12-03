using System.Collections.Generic;

namespace Markdown
{
    public abstract class Token
    {
        /// <summary>
        ///     String tokens should be ignored;
        /// </summary>
        public abstract List<Token> InnerTokens { get; set; }

        public abstract int Length { get; set; }

        public abstract Token ParentToken { get; set; }

        public abstract int Position { get; set; }

        public abstract string Value { get; set; }
    }
}
