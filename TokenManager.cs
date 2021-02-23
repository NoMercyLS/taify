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
                {"if", "Keyword" },
                {"while", "Keyword" },
                {"for", "Keyword" },
                {"int", "Keyword" },
                {"float", "Keyword" },
                {"write", "Keyword" },
                {"read", "Keyword" },
            };
            _tokens = tokensDict;
        }

        public string GetToken(string key)
        {
            return _tokens[key];
        }

    }
}
