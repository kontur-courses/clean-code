using System.Collections.Generic;

namespace Markdown.Extensions
{
    public static class StackExtensions
    {
        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            return stack.Count == 0;
        }
    }
}
