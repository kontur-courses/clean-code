using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class MdTest
    {
        [Test]
        public void Render_SimpleHeader_Work()
        {
            var md = new Md();
            var mdLine = "# header";
            var html = md.Render(mdLine);
            var correctHtml = "\\<h1>header\\</h1>";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_HeaderWithLattice_Work()
        {
            var md = new Md();
            var mdLine = "# header#";
            var html = md.Render(mdLine);
            var correctHtml = "\\<h1>header#\\</h1>";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_Italics_Work()
        {
            var md = new Md();
            var mdLine = "_t_";
            var html = md.Render(mdLine);
            var correctHtml = "\\<em>t\\</em>";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_Bold_Work()
        {
            var md = new Md();
            var mdLine = "__t__";
            var html = md.Render(mdLine);
            var correctHtml = "\\<strong>t\\</strong>";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_Example()
        {
            var md = new Md();
            var mdLine = "# Заголовок __с _разными_ символами__";
            var html = md.Render(mdLine);
            var correctHtml = "\\<h1>Заголовок \\<strong>с \\<em>разными\\</em> символами\\</strong>\\</h1>";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_Shielding() //экранирование
        {
            var md = new Md();
            var mdLine = "\\_Вот это\\_";
            var html = md.Render(mdLine);
            var correctHtml = "_Вот это_";
            html.Should().Be(correctHtml);
        }


        [Test]
        public void Render_UnpairedBold()
        {
            var md = new Md();
            var mdLine = "__Непарные";
            var html = md.Render(mdLine);
            var correctHtml = "__Непарные";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_UnpairedItalics()
        {
            var md = new Md();
            var mdLine = "_Непарные";
            var html = md.Render(mdLine);
            var correctHtml = "_Непарные";
            html.Should().Be(correctHtml);
        }

        [Test]
        public void Render_UnpairedItalicsAndBold()
        {
            var md = new Md();
            var mdLine = "__Непарные_ символы в";
            var html = md.Render(mdLine);
            var correctHtml = "__Непарные_ символы в";
            html.Should().Be(correctHtml);
        }


        [Test]
        public void Render_TextWithNumbers()
        {
            var md = new Md();
            var mdLine = "внутри текста c цифрами_12_3 не считаются";
            var html = md.Render(mdLine);
            var correctHtml = "внутри текста c цифрами_12_3 не считаются";
            html.Should().Be(correctHtml);
        }


        [Test]
        public void Render_ItalicsInBold_Work()
        {
            var md = new Md();
            var mdLine = "Внутри __двойного выделения _одинарное_ тоже__ работает";
            var html = md.Render(mdLine);
            var correctHtml = "Внутри \\<strong>двойного выделения \\<em>одинарное\\</em> тоже\\</strong> работает";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_BoldInItalics_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
            var html = md.Render(mdLine);
            var correctHtml = "Но не наоборот — внутри \\<em>одинарного __двойное__ не\\</em> работает.";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_ItalicsInStartOfWord()
        {
            var md = new Md();
            var mdLine = "Однако выделять часть слова они могут: и в _нач_але,";
            var html = md.Render(mdLine);
            var correctHtml = "Однако выделять часть слова они могут: и в \\<em>нач\\</em>але,";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_BoldInStartOfWord()
        {
            var md = new Md();
            var mdLine = "Однако выделять часть слова они могут: и в __нач__але,";
            var html = md.Render(mdLine);
            var correctHtml = "Однако выделять часть слова они могут: и в \\<strong>нач\\</strong>але,";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_ItalicsInMiddleOfWord()
        {
            var md = new Md();
            var mdLine = "Однако выделять часть слова они могут: и в сер_еди_не";
            var html = md.Render(mdLine);
            var correctHtml = "Однако выделять часть слова они могут: и в сер\\<em>еди\\</em>не";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_BoldInMiddleOfWord()
        {
            var md = new Md();
            var mdLine = "Однако выделять часть слова они могут: и в сер__еди__не";
            var html = md.Render(mdLine);
            var correctHtml = "Однако выделять часть слова они могут: и в сер\\<strong>еди\\</strong>не";
            html.Should().Be(correctHtml);
        }
        
        [Test]
        public void Render_ItalicsInDifferentWords_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "В то же время выделение в ра_зных сл_овах не работает.";
            var html = md.Render(mdLine);
            var correctHtml = "В то же время выделение в ра_зных сл_овах не работает.";
            html.Should().Be(correctHtml);
        } 
        
        [Test]
        public void Render_BoldInDifferentWords_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "В то же время выделение в ра__зных сл__овах не работает.";
            var html = md.Render(mdLine);
            var correctHtml = "В то же время выделение в ра__зных сл__овах не работает.";
            html.Should().Be(correctHtml);
        } 
        
        [Test]
        public void Render_EmptyString_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "____";
            var html = md.Render(mdLine);
            var correctHtml = "____";
            html.Should().Be(correctHtml);
        } 
        
        [Test]
        public void Render_ClosingItalicsWithSpace_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "Иначе эти _подчерки _не считаются_ о";
            var html = md.Render(mdLine);
            var correctHtml = "Иначе эти _подчерки \\<em>не считаются\\</em> о";
            html.Should().Be(correctHtml);
        } 
        
        [Test]
        public void Render_ClosingBoldAfterSpace_DoesNotWork()
        {
            var md = new Md();
            var mdLine = "__подчерки __не считаются__";
            var html = md.Render(mdLine);
            var correctHtml = "__подчерки \\<strong>не считаются\\</strong>";
            html.Should().Be(correctHtml);
        } 
        
        
    }
}