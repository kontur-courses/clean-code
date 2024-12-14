using System.Text;
using Markdown.AbstractSyntaxTree;
using Markdown.NodeView;
using Markdown.Parser;
using Markdown.SyntaxRules;
using Markdown.Token;
using Markdown.Tokenizer;

namespace Markdown;

public class Md(
    Dictionary<MdTokenType, string> tokenTags,
    ITokenizer<MdTokenType, MdToken> tokenizer,
    IParser<MdTokenType, MdToken> parser,
    ISyntaxRule<MdTokenType>[] syntaxRules) : IRenderer
{
    public string Render(string input)
    {
        var tokens = tokenizer.Tokenize(input.AsMemory());
        var parseTree = parser.Parse(tokens);
        var syntaxTree = MdAbstractSyntaxTree.FromParseTree(parseTree);

        foreach (var syntaxRule in syntaxRules)
            syntaxTree.AddRule(syntaxRule);
        
        return syntaxTree
            .ApplyRules()
            .Traverse()
            .Aggregate(new StringBuilder(),
                (sb, node) => ProcessNode(node, sb))
            .ToString();
    }

    private StringBuilder ProcessNode(BaseNodeView<MdTokenType>? node, StringBuilder sb)
    {
        if (node is AbstractSyntaxTreeNodeView<MdTokenType> nodeView)
        {
            if (nodeView.TokenType is MdTokenType.PlainText or MdTokenType.Document or MdTokenType.Line)
                sb.Append(nodeView.Text);
            else
                sb.Append($"<{tokenTags[nodeView.TokenType]}>");
        }
        else if (node is ViewEnd<MdTokenType> viewEnd)
        {
            if (viewEnd.TokenType is not (MdTokenType.PlainText or MdTokenType.Document or MdTokenType.Line))
            {
                sb.Append($"</{tokenTags[viewEnd.TokenType]}>");
            }
        }

        return sb;
    }
}