using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;

namespace Markdown.ParserClasses.Parsers
{
//    public class Utils
//    {
//        public static Tuple<List<Node>, int> ParseMultiple(TokenList tokens, Func<TokenList, Node> parser)
//        {
//            var nodes = new List<Node>();
//            var consumed = 0;
//
//            while (true)
//            {
//                var node = parser(tokens.Offset(consumed));
//                if (node == null)
//                    break;
//                nodes.Add(node);
//                consumed += node.Consumed;
//            }
//
//            return Tuple.Create(nodes, consumed);
//        }
//
//        public static Tuple<List<ParagraphNode>, int> ParseMultipleParagraph(TokenList tokens, Func<TokenList, ParagraphNode> parser)
//        {
//            var nodes = new List<ParagraphNode>();
//            var consumed = 0;
//
//            while (true)
//            {
//                var node = parser(tokens.Offset(consumed));
//                if (node == null)
//                    break;
//                nodes.Add(node);
//                consumed += node.Consumed;
//            }
//
//            return Tuple.Create(nodes, consumed);
//        }
//    }
}
