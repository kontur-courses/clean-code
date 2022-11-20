using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenAnalyzer
    {
        private string text;
        private Stack<Token> openModules;
        private Stack<Token> verifiedModules;
        private List<Token> preTokens;

        public TokenAnalyzer(string text)
        {
            this.text = text;
            openModules = new Stack<Token>();
            verifiedModules = new Stack<Token>();
        }

        public List<Token> LeadToSpecification(List<Token> preliminaryTokens)
        {
            preTokens = preliminaryTokens;
            FilterDigitsAndSlashes();

            for (int i = 0; i < preliminaryTokens.Count; i++)
            {
                var curModule = preTokens[i];

                if (curModule.modType != Mod.Common)
                {
                    openModules.TryPeek(out var lastModule);

                    if (lastModule != null && lastModule.modType == curModule.modType)
                    {
                        if (curModule.startInd - lastModule.endInd == 1)
                        {
                            curModule.Close();
                            lastModule.Close();
                            curModule.modType = Mod.Common;
                            lastModule.modType = Mod.Common;
                        }
                        else if (IsModuleInWord(lastModule) && 
                            IsModuleInWord(curModule) && 
                            IsModulesInSameWord(lastModule, curModule))
                        {
                            CorrectBoldInclusion(curModule);
                            curModule.Close();
                            openModules.Pop();
                            verifiedModules.Push(curModule);
                        }
                        else if (IsModuleInWord(curModule) &&
                            !IsModulesInSameWord(lastModule, curModule))
                        {
                            curModule.modType = Mod.Common;
                            curModule.Close();
                        }
                        else if(IsClosingModule(curModule))
                        {
                            CorrectBoldInclusion(curModule);
                            curModule.Close();
                            openModules.Pop();
                            verifiedModules.Push(curModule);
                        }
                        else
                        {
                            curModule.modType = Mod.Common;
                        }
                    }
                    else
                    {
                        if (IsOpeningModule(curModule) || IsModuleInWord(curModule))
                        {
                            verifiedModules.Push(curModule);
                            openModules.Push(curModule);
                        }
                        else
                        {
                            CorrectIntersect(curModule);

                            curModule.modType = Mod.Common;
                            curModule.Close();
                        }
                    }
                }
            }

            SolveUnverifiedModules();
            return preTokens;
        }

        private void CorrectIntersect(Token curModule)
        {
            openModules.TryPop(out var lastMod);
            openModules.TryPeek(out var preLastMod);

            if (preLastMod != null && lastMod != null)
            {
                if (preLastMod.modType == curModule.modType)
                {
                    openModules.Pop();
                    preLastMod.Close();
                    lastMod.Close();

                    preLastMod.modType = Mod.Common;
                    lastMod.modType = Mod.Common;
                }
            }
        }

        private void SolveUnverifiedModules()
        {
            var isStackHasOpenTitle = false;

            foreach (var module in openModules)
            {
                if (module.modType == Mod.Title)
                {
                    if (isStackHasOpenTitle)
                    {
                        module.Close();
                        isStackHasOpenTitle = false;
                    }
                    else
                    {
                        isStackHasOpenTitle = true;
                    }
                }
                else
                {
                    module.modType = Mod.Common;
                }
            }

            if (isStackHasOpenTitle)
            {
                verifiedModules.TryPeek(out var lastModule);
                var closingToken = new Token(lastModule.endInd + 1,
                    lastModule.endInd + 1,
                    Mod.Title,
                    false);
                verifiedModules.Push(closingToken);
                preTokens.Add(closingToken);
            }
        }

        private bool IsModuleInWord(Token module)
        {
            if ((module.startInd > 0 && text[module.startInd - 1] != ' ') && 
                (module.endInd + 1 < text.Length && text[module.endInd + 1] != ' '))
            {
                return true;
            }

            return false;
        }

        private bool IsModulesInSameWord(Token first, Token second)
        {
            var subString = text.Substring(first.endInd, second.startInd - first.endInd);

            if (subString.Contains(' '))
            {
                return false;
            }

            return true;
        }

        private bool IsOpeningModule(Token curModule)
        {
            if (curModule.startInd == 0 || 
                text[curModule.startInd - 1] == ' ')
            {
                return true;
            }

            return false;
        }

        private bool IsClosingModule(Token curModule)
        {
            if (curModule.endInd + 1 == text.Length ||
                text[curModule.endInd - 1] != ' ')
            {
                return true;
            }

            return false;
        }

        private void CorrectBoldInclusion(Token curModule)
        {
            openModules.TryPeek(out var lastModule);
            verifiedModules.TryPeek(out var lastClosedMod);

            if (curModule.modType == Mod.Italic && lastClosedMod.modType == Mod.Bold)
            {
                var first = verifiedModules.Pop();
                var second = verifiedModules.Pop();

                first.modType = Mod.Common;
                first.Close();
                second.modType = Mod.Common;
            }
        }
       
        private void FilterDigitsAndSlashes()
        {
            foreach (var token in preTokens)
            {
                if (token.modType == Mod.Bold || token.modType == Mod.Italic)
                {
                    var startInd = token.startInd;
                    var endInd = token.endInd;

                    if ((startInd > 0 && char.IsDigit(text[startInd - 1])) ||
                        (endInd + 1 < text.Length && char.IsDigit(text[endInd + 1])))
                    {
                        token.modType = Mod.Common;
                        token.Close();
                    }
                }

                if (token.modType == Mod.Slash)
                {
                    token.modType = Mod.Common;
                    token.Close();
                }
            }
        }
    }
}
