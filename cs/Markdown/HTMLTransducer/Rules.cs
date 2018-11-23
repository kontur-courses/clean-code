using System.Collections.Generic;
using System.Linq;

namespace Markdown.HTMLTransducer
{
    public class Rules
    {
        private readonly List<Rule> rules;
        private readonly List<ProhibitInheritTokenRule> prohibitInheritTokenRules;
        private readonly HashSet<Token> escapeTokens;

        public Rules(List<Rule> rules = null, HashSet<Token> escapeTokens = null,
            List<ProhibitInheritTokenRule> prohibitInheritTokenRules = null)
        {
            this.rules = rules ?? new List<Rule>();
            this.escapeTokens = escapeTokens ?? new HashSet<Token>();
            this.prohibitInheritTokenRules = prohibitInheritTokenRules
                ?? new List<ProhibitInheritTokenRule>();
        }

        public void AddRule(Rule rule) =>
            rules.Add(rule);

        public void AddEscapeToken(Token token) =>
            escapeTokens.Add(token);

        public bool IsEscape(Token token) =>
            escapeTokens.Contains(token);

        public bool ContainsRuleFor(Token token) =>
            rules.Any(r => r.For(token));

        public bool ContainsProhibitInheritRuleFor(
            Token parentToken, Token childToken) =>
            prohibitInheritTokenRules.Any(r => r.For(parentToken, childToken));

        public Token PerformFor(Token token, bool isClosed) =>
            rules.First(r => r.For(token)).Perform(token, isClosed);
    }
}