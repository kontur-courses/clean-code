using System.Xml.Linq;

namespace Markdown.Model;

public static class HtmlDocument
{
    public static XDocument CreateHtmlDocument() =>
        new(
            new XDocumentType("html", null, null, null),
            new XElement("html",
                new XElement("head",
                    new XElement("meta", new XAttribute("charset", "utf-8")),
                    new XElement("title", "Converted from Markdown")
                ),
                new XElement("body")
            )
        );

    public static XElement GetBodyElement(this XDocument document) => document.Root?.Element("body") ??
                                                                      throw new InvalidOperationException(
                                                                          "The document has no <body> element.");
}