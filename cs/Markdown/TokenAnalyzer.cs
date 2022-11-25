using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenAnalyzer
    {
        private string text;
        private Stack<Token> openModules;
        private Stack<Token> verifiedModules;
        private List<Token> preTokens;
        private Token linkNameToken;

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

                if (curModule.modType == Mod.LinkName)
                {
                    linkNameToken = curModule;
                }
                else if (curModule.modType == Mod.LinkUrl)
                {
                    if (linkNameToken == null) curModule.CastToCommonType();
                    else linkNameToken = null;
                }
                else if (curModule.modType != Mod.Common)
                {
                    ValidConcatination(curModule);
                }
            }

            if (linkNameToken != null) linkNameToken.CastToCommonType();

            SolveUnverifiedModules();
            return preTokens;
        }

        private void ValidConcatination(Token curModule)
        {
            openModules.TryPeek(out var lastModule);

            if (lastModule != null && lastModule.modType == curModule.modType)
            {
                HandingConcatinationClosure(lastModule, curModule);
            }
            else
            {
                HandingConcatinationOpen(curModule);
            }
        }

        private void HandingConcatinationOpen(Token curModule)
        {
            if (IsOpeningModule(curModule) || IsModuleInWord(curModule))
            {
                verifiedModules.Push(curModule);
                openModules.Push(curModule);
            }
            else
            {
                CorrectIntersect(curModule);
                curModule.CastToCommonType();
            }
        }

        private void HandingConcatinationClosure(Token lastModule, Token curModule)
        {
            if (curModule.StartInd - lastModule.EndInd == 1)
            {
                lastModule.CastToCommonType();
                curModule.CastToCommonType();
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
                curModule.CastToCommonType();
            }
            else if (IsClosingModule(curModule))
            {
                CorrectBoldInclusion(curModule);
                curModule.Close();
                openModules.Pop();
                verifiedModules.Push(curModule);
            }
            else
            {
                curModule.CastToCommonType();
            }
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
                    preLastMod.CastToCommonType();
                    lastMod.CastToCommonType();
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
                    module.CastToCommonType();
                }
            }

            if (isStackHasOpenTitle)
            {
                verifiedModules.TryPeek(out var lastModule);
                var closingToken = new Token(lastModule.EndInd + 1,
                    lastModule.EndInd + 1,
                    Mod.Title,
                    false);
                verifiedModules.Push(closingToken);
                preTokens.Add(closingToken);
            }
        }

        private bool IsModuleInWord(Token module)
        {
            if ((module.StartInd > 0 && text[module.StartInd - 1] != ' ') && 
                (module.EndInd + 1 < text.Length && text[module.EndInd + 1] != ' '))
            {
                return true;
            }

            return false;
        }

        private bool IsModulesInSameWord(Token first, Token second)
        {
            var subString = text.Substring(first.EndInd, second.StartInd - first.EndInd);

            if (subString.Contains(' '))
            {
                return false;
            }

            return true;
        }

        private bool IsOpeningModule(Token curModule)
        {
            if (curModule.StartInd == 0 || 
                text[curModule.StartInd - 1] == ' ')
            {
                return true;
            }

            return false;
        }

        private bool IsClosingModule(Token curModule)
        {
            if (curModule.EndInd + 1 == text.Length ||
                text[curModule.EndInd - 1] != ' ')
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

                first.CastToCommonType();
                second.CastToCommonType();
            }
        }
       
        private void FilterDigitsAndSlashes()
        {
            foreach (var token in preTokens)
            {
                if (token.modType == Mod.Bold || token.modType == Mod.Italic)
                {
                    var startInd = token.StartInd;
                    var endInd = token.EndInd;

                    if ((startInd > 0 && char.IsDigit(text[startInd - 1])) ||
                        (endInd + 1 < text.Length && char.IsDigit(text[endInd + 1])))
                    {
                        token.CastToCommonType();
                    }
                }

                if (token.modType == Mod.Slash)
                {
                    token.CastToCommonType();
                }
            }
        }
    }
}
