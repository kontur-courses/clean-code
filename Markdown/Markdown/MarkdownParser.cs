using System.Collections.Generic;
using System.Linq;
using Markdown.Ecxeptions;
using Markdown. Elements;

namespace Markdown
{
    public class MarkdownParser
    {
        private readonly MarkdownLexer lexer;

        public MarkdownParser(string sourсe)
        {
            lexer = new MarkdownLexer(sourсe);
        }

        public IElement Parse()
        {
            var root = new CompositeElement(lexer.Tokenize());
            BuildTree(root);
            return root;
        }

        private void BuildTree(CompositeElement element)
        {
            var tokens = element.Tokens;
            if (tokens.Length == 0)
                return;
            if (tokens.Length == 1)
            {
                element.Child = new SimpleElement(tokens.Single());
            }
            else if (tokens.TrySeparateSimplePart(out var tuple))
            {
                element.LeftChild = new SimpleElement(tuple.SeparetedPart.First());
                CreateNextCompositeElement(tuple.OtherTokens, element);
                
            }
            else if(tokens.TrySeparateStylePart(out var tokenTuple))
            {
                var styleElement = CreateStyleElement(tokenTuple.SeparetedPart, element);
                element.LeftChild = styleElement;
                CreateNextCompositeElement(styleElement.InnerTokens, styleElement);
                CreateNextCompositeElement(tokenTuple.OtherTokens, element);              
            }
            else if (tokens.TrySeparateLinkPart(out var tokentup))
            {
                var styleElement = CreateStyleElement(tokenTuple.SeparetedPart, element);
                element.LeftChild = styleElement;
                CreateNextCompositeElement(styleElement.InnerTokens, styleElement);
                CreateNextCompositeElement(tokenTuple.OtherTokens, element);
            }
            else
            {
                element.LeftChild = new SimpleElement(element.Tokens.First());
                CreateNextCompositeElement(tokens.Skip(1), element);
            }
        }

        private void CreateNextCompositeElement(IEnumerable<Token> tokens, IElement parent)
        {
            var comp = new CompositeElement(tokens.ToArray());
            parent.Child = comp;              
            BuildTree(comp);
        }

        private IStyleElement CreateStyleElement(IEnumerable<Token> tokens, IElement parent)
        {
            var innerTokens = tokens.GetInnerTokens();
            var firstToken = tokens.First();
            switch (firstToken.Type)
            {
                case TokenType.BoldDelimiter:
                    return new BoldStyleElement(innerTokens);
                case TokenType.ItalicDelimiter:
                    return new ItalicStyleElement(innerTokens);
            }           
            throw new UnknownTokenException(firstToken);
        }    
    }
}
