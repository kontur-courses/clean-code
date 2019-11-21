using System;
using System.Collections.Generic;

namespace Markdown.MdTokens
{
    public class MdTokenizerConfig
    { 
        private Dictionary<string, MdTokenSpec> symbols;
        private string shieldingSymbol;

        public MdTokenizerConfig()
        {
            symbols = new Dictionary<string, MdTokenSpec>();
        }

        public void AddSpecialSymbol(string symbol, bool isPaired, bool canBeNested)
        {
            if(symbol is null) throw new ArgumentNullException(nameof(symbol));
            var spec = new MdTokenSpec(symbol, isPaired, canBeNested);
            symbols[symbol] = spec;
        }

        public bool IsSymbolPaired(string symbol)
        {
            if (symbols.ContainsKey(symbol)) return symbols[symbol].IsPaired;
            throw new ArgumentException("No symbol found");
        }

        public bool CanSymbolBeNested(string symbol)
        {
            if (symbols.ContainsKey(symbol)) return symbols[symbol].CanBeNested;
            throw new ArgumentException("No symbol found");
        }

        public bool HasSpecialSymbol(string symbol)
        {
            return symbols.ContainsKey(symbol);
        }

        public void SetShieldingSymbol(string symbol)
        {
            shieldingSymbol = symbol ?? throw new ArgumentNullException();
        }

        public string GetShieldingSymbol()
        {
            return shieldingSymbol;
        }

        public void AddNestingExceptionForSymbol(string symbol, string exception)
        {
            if (!symbols.ContainsKey(symbol)) throw new ArgumentException();
            symbols[symbol].AddNestingExceptions(exception);
        }
        
        public bool IsSymbolNestingException(string symbol, string isException)
        {
            if (symbols.ContainsKey(symbol)) return symbols[symbol].NestingExceptions.Contains(isException);
            throw new ArgumentException();
        }

        public static MdTokenizerConfig DefaultConfig()
        {
            var config = new MdTokenizerConfig();
            config.AddSpecialSymbol("_", true, true);
            config.AddSpecialSymbol("__", true, true);
            config.AddSpecialSymbol("NONE", false, true);
            config.AddNestingExceptionForSymbol("__", "_");
            config.SetShieldingSymbol(@"\");
            return config;
        }
    }
}