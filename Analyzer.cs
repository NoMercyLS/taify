using System;
using System.Collections.Generic;

namespace LexicalAnalyzer
{
    public class Analyzer
    {
        private FileManager _fileManager;
        private TokenManager _tokenManager;
        private CharManager _charManager;
        private List<string> _lines;
        private int _verticalPos;
        private int _horizontalPos;
        private State.State _state;

        public Analyzer(FileManager manager)
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

        public char GetChar()
        {
            char ch = _lines[_verticalPos][_horizontalPos];
            //TODO: Add new state which drops current state on readln
            if (_horizontalPos == _lines[_verticalPos].Length - 1)
            {
                _horizontalPos = 0;
                if (_verticalPos <= _lines.Count - 1)
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

        public void WriteLine(string word)
        {
            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
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
                            if (_charManager.IsLetter(ch))
                            {
                                _state = State.State.Identifier;
                                word += ch;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                _state = State.State.Number;
                                word += ch;
                                break;
                            }
                            if (_charManager.IsSeparator(ch))
                            {
                                _state = State.State.Separator;
                                break;
                            }
                            if (_charManager.IsError(ch))
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _state = State.State.Operator;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Delimiter;
                                break;
                            }
                            break;
                        //TODO: After accept identifier needs to get word token,
                        //if token doesn't exists return Identifier token
                        case State.State.Identifier:
                            ch = GetChar();
                            if (_charManager.IsDigit(ch) || _charManager.IsLetter(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ' ')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                word = "";
                                _state = State.State.Wait;
                            }
                            if (ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            break;
                        case State.State.Number:
                            ch = GetChar();
                            if (_charManager.IsDigit(ch))
                            {
                                if (word[0] == '0' && word.Length == 1 && _charManager.IsDigit(ch))
                                {
                                    _state = State.State.OctalInteger;
                                }
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                word += ch;
                                if (ch == 'b' || word[^1] == 0)
                                {
                                    _state = State.State.BinaryInteger;
                                    break;
                                }
                                if (ch == 'x' || word[^1] == 0)
                                {
                                    _state = State.State.HexInteger;
                                    break;
                                }
                                _state = State.State.Error;
                                break;
                            }
                            if (ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (ch == '.')
                            {
                                word += ch;
                                _state = State.State.Float;
                                break;
                            }
                            if (ch == ' ' || _charManager.IsDelimiter(ch) || _charManager.IsOperator(ch))
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            if (ch == ';')
                            {
                                _state = State.State.Integer;
                                break;
                            }
                            break;
                        case State.State.Float:
                            ch = GetChar();
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch) || ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                            }
                            if (ch == ' ')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                word = "";
                                _state = State.State.Wait;
                            }
                            break;
                        case State.State.Integer:
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch) || ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ' ')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                word = "";
                                _state = State.State.Wait;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                            }
                            break;
                        case State.State.BinaryInteger:
                            ch = GetChar();
                            if (ch == '0' || ch == '1')
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch) || ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                            }
                            break;
                        case State.State.OctalInteger:
                            ch = GetChar();
                            if ("01234567".Contains(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch) || ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                            }
                            break;
                        case State.State.HexInteger:
                            ch = GetChar();
                            if (ch == '@')
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (_charManager.IsOperator(ch))
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _horizontalPos, _verticalPos));
                                _state = State.State.Operator;
                                break;
                            }
                            if (ch == ';')
                            {
                                _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                                _state = State.State.Separator;
                                break;
                            }
                            if (_charManager.IsDigit(ch))
                            {
                                word += ch;
                                break;
                            }
                            if (_charManager.IsLetter(ch) && "abcdefABCDEF".Contains(ch))
                            {
                                word += ch;
                                break;
                            }
                            else
                            {
                                word += ch;
                                _state = State.State.Error;
                                break;
                            }
                        case State.State.Commentary:
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            _horizontalPos = 0;
                            _verticalPos++;
                            _state = State.State.Wait;
                            break;
                        case State.State.Operator:
                            word = "";
                            word += ch;
                            buff = GetChar();
                            if (ch == buff)
                            {
                                if (ch == '/')
                                {
                                    word += ch;
                                    _state = State.State.Commentary;
                                    break;
                                }
                                if (ch == '=')
                                {
                                    word += ch;
                                    _state = State.State.Comparison;
                                    break;
                                }
                            }
                            if (_charManager.IsDigit(buff) && (ch == '+' || ch == '-'))
                            {
                                word += buff;
                                _state = State.State.Number;
                                break;
                            }
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            ch = buff;
                            word += ch;
                            if (_charManager.IsDigit(ch))
                            {
                                _state = State.State.Number;
                                break;
                            }
                            if (_charManager.IsComparison(ch))
                            {
                                _state = State.State.Comparison;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                _state = State.State.Identifier;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (ch == '@')
                            {
                                _state = State.State.Error;
                                break;
                            }
                            if (ch == ' ')
                            {
                                _state = State.State.Wait;
                                word = "";
                                break;
                            }
                            if (ch == ';')
                            {
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Separator:
                            word = "";
                            word += ch;
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            _state = State.State.Wait;
                            break;
                        case State.State.Delimiter:
                            word = "";
                            word += ch;
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            _state = State.State.Wait;
                            break;
                        case State.State.Comparison:
                            word = "";
                            word += ch;
                            buff = GetChar();
                            if (ch == '!')
                            {
                                if (buff == '=')
                                {
                                    word += buff;
                                }
                                else
                                {
                                    word += buff;
                                    _state = State.State.Error;
                                    break;
                                }
                            }
                            if (ch == '=' || ch == '|' || ch == '&')
                            {
                                if (ch == buff)
                                {
                                    word += buff;
                                }
                                else if (ch == '=')
                                {
                                    ch = buff;
                                    _state = State.State.Operator;
                                    break;
                                }
                                else
                                {
                                    word += buff;
                                    _state = State.State.Error;
                                    break;
                                }
                            }
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            ch = buff;
                            word += ch;
                            if (_charManager.IsDigit(ch))
                            {
                                _state = State.State.Number;
                                break;
                            }
                            if (_charManager.IsLetter(ch))
                            {
                                _state = State.State.Identifier;
                                break;
                            }
                            if (_charManager.IsDelimiter(ch))
                            {
                                _state = State.State.Delimiter;
                                break;
                            }
                            if (ch == '@')
                            {
                                _state = State.State.Error;
                                break;
                            }
                            if (ch == ' ')
                            {
                                _state = State.State.Wait;
                                word = "";
                                break;
                            }
                            if (ch == ';')
                            {
                                _state = State.State.Separator;
                                break;
                            }
                            break;
                        case State.State.Keyword:
                            break;
                        case State.State.Error:
                            _fileManager.WriteLine(FileManager.GetOutputString(word, _state, _verticalPos, _horizontalPos));
                            word = "";
                            _state = State.State.Wait;
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    _state = State.State.Final;
                }
            }
        }
    }
}
