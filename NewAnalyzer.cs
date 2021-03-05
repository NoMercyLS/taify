using System;
using System.Collections.Generic;

namespace LexicalAnalyzer
{
    public class NewAnalyzer
    {
        private FileManager _fileManager;
        private TokenManager _tokenManager;
        private CharManager _charManager;
        private List<string> _lines;
        private int _verticalPos;
        private int _horizontalPos;
        private State.State _state;

        public NewAnalyzer(FileManager manager)
        {
            _fileManager = manager;
            _tokenManager = new TokenManager();
            _charManager = new CharManager();
            _verticalPos = 0;
            _horizontalPos = 0;
            _state = State.State.Wait;
            _lines = new List<string>();
        }

        private void GetLines()
        {
            string line;
            while ((line = _fileManager.ReadLine()) != null)
            {
                if (line.Length != 0)
                {
                    _lines.Add(line);
                }
            }
        }

        private char GetChar()
        {
            char ch = _lines[_verticalPos][_horizontalPos];
            //Console.WriteLine($"DEBUG (state = {_state}");
            //Console.WriteLine($"DEBUG (ch = {ch}");
            if (_horizontalPos == _lines[_verticalPos].Length - 1)
            {
                _horizontalPos = 0;
                if (_verticalPos < _lines.Count)
                {
                    _verticalPos++;
                }
                else
                {
                    _fileManager.WriteLine("End of file!");
                    throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _horizontalPos++;
            }
            return ch;
        }

        private void WriteLine(string word)
        {
            _fileManager.WriteLine(_verticalPos > 0
                ? FileManager.GetOutputString(word, _state, _verticalPos + 1, _horizontalPos)
                : FileManager.GetOutputString(word, _state, _verticalPos + 1, _horizontalPos - 1));
        }

        private void GetKeyword(string word)
        {
            try
            {
                _state = State.State.Keyword;
                string token = _tokenManager.GetToken(word);
                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos + 1, _horizontalPos - 1));
            }
            catch (Exception)
            {
                _state = State.State.Identifier;
                WriteLine(word);

            }
        }

        public void Analyze()
        {
            GetLines();
            string word = "";
            char ch = ' ';
            char buff = ' ';
            while (_state != State.State.Final)
            {
                try
                {
                    switch (_state)
                    {
                        case State.State.Wait:
                            ch = GetChar();
                            word += ch;
                            if (_charManager.IsComparison(ch))
                            {
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                _state = State.State.Number;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                _state = State.State.Identifier;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Identifier:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                GetKeyword(word);
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                GetKeyword(word);
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                GetKeyword(word);
                                word = "";
                                word += ch;
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                GetKeyword(word);
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Number:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                if (word[0] == '0' && "01234567".Contains(ch))
                                {
                                    _state = State.State.OctalInteger;
                                }
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                if (word.Length == 1 && word[0] == '0')
                                {
                                    if (ch == 'b')
                                    {
                                        word += ch;
                                        _state = State.State.BinaryInteger;
                                        break;
                                    }
                                    if (ch == 'x')
                                    {
                                        word += ch;
                                        _state = State.State.HexInteger;
                                        break;
                                    }
                                }
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            if (ch == '.')
                            {
                                word += ch;
                                _state = State.State.Float;
                            }
                            break;
                        case State.State.Float:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Integer:
                            WriteLine(word);
                            word = "";
                            word += ch;
                            if (_charManager.IsComparison(ch))
                            {
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsError(ch) || _charManager.IsLetter(ch))
                            {
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.BinaryInteger:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                if (!"01".Contains(ch))
                                {
                                    _state = State.State.Error;
                                }
                                break;
                            }
                            if (_charManager.IsError(ch) || _charManager.IsLetter(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.OctalInteger:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                if (!"01234567".Contains(ch))
                                {
                                    _state = State.State.Error;
                                }
                                break;
                            }
                            if (_charManager.IsError(ch) || _charManager.IsLetter(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.HexInteger:
                            ch = GetChar();
                            if (_charManager.IsComparison(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                word += ch;
                                if (!"ABCDEFabcdef".Contains(ch))
                                {
                                    _state = State.State.Error;
                                    break;
                                }
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                WriteLine(word);
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Commentary:
                            WriteLine(word);
                            word = "";
                            _state = State.State.Wait;
                            _horizontalPos = 0;
                            _verticalPos++;
                            break;
                        case State.State.Operator:
                            if (ch == '=')
                            {
                                buff = GetChar();
                                if (buff == ch)
                                {
                                    word += buff;
                                    _state = State.State.Comparison;
                                    buff = ' ';
                                    break;
                                }
                            }
                            if (ch == '/')
                            {
                                buff = GetChar();
                                if (buff == ch)
                                {
                                    word += buff;
                                    _state = State.State.Commentary;
                                    buff = ' ';
                                    break;
                                }
                            }
                            WriteLine(word);
                            ch = buff;
                            if (_charManager.IsComparison(ch))
                            {
                                word = "";
                                word += ch;
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                word = "";
                                word += ch;
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                word = "";
                                word += ch;
                                _state = State.State.Identifier;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                word = "";
                                word += ch;
                                _state = State.State.Separator;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word = "";
                                word += ch;
                                _state = State.State.Number;
                            }
                            break;
                        case State.State.Separator:
                            WriteLine(word);
                            word = "";
                            _state = State.State.Wait;
                            break;
                        case State.State.Delimiter:
                            if (ch == ' ')
                            {
                                word = "";
                                _state = State.State.Wait;
                                break;
                            }
                            WriteLine(word);
                            word = "";
                            _state = State.State.Wait;
                            break;
                        case State.State.Comparison:
                            if (ch == '!' || ch == '<' || ch == '>')
                            {
                                buff = GetChar();
                                if (buff == '=')
                                {
                                    word += buff;
                                }
                            }
                            WriteLine(word);
                            word = "";
                            buff = ' ';
                            _state = State.State.Wait;
                            break;
                        case State.State.Error:
                            WriteLine(word);
                            word = "";
                            _state = State.State.Wait;
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (_state != State.State.Wait)
                    {
                        WriteLine(word);
                    }
                    _state = State.State.Final;
                    WriteLine("");
                }
            }
        }
    }
}
