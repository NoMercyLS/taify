using System.Collections.Generic;
using System.Linq;
namespace LexicalAnalyzer
{
    public class CharManager
    {
        List<char> _digits = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        List<char> _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        List<char> _delimeters = new List<char> { '[', ']', '(', ')', '{', '}' };
        List<char> _operators = new List<char> { '+', '-', '*', '/' };

        public bool IsDigit(char ch)
        {
            return _digits.Contains(ch);
        }

        public bool IsLetter(char ch)
        {
            return _alphabet.Contains(ch);
        }

        public bool IsDelimeter(char ch)
        {
            return _delimeters.Contains(ch);
        }

        public bool IsOperator(char ch)
        {
            return _operators.Contains(ch);
        }
    }
}
