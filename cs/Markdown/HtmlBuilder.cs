using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlBuilder
    {
        private string text;
        private Dictionary<Mod, string> htmlAnalogs;
        private StringBuilder builder;
        private Token curLinkNameModule;

        public HtmlBuilder(Dictionary<Mod, string> htmlAnalogs, string text)
        {
            this.htmlAnalogs = htmlAnalogs;
            this.text = text;
            builder = new StringBuilder(text.Length);
        }

        public void ConvertTokensToHtml(List<Token> verifiedModules)
        {
            foreach (var module in verifiedModules)
            {
                string moduleText;

                switch (module.modType)
                {
                    case Mod.Common:
                        moduleText = GetCommonModuleText(module);
                        builder.Append(moduleText);
                        break;

                    case Mod.Italic:
                        moduleText = GetHtmlModuleText(module);
                        builder.Append(moduleText);
                        break;

                    case Mod.Bold:
                        moduleText = GetHtmlModuleText(module);
                        builder.Append(moduleText);
                        break;

                    case Mod.Title:
                        moduleText = GetHtmlModuleText(module);
                        builder.Append(moduleText);
                        break;

                    case Mod.LinkName:
                        curLinkNameModule = module;
                        break;

                    case Mod.LinkUrl:
                        moduleText = GetLinkModuleText(module);
                        builder.Append(moduleText);
                        break;
                }
            }
        }

        private string GetLinkModuleText(Token module)
        {
            var linkName = text.Substring(curLinkNameModule.StartInd + 1, 
                curLinkNameModule.EndInd - curLinkNameModule.StartInd - 1);
            var linkUrl = text.Substring(module.StartInd + 1, module.EndInd - module.StartInd - 1);

            var html = $"<a href=\"{linkUrl}\">{linkName}</a>";
            return html;
        }

        private string GetCommonModuleText(Token module)
        {
            var startInd = module.StartInd;
            var moduleLenght = module.EndInd - module.StartInd + 1;
            string moduleText;

            if (startInd + moduleLenght >= text.Length) moduleText = text.Substring(startInd);
            else moduleText = text.Substring(startInd, moduleLenght);

            return moduleText;
        }

        private string GetHtmlModuleText(Token module)
        {
            var htmlAnalog = htmlAnalogs[module.modType];
            string moduleText;

            if (module.isOpen) moduleText = $"<{htmlAnalog}>";
            else moduleText = $"</{htmlAnalog}>";

            return moduleText;
        }
        public string GetHtml()
        {
            return builder.ToString();
        }
    }
}
