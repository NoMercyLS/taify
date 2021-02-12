using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LexicalAnalyzer
{
    public class FileManager
    {
        private TextReader _inputStream;
        private TextWriter _outputStream;

        public FileManager(TextReader inputStream, TextWriter outputStream)
        {
            _inputStream = inputStream;
            _outputStream = outputStream;
        }

        public string ReadLine()
        {
            return _inputStream.ReadLine();
        }

        public void WriteLine(string text)
        {
            _outputStream.WriteLine(text);
        }
    }
}
