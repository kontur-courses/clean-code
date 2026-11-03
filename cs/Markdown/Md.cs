using System;
using System.Collections.Generic;

namespace Markdown
{
    public enum TokenType { Text, Underscore, DoubleUnderscore, Screening, HeaderMarker, Space, Newline, Eof }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Position { get; }
        public Token(TokenType type, string value, int position) {   
                Type = type; 
                Value = value; 
                Position = position; 
            }
    }

    public class TokenCreator
    {
        public List<Token> CreateTokens(string input) { throw new NotImplementedException(); }
    }
    
    public abstract class Node { }
    public class DocumentNode : Node { public List<Node> Children { get; } = new(); }
    public class ParagraphNode : Node { public List<Node> Children { get; } = new(); }
    public class HeadingNode : Node { public List<Node> Children { get; } = new(); }
    public class CursiveNode : Node { public List<Node> Children { get; } = new(); }
    public class BoldNode : Node { public List<Node> Children { get; } = new(); }

    public class TextNode : Node
    {
        public string Content { get; }

        public TextNode(string content)
        {
            Content = content;
        }
    }
    
    
    public class Parser
    {
        public DocumentNode Parse(List<Token> tokens) { throw new NotImplementedException(); }
    }
    
    public class HtmlRenderer
    {
        public string Render(DocumentNode doc) { throw new NotImplementedException(); }
    }
    
    public class Md
    {
        public string Render(string input)
        {
            var tokens = new TokenCreator().CreateTokens(input);
            var nodeTree = new Parser().Parse(tokens);
            var render =  new HtmlRenderer().Render(nodeTree);
            return render;
        }
    }
}