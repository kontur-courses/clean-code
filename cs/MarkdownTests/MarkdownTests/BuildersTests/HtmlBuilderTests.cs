using NUnit.Framework;
using Markdown.Models;
using Markdown.Entities.Builders;
using Markdown.Enums;
using Markdown.Models.SyntaxTreeModels;

namespace Markdown.MarkdownTests.BuildersTests
{
    [TestFixture]
    public class HtmlBuilderTests
    {
        private HtmlBuilder _htmlBuilder;

        [SetUp]
        public void Setup()
        {
            _htmlBuilder = new HtmlBuilder();
        }

        [Test]
        public void Build_ReturnsEmTag_WithItalicText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateItalic("окруженный с двух сторон")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><em>окруженный с двух сторон</em></p>"));
        }

        [Test]
        public void Build_ReturnsStrongTag_WithBoldText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateBold("выделенный текст")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><strong>выделенный текст</strong></p>"));
        }

        [Test]
        public void Build_ReturnsPlainText_WithEscapedUnderscore()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("_Вот это_")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>_Вот это_</p>"));
        }

        [Test]
        public void Build_ReturnsNestedTags_WithBoldInsideItalic()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateBold(
                        CreateText("внутри двойного "),
                        CreateItalic("одинарное работает"),
                        CreateText(" правильно")
                    )
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><strong>внутри двойного <em>одинарное работает</em> правильно</strong></p>"));
        }

        [Test]
        public void Build_ItalicTreatedAsText_WithItalicInsideBold()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateItalic(
                        CreateText("внутри одинарного "),
                        CreateText("__двойное не работает__"),
                        CreateText(" как ожидается")
                    )
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><em>внутри одинарного __двойное не работает__ как ожидается</em></p>"));
        }

        [Test]
        public void Build_ReturnsPlainText_WithUnderscoresWithNumbers()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("цифрами_12_3")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>цифрами_12_3</p>"));
        }

        [Test]
        public void Build_CanBeFormatted_WithWordParts()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("в нач"),
                    CreateItalic("але"),
                    CreateText(" слова")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>в нач<em>але</em> слова</p>"));
        }

        [Test]
        public void Build_ReturnsPlainUnderscores_WithEmptyFormatting()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("____")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>____</p>"));
        }

        [Test]
        public void Build_ReturnsHeaderWithNestedTags_WithHeaderWithFormatting()
        {
            var tree = CreateSyntaxTree(
                CreateHeader(
                    CreateText("Заголовок "),
                    CreateBold(
                        CreateText("с "),
                        CreateItalic("разными"),
                        CreateText(" символами")
                    )
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"));
        }

        [Test]
        public void Build_ReturnsSeparatedParagraphs_WithMultipleParagraphs()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("Первый абзац")
                ),
                CreateParagraph(
                    CreateText("Второй абзац")
                ),
                CreateParagraph(
                    CreateText("Третий абзац")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>Первый абзац</p><p>Второй абзац</p><p>Третий абзац</p>"));
        }

        [Test]
        public void Build_ReturnsCorrectHtml_WithComplexDocument()
        {
            var tree = CreateSyntaxTree(
                CreateHeader(
                    CreateText("Заголовок")
                ),
                CreateParagraph(
                    CreateText("Обычный "),
                    CreateItalic("курсив"),
                    CreateText(" и "),
                    CreateBold("жирный")
                ),
                CreateParagraph(
                    CreateText("Новый абзац")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<h1>Заголовок</h1><p>Обычный <em>курсив</em> и <strong>жирный</strong></p><p>Новый абзац</p>"));
        }

        [Test]
        public void Build_ReturnsTextInParagraph_WithOnlyText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("Простой текст без разметки")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>Простой текст без разметки</p>"));
        }

        [Test]
        public void Build_ReturnsEmptyString_WithEmptyTree()
        {
            var tree = CreateSyntaxTree();

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void Build_ReturnsCorrectHtml_WithMixedFormattingInSentence()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("Это "),
                    CreateItalic("курсив"),
                    CreateText(", а это "),
                    CreateBold("жирный"),
                    CreateText(", а это "),
                    CreateBold(
                        CreateItalic("жирный курсив")
                    ),
                    CreateText(".")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>Это <em>курсив</em>, а это <strong>жирный</strong>, а это <strong><em>жирный курсив</em></strong>.</p>"));
        }

        [Test]
        public void Build_HandlesGracefully_WithUnclosedFormatting()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateItalic("незакрытый курсив"),
                    CreateText(" и обычный текст")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><em>незакрытый курсив</em> и обычный текст</p>"));
        }

        [Test]
        public void Build_ReturnsAnchorTag_WithSimpleLink()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateLink("пример ссылки", "https://example.com")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><a href=\"https://example.com\">пример ссылки</a></p>"));
        }

        [Test]
        public void Build_ReturnsAnchorTagWithTitle_WithLinkWithTitle()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateLinkWithTitle("ссылка с заголовком", "https://site.com", "Заголовок ссылки")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><a href=\"https://site.com\" title=\"Заголовок ссылки\">ссылка с заголовком</a></p>"));
        }

        [Test]
        public void Build_ReturnsAnchorWithStrongTag_WithLinkContainingBoldText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateLink(
                        CreateText("это "),
                        CreateBold("жирный текст")
                    , "https://example.com")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><a href=\"https://example.com\">это <strong>жирный текст</strong></a></p>"));
        }

        [Test]
        public void Build_ReturnsAnchorWithEmTag_WithLinkContainingItalicText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateLink(
                        CreateText("это "),
                        CreateItalic("курсивный текст")
                    , "https://example.com")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p><a href=\"https://example.com\">это <em>курсивный текст</em></a></p>"));
        }

        [Test]
        public void Build_ReturnsParagraphWithAnchor_WithLinkInText()
        {
            var tree = CreateSyntaxTree(
                CreateParagraph(
                    CreateText("Посетите "),
                    CreateLink("наш сайт", "https://example.com"),
                    CreateText(" для деталей")
                )
            );

            var result = _htmlBuilder.Build(tree);

            Assert.That(result, Is.EqualTo("<p>Посетите <a href=\"https://example.com\">наш сайт</a> для деталей</p>"));
        }


        // Вспомогательные методы для создания тестовых данных
        private MockSyntaxTree CreateSyntaxTree(params Node[] nodes)
        {
            return new MockSyntaxTree(new List<Node>(nodes));
        }

        private Node CreateParagraph(params Node[] children)
        {
            return new Node(NodeType.Paragraph, new List<Node>(children), null);
        }

        private Node CreateHeader(params Node[] children)
        {
            return new Node(NodeType.Header, new List<Node>(children), null);
        }

        private Node CreateBold(params Node[] children)
        {
            return new Node(NodeType.Bold, new List<Node>(children), null);
        }

        private Node CreateItalic(params Node[] children)
        {
            return new Node(NodeType.Italic, new List<Node>(children), null);
        }

        private Node CreateText(string value)
        {
            return new Node(NodeType.Text, null, value);
        }

        private Node CreateBold(string text)
        {
            return new Node(NodeType.Bold, new List<Node> { CreateText(text) }, null);
        }

        private Node CreateItalic(string text)
        {
            return new Node(NodeType.Italic, new List<Node> { CreateText(text) }, null);
        }

        // Новые вспомогательные методы для ссылок
        private Node CreateLink(string text, string url)
        {
            return new Node(NodeType.Link, new List<Node> { CreateText(text) }, url);
        }

        private Node CreateLinkWithTitle(string text, string url, string title)
        {
            return new Node(NodeType.Link, new List<Node> { CreateText(text) }, url)
            {
                Title = title
            };
        }

        private Node CreateLink(params object[] childrenAndUrl)
        {
            // Последний элемент - это URL
            var url = childrenAndUrl[childrenAndUrl.Length - 1] as string;
            var children = new List<Node>();

            for (int i = 0; i < childrenAndUrl.Length - 1; i++)
            {
                if (childrenAndUrl[i] is Node node)
                {
                    children.Add(node);
                }
                else if (childrenAndUrl[i] is string text)
                {
                    children.Add(CreateText(text));
                }
            }

            return new Node(NodeType.Link, children, url);
        }
    }

    // Mock-класс для тестирования
    internal class MockSyntaxTree : ISyntaxTree
    {
        public List<Node> Tree { get; }

        public MockSyntaxTree(List<Node> tree)
        {
            Tree = tree;
        }
    }
}