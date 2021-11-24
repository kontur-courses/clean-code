﻿using System;
using System.Linq;
using System.Text;

namespace Markdown.Tokens
{
    public class TagNode
    {
        public readonly Tag Tag;
        public readonly TagNode[] Children;

        public TagNode(Tag tag, TagNode[] children)
        {
            Tag = tag;
            Children = children;
        }

        public TagNode(Tag tag, TagNode child) : this(tag, new[] { child })
        {
        }

        public TagNode(Tag tag) : this(tag, Array.Empty<TagNode>())
        {
        }

        public string ToText()
        {
            if (Children.Length == 0) return Tag.Value;
            return new StringBuilder()
                .Append(Tag.Value)
                .AppendJoin("", Children.Select(x => x.ToText()))
                .Append(Tag.Value)
                .ToString();
        }

        public override bool Equals(object? obj) => obj is TagNode node && Equals(node);

        private bool Equals(TagNode node) => Tag.Equals(node.Tag) && Children.SequenceEqual(node.Children);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Tag.GetHashCode());
            foreach (var child in Children) hash.Add(child.GetHashCode());

            return hash.ToHashCode();
        }

        public override string ToString() => ToString(0);

        private string ToString(int nesting)
        {
            var sb = new StringBuilder();
            var tab = new string('\t', nesting);
            sb.AppendLine(tab);
            sb.AppendLine($"{tab}{{");
            sb.AppendLine($"{tab}\tToken = {ToString(Tag)}");
            if (Children.Length > 0)
            {
                sb.AppendLine($"{tab}\tChildren = ");
                sb.Append($"{tab}\t[");
                foreach (var child in Children) sb.Append($"{tab}\t{child.ToString(nesting + 1)}");
                sb.AppendLine($"{tab}\t]");
            }

            sb.AppendLine($"{tab}}}");
            return sb.ToString();
        }

        private static string ToString(Tag token) => $"{{ TagType = {token.Type}, Value = \"{token.Value}\" }}";
    }
}