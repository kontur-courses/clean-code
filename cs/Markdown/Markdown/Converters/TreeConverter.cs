using System.Linq;
using Markdown.Tag;
using Markdown.Tree;

namespace Markdown.Converters
{
    public static class TreeConverter
    {
        public static Node ConvertToTree(string text)
        {
            if (text == null)
            {
                return null;
            }

            var result = new Node(new EmptyTag(0, text.Length));

            var tags = TagsConverter.GetAllTags(text);
            foreach (var tag in tags)
            {
                AddTag(result, tag);
            }

            return result;
        }

        private static void AddTag(Node tree, ITag tag)
        {
            if (tree.Tag == null)
            {
                tree.Tag = tag;
            }
            else
            {
                if (tree.Children.Count == 0)
                {
                    tree.Children.Add(new Node(tag));
                }
                else
                {
                    foreach (var subtree in tree.Children.Where(node =>
                        tag.End < node.Tag.End && tag.Start > node.Tag.Start))
                    {
                        AddTag(subtree, tag);
                        return;
                    }

                    tree.Children.Add(new Node(tag));
                }
            }
        }
    }
}