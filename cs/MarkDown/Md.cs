using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    //Непосредственно процессор
    //Тестов пока нет, потому что все реализации тут - наброски, чтобы примерно показать архитектуру и нагрузки не несут
    public class Md
    {
        private Tag firstTag;
        private TagParser tagParser;
        private int currentPosition;
        private IEnumerable<TagType> availableTagTypes;

        public Md(IEnumerable<TagType> availableTagTypes)
        {
            tagParser = new TagParser();
            this.availableTagTypes = availableTagTypes;
        }

        public string Renderer(string textParagraph)
        {
            currentPosition = 0;
            firstTag = new Tag(null, 0, new ParagraphTag(), textParagraph);
            return ParseMd(textParagraph).ParseToHtml();
        }

        private Tag ParseMd(string text)
        {
            
            //идти пока не найдешь спецсимвол любого из 
            //отдаем парсеру, парсер пытается найти закрывающий символ по всем канонам, если нашел, вернет true и в out тэг запишет
            //полученный тэг назначаем родителем firstTag
            //
            throw new NotImplementedException();
        }
    }
}
