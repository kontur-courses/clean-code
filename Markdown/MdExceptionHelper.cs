using System;

namespace Markdown
{
    internal record ExceptionCheckObject(string Name, object Obj);
    
    internal static class MdExceptionHelper
    {
        public static void ThrowArgumentNullExceptionIf(params ExceptionCheckObject[] objects)
        {
            foreach (var (name, obj) in objects)
            {
                if (name is null) throw new ArgumentException("object name can not be null");
                if (obj is null) throw new ArgumentNullException(name);
            }
        }
    }
}