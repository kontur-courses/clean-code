using Markdown.InlinePieces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Markdown
{
    public static class HtmlRenderer
    {
        public static string Render(RenderedBlock[] blocks)
        {
            if (blocks.Length == 0)
                return string.Empty;

            var htmlStrings = new string[blocks.Length];

            for (var i = 0; i < blocks.Length; i++)
            {
                htmlStrings[i] = RenderBlock(blocks[i]);
            }

            return string.Join("\n", htmlStrings);
        }

        private static string RenderBlock(RenderedBlock block)
        {
            var htmlBlock = RenderInlinePieces(block.InlinePieces);

            if (block.Type == BlockType.Heading)
                return $"<h1>{htmlBlock}</h1>";
            else
                return $"<p>{htmlBlock}</p>";
        }

        private static string RenderInlinePieces(InlinePiece[] pieces)
        {
            var htmlPieces = new string[pieces.Length];
            for (var i = 0; i < pieces.Length; i++)
            {
                htmlPieces[i] = RenderInlinePiece(pieces[i]);
            }

            return string.Join("", htmlPieces);
        }

        private static string RenderInlinePiece(InlinePiece piece)
        {
            switch (piece)
            {
                case TextInlinePiece text:
                    return text.Text;
                case EmInlinePiece em:
                    return @$"<em>{RenderInlinePieces(em.Children)}</em>";
                case StrongInlinePiece strong:
                    return @$"<strong>{RenderInlinePieces(strong.Children)}</strong>";
                case LinkInlinePiece link:
                    return $"<a href=\"{link.Url}\">{RenderInlinePieces(link.Children)}</a>";
                default:
                    throw new ArgumentException();

            }
        }
    }
}
