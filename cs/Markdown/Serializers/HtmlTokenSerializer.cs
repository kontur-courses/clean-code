using Markdown.Tokens;
using System.Text;

namespace Markdown.Serializers
{
    public class HtmlTokenSerializer : ITokenSerializer
    {
        public static readonly Dictionary<TagType, (string openTag, string closingTag)> Translations;

        static HtmlTokenSerializer()
        {
            Translations = new Dictionary<TagType, (string openTag, string closingTag)>()
            {
                { TagType.Text, ("", "") },
                { TagType.Header, ("<h1>", "</h1>") },
                { TagType.Italic, ("<em>", "</em>") },
                { TagType.Bold, ("<strong>", "</strong>") },
                { TagType.Line, ("", "<br>") },
                { TagType.LastLine, ("", "") }            
            };
        }

        public string Serialize(MdDoc document)
        {
            var sb = new StringBuilder();

            foreach (var tag in document.Lines)
            {
                sb.Append(Translations[tag.Type].openTag);
                Traverse(tag);
                sb.Append(Translations[tag.Type].closingTag);
            }

            return sb.ToString();

            void Traverse(NestedTag parentTag)
            {
                foreach (var tag in parentTag.Tags)
                {
                    sb.Append(Translations[tag.Type].openTag);

                    if (tag is NestedTag)
                        Traverse(tag as NestedTag);
                    else
                        sb.Append((tag as AtomicTag).Content);

                    sb.Append(Translations[tag.Type].closingTag);
                }
            }
        }
    }
}