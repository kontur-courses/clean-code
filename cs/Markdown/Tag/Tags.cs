using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tag
{
    public static class Tags
    {
        private static readonly IList<ITag> RegisteredTags;

        static Tags()
        {
            var type = typeof(ITag);
            RegisteredTags = type.Assembly.GetTypes()
                .Where(t => type.IsAssignableFrom(t))
                .SelectMany(t => t.GetFields().Select(f => f.GetValue(null)).Cast<ITag>())
                .ToList();
        }

        public static T GetOrDefault<T>(string value) where T : ITag
        {
            return (T)RegisteredTags
                .SingleOrDefault(t => t.Opening == value || t.Closing == value);
        }

        public static IEnumerable<T> GetAll<T>() where T : ITag
        {
            var type = typeof(T);
            return RegisteredTags
                .Where(tag => type.IsInstanceOfType(tag))
                .Cast<T>();
        }
    }
}
