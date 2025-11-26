using System.Text;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Rendering;

public class Renderer : IRenderer
  {
      public string Render(DocumentNode document)
      {
          var result = new StringBuilder();

          foreach (var block in document.Children)
          {
              switch (block)
              {
                  case HeadingNode heading:
                      result.Append(RenderHeading(heading));
                      break;
                  case ParagraphNode paragraph:
                      result.Append(RenderParagraph(paragraph));
                      break;
              }
          }
          return result.ToString();
      }

      private static string RenderHeading(HeadingNode heading)
      {
          var content = RenderInlines(heading.Inlines);
          return $"<h{heading.Level}>{content}</h{heading.Level}>";
      }

      private static string RenderParagraph(ParagraphNode paragraph)
      {
          var content = RenderInlines(paragraph.Inlines);
          return $"<p>{content}</p>";
      }
      

      private static string RenderInlines(IList<InlineNode> inlines)
      {
          var builder = new StringBuilder();

          foreach (var inline in inlines)
          {
              switch (inline)
              {
                  case TextNode text:
                      builder.Append(Escape(text.Text));
                      break;
                  case EmphasisNode emphasis:
                      builder.Append("<em>");
                      builder.Append(RenderInlines(emphasis.Inlines));
                      builder.Append("</em>");
                      break;
                  case StrongNode strong:
                      builder.Append("<strong>");
                      builder.Append(RenderInlines(strong.Inlines));
                      builder.Append("</strong>");
                      break;
                  case LinkNode link:
                      builder.Append("<a href=\"");
                      builder.Append(Escape(link.Href));
                      builder.Append("\">");
                      builder.Append(RenderInlines(link.Inlines));
                      builder.Append("</a>");
                      break;
              }
          }
          return builder.ToString();
      }

      private static string Escape(string text) =>
          text.Replace("&", "&amp;")
              .Replace("<", "&lt;")
              .Replace(">", "&gt;")
              .Replace("\"", "&quot;")
              .Replace("'", "&#39;");
  }
