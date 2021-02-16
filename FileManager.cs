using System.IO;

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

        public string GetOutputString(string word, State.State state, int v, int h)
        {
            return $"Word: {word} | Token: {state} | Position {v}:{h}";
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
