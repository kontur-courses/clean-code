using System.Collections.Generic;

namespace Markdown
{
    public abstract class Token
    {
        public abstract int Position { get; set; }
        public abstract int Length { get; set; }

        public abstract string Value { get; set; }

        /// <summary>
        ///     Из каждого токена может "расти" дерево внутренних токенов, подобно AST.
        ///     Не AST т.к. в МД нет корня явного (ну, как результат выражения например для математических строк)
        /// </summary>
        public abstract List<Token> InnerTokens { get; set; }

        public abstract Token ParentToken { get; set; }
    }
}
