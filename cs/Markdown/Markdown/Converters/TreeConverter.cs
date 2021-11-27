using System.Linq;
using Markdown.MdTags;
using Markdown.SyntaxTree;

namespace Markdown.Converters
{
    public static class TreeConverter
    {
        public static Tree ConvertToTree(string paragraph)
        {
            if (paragraph == null)
            {
                return null;
            }

            var result = new Tree(new Tag(0, paragraph.Length));
            var tags = TagsConverter.GetAllTags(paragraph);
            foreach (var tag in tags)
            {
                AddTag(result, tag);
            }

            return result;
        }

        private static void AddTag(Tree tree, Tag tag)
        {
            if (tree.Root == null)
            {
                tree.Root = tag;
            }
            else
            {
                if (!CanAddTag(tree, tag))
                {
                    return;
                }

                if (tree.Children.Count == 0)
                {
                    tree.Children.Add(new Tree(tag));
                }
                else
                {
                    foreach (var subtree in tree.Children.Where(node =>
                        tag.End < node.Root.End && tag.Start > node.Root.Start))
                    {
                        AddTag(subtree, tag);
                        return;
                    }

                    tree.Children.Add(new Tree(tag));
                }
            }
        }

        private static bool CanAddTag(Tree tree, Tag tag)
        {
            return !(tree.Root.Type == TagType.Italics && tag.Type == TagType.StrongText ||
                     tree.Root.Type == TagType.UnnumberedList && tag.Type != TagType.ListElement ||
                     tree.Root.Type == TagType.Title && tag.Type is TagType.UnnumberedList or TagType.ListElement ||
                     tree.Children.Count == 0 && tree.Root.Type != TagType.UnnumberedList &&
                     tag.Type == TagType.ListElement);
        }
    }
}