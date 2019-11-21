using System;
using System.Collections.Generic;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;
using Markdown.Tree;

namespace Markdown.Parser
{
    public class TreeBuilder
    {
        private readonly List<MarkdownTag> tags;
        private readonly CharClassifier classifier;

        public TreeBuilder(List<MarkdownTag> tags, CharClassifier classifier)
        {
            this.tags = tags;
            this.classifier = classifier;
        }
        public RootNode ParseMarkdown(string markdown)
        {
            var tagsReader = new TagsReader(markdown, tags, classifier);
            var events = tagsReader.GetEvents();

            var tokenizer = new Tokenizer(markdown, events, classifier);
            var tokens = tokenizer.GetTokens();

            var root = BuildTree(tokens);

            return root;
        }

        private static RootNode BuildTree(IEnumerable<Token> tokens)
        {
            var root = new RootNode();
            Node current = root;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.PlainText:
                        current.AddNode(new PlainTextNode(token.Value));
                        break;

                    case TokenType.BoldStart:
                        var bold = new BoldNode();
                        current.AddNode(bold);
                        current = bold;
                        break;


                    case TokenType.ItalicStart:
                        var italic = new ItalicNode();
                        current.AddNode(italic);
                        current = italic;
                        break;

                    case TokenType.ItalicEnd:
                    case TokenType.BoldEnd:
                        current = current.Parent;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return root;
        }
    }
}