namespace LexicalAnalyzer.State
{
    public enum State
    {
        Wait,
        Identifier,
        Number,
        Float,
        Integer,
        BinaryInteger,
        OctalInteger,
        HexInteger,
        Commentary,
        Operator,
        Separator,
        Delimeter,
        Keyword,
        Final,
        Error
    }
}
