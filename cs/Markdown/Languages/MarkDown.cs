using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tree;

namespace Markdown.Languages
{
    public class MarkDown : ILanguage
    {
        private Stack<TagToken> stackOfTags;
        private List<TagToken> validTags;
        public Dictionary<TagType, Tag> Tags { get; }

        public MarkDown() : base()
        {
            Tags = new Dictionary<TagType, Tag>()
            {
                {TagType.Em, new Tag("_", "_", new TagType[] { })},
                {TagType.Strong, new Tag("__", "__", new TagType[] {TagType.Em})}
            };
            stackOfTags = new Stack<TagToken>();
            validTags = new List<TagToken>();
        }

        public SyntaxTree RenderTree(string str)
        {
            if (str == null)
                throw new ArgumentException("The string should not be null");
            for (var i = 0; i < str.Length; i++)
            {
                var tag = CreateTag(str, i);
                if (tag == null)
                    continue;
                
                if (tag.IsOpen && (stackOfTags.Count == 0 || (stackOfTags.Peek().Tagtype != tag.Tagtype && !stackOfTags.Peek().IsOpen)))
                {
                    stackOfTags.Push(tag);
                    
                }
                else if(!tag.IsOpen && stackOfTags.Count > 0 && stackOfTags.Peek().Tagtype == tag.Tagtype && stackOfTags.Peek().IsOpen)
                {
                    UpdateTags(tag);
                    
                }

                if (tag.IsOpen)
                    i += Tags[tag.Tagtype].Start.Length;
                else
                    i += Tags[tag.Tagtype].End.Length;
            }
            
            return validTags.Count == 0 ? new SyntaxTree(new List<SyntaxNode>(){new TextNode(str)}) : ReplaceMdToSyntaxTree(str);
        }

        private TagToken CreateTag(string line, int i)
        {
            if (IsStrongTag(line, i) && IsCloseTag(line, i))
                return new TagToken(TagType.Strong, false, i);

            if (IsStrongTag(line, i) && IsOpenTag(line, i + 1))
                return new TagToken(TagType.Strong, true, i);

            if (IsEmTag(line, i) && IsCloseTag(line, i))
                return new TagToken(TagType.Em, false, i);

            if (IsEmTag(line, i) && IsOpenTag(line, i))
                return new TagToken(TagType.Em, true, i);
            return null;
        }

        private void UpdateTags(TagToken closeTag)
        {
            while (stackOfTags.Count != 0)
            {
                if (stackOfTags.Peek().Tagtype == closeTag.Tagtype)
                    break;
                stackOfTags.Pop();
            }

            if (stackOfTags.Count == 0) return;

            var openTag = stackOfTags.Pop();
            validTags.Add(openTag);
            validTags.Add(closeTag);
        }

        private SyntaxTree ReplaceMdToSyntaxTree(string line)
        {
            var syntaxTree = new SyntaxTree();
            var topBranch = new Stack<TagToken>();
            validTags.Sort((t1, t2) => t1.Position.CompareTo(t2.Position));
            var i = 0;
            foreach (var tag in validTags)
            {
                if (tag.IsOpen &&( topBranch.Count == 0 ||
                    Tags[topBranch.Peek().Tagtype].Children.Contains(tag.Tagtype)))
                {
                    if (tag.Position - i != 0)
                    {
                        syntaxTree.Add(new TextNode(line.Substring(i,tag.Position)));
                    }
                    i = tag.Position + Tags[tag.Tagtype].Start.Length;
                    topBranch.Push(tag);
                }
                if (!tag.IsOpen)
                {
                    while (topBranch.Count != 0)
                    {
                        if (topBranch.Peek().Tagtype == tag.Tagtype)
                        {
                            var node = new TagNode(tag.Tagtype,
                                new List<SyntaxNode>()
                                {
                                    new TextNode(
                                        line.Substring(i, tag.Position - i))
                                });
                            i = tag.Position + Tags[tag.Tagtype].End.Length;
                            topBranch.Pop();
                            syntaxTree.Add(node);
                            break;
                        }
                        stackOfTags.Pop();
                    }
                }
            }
            
            if (line.Length - i != 0)
            {
                syntaxTree.Add(new TextNode(
                    line.Substring(i, line.Length - i)));
            }

            return syntaxTree;
        }


        private bool IsOpenTag(string line, int i)
        {
            return i + 1 < line.Length && line[i + 1] != ' ' && !char.IsNumber(line, i + 1);
        }

        private static bool IsCloseTag(string line, int i)
        {
            return i > 0 && line[i - 1] != ' ' && !char.IsNumber(line, i - 1);
        }

        private static bool IsEmTag(string line, int i) => line[i] == '_' && (i+1 >= line.Length || line[i+1] != '_');

        private static bool IsStrongTag(string line, int i)
        {
            return i < line.Length - 1 && line[i] == '_' && line[i + 1] == '_' && (i+2>= line.Length || line[i + 2] != '_');
        }
    }
}