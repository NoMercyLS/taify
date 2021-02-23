using System.Collections.Generic;
using System.Linq;

namespace LexicalAnalyzer
{
    public class CharManager
    {
        private readonly List<char> _digits = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private readonly List<char> _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        private readonly List<char> _delimiters = new List<char> { '[', ']', '(', ')', '{', '}', ' ' };
        private readonly List<char> _operators = new List<char> { '+', '-', '*', '/', '=' };
        private readonly List<char> _comparisons = new List<char> { '<', '>', '!', '|', '&' };
        private readonly List<char> _banned = new List<char> { '@', '#', '"', '%', '^', '_', '~', ',', '\'', '?' };

        public bool IsDigit(char ch)
        {
            return _digits.Contains(ch);
        }

        public bool IsLetter(char ch)
        {
            return _alphabet.Contains(ch);
        }

        public bool IsDelimiter(char ch)
        {
            return _delimiters.Contains(ch);
        }

        public bool IsOperator(char ch)
        {
            return _operators.Contains(ch);
        }

        public bool IsComparison(char ch)
        {
            return _comparisons.Contains(ch);
        }

        public bool IsSeparator(char ch)
        {
            return ch == ';';
        }

        public bool IsError(char ch)
        {
            return _banned.Contains(ch);
        }
    }
}
