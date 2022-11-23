using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlBuilder
    {
        private string text;
        private Dictionary<Mod, string> htmlAnalogs;
        private StringBuilder builder;

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
                var startInd = module.StartInd;
                var moduleLenght = module.EndInd - module.StartInd + 1;
                string moduleText;

                switch (module.modType)
                {
                    case Mod.Common:
                        if (startInd + moduleLenght >= text.Length) moduleText = text.Substring(startInd);
                        else moduleText = text.Substring(startInd, moduleLenght);
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
                }
            }
        }

        public string GetHtml()
        {
            return builder.ToString();
        }

        private string GetHtmlModuleText(Token module)
        {
            var htmlAnalog = htmlAnalogs[module.modType];
            string moduleText;

            if (module.isOpen) moduleText = $"<{htmlAnalog}>";
            else moduleText = $"</{htmlAnalog}>";

            return moduleText;
        }
    }
}
