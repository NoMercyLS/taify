using System;
using System.Collections.Generic;
using System.Text;

namespace LexicalAnalyzer
{
    public class Analyzer
    {
        private FileManager _fileManager;
        private Dictionary<string, string> _lexemas;
        public Analyzer(FileManager manager)
        {
            _fileManager = manager;
            _lexemas = new Dictionary<string, string>
            {
                {"if", "keyword" },
                {"while", "keyword" },
                {"for", "keyword" },
                {"int", "keyword" },
                {"float", "keyword" },
                {"bool", "keyword" },
                {"write", "keyword" },
                {"read", "keyword" },
                {";", "separator" },
                {"=", "operator" },
                {"+", "operator" },
                {"-", "operator" },
                {"*", "operator" },
                {"/", "operator" },
                {"div", "operator" },
                {"mod", "operator" },
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
                {"<=", "comparison" }
            };
        }

        private static bool IsIdentifier(string word)
        {
            foreach (char ch in word)
            {
                if (word[0] == ch && Char.IsDigit(ch))
                {
                    return false;
                }
                if (!Char.IsLetterOrDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }

        public void Analyze()
        {
            string line;
            int verticalPos = 0;
            while ((line = _fileManager.ReadLine()) != null)
            {
                verticalPos++;
                FindLexema(line, verticalPos);
            }
        }

        public void AcceptToken(string word, int verticalPosition, int horizontalPosition)
        {
            if (_lexemas.ContainsKey(word))
            {
                _fileManager.WriteLine($"Word: {word} | Token: " +
                    $"{_lexemas[word]} | Position: line {verticalPosition} " +
                    $"in position {horizontalPosition - word.Length}");
            }
            else
            {
                if (IsIdentifier(word))
                {
                    _fileManager.WriteLine($"Word: {word} | Token: Identifier | " +
                        $"Position: line {verticalPosition} " +
                        $"in position {horizontalPosition - word.Length}");
                }
                else
                {
                    _fileManager.WriteLine($"Word: {word} | Token: Error | " +
                        $"Position: line {verticalPosition} " +
                        $"in position {horizontalPosition - word.Length}");
                }
            }
        }

        private void FindLexema(string line, int verticalPosition)
        {
            _fileManager.WriteLine($"Analyzed line: '{line}'");
            string word = "";
            int horizontalPos = 0;
            foreach (char ch in line)
            {
                horizontalPos++;
                if (ch != ' ')
                {
                    word += ch;
                }
                else
                {
                    if (ch == ';')
                    {
                        AcceptToken(word, verticalPosition, horizontalPos);
                        AcceptToken(ch.ToString(), verticalPosition, horizontalPos);
                    }
                    else
                    {
                        AcceptToken(word, verticalPosition, horizontalPos);
                        word = "";
                    }
                }
            }
        }
    }
}
