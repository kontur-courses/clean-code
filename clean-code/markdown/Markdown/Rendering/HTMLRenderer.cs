using Markdown.Models;
using System.Text;
using System.Text.Encodings.Web;

namespace Markdown.Rendering
{
	// Рекурсивно обходит INode и собирает HTML.
	public class HTMLRenderer
	{
		public string Render(IReadOnlyList<INode> nodes)
		{
			var sb = new StringBuilder();
			RenderNodes(sb, nodes);
			return sb.ToString();
		}

		private void RenderNodes(StringBuilder sb, IReadOnlyList<INode> nodes)
		{
			foreach (var node in nodes)
			{
				switch (node)
				{
					case TextNode t:
						sb.Append(System.Net.WebUtility.HtmlEncode(t.Value));
						break;

					case EmNode em:
						sb.Append("<em>");
						RenderNodes(sb, em.Children);
						sb.Append("</em>");
						break;

					case StrongNode st:
						sb.Append("<strong>");
						RenderNodes(sb, st.Children);
						sb.Append("</strong>");
						break;

					case HeadingNode h:
						sb.Append("<h1>");
						RenderNodes(sb, h.Children);
						sb.Append("</h1>");
						break;

					case ParagraphNode p:
						sb.Append("<p>");
						RenderNodes(sb, p.Children);
						sb.Append("</p>");
						break;

					case UnorderedListNode ul:
						sb.Append("<ul>");
						RenderNodes(sb, ul.Children);
						sb.Append("</ul>");
						break;

					case ListItemNode li:
						sb.Append("<li>");
						RenderNodes(sb, li.Children);
						sb.Append("</li>");
						break;
				}
			}
		}
	}
}
