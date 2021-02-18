namespace LexicalAnalyzer.State
{
    //TODO: Add new state COMPARISON
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
        Delimiter,
        Keyword,
        Final,
        Error
    }
}
