using System.Collections.Generic;

namespace Markdown
{
    public static class StackExtensions
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            value = default(T);
            if (stack.Count == 0)
                return false;
            value = stack.Peek();
            return true;
        }
    }
}
