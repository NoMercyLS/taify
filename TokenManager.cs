using System.Collections.Generic;

namespace LexicalAnalyzer
{
    public class TokenManager
    {
        private readonly Dictionary<string, string> _tokens;

        public TokenManager()
        {
            Dictionary<string, string> tokensDict = new Dictionary<string, string>
            {
                {"if", "keyword" },
                {"while", "keyword" },
                {"for", "keyword" },
                {"int", "keyword" },
                {"float", "keyword" },
                {"write", "keyword" },
                {"read", "keyword" },
                {";", "separator" },
                {"=", "operator" },
                {"+", "operator" },
                {"-", "operator" },
                {"*", "operator" },
                {"/", "operator" },
                {"//", "commentary" },
                {"{", "brace" },
                {"}", "brace" },
                {"(", "bracket" },
                {")", "bracket" },
                {"[", "bracket" },
                {"]", "bracket" },
                {"<", "comparison" },
                {">", "comparison" },
                {"==", "comparison" },
                {"!=", "comparison" },
                {"&&", "comparison" },
                {"||", "comparison" },
                {">=", "comparison" },
                {"<=", "comparison" },
            };
            _tokens = tokensDict;
        }

        public string GetToken(string key)
        {
            return _tokens[key];
        }

    }
}
