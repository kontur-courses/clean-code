using System.Collections.Generic;

namespace Markdown.Extensions
{
    public static class StackExtensions
    {
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            if (!stack.IsEmpty())
            {
                value = stack.Pop();
                return true;
            }
            value = default;
            return false;
        }

        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            return stack.Count == 0;
        }
    }
}
