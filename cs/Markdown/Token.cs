using System.Collections.Generic;

namespace Markdown
{
    internal class Token
    {
        public int Position { get; private set; }
        public int Length { get; private set; }
        public TokenType Type { get; private set; }
        public string ReadOnlyProperty { get; private set; }
        
        /// <summary>
        /// Из каждого токена может "расти" дерево внутренних токенов, подобно AST.
        /// Не AST т.к. в МД нет корня явного (ну, как результат выражения например для математических строк)
        /// </summary>
        public IEnumerable<Token> InnerTokens { get; private set; }
    }
}