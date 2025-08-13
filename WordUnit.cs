using System.Numerics;
using Raylib_cs;

class WordUnit
{
    private List<char> word;
    private List<char> typedWord;
    public Vector2 dimensions;
    public WordUnit(List<char> word)
    {
        this.word = word;
        typedWord = new List<char>();
        dimensions = CalculateWordDimensions(word);
    }

    private Vector2 CalculateWordDimensions(List<char> word)
    {
        throw new NotImplementedException();
    }
}
