using System.Collections.Generic;

namespace Markdown
{
    public static class StackExtensions
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T item)
            where T : class
        {
            item = null;
            if (stack.Count == 0)
                return false;
            item = stack.Peek();
            return true;
        }
    }
}