using System.Numerics;
using Raylib_cs;

class WordUnit
{
    private List<char> word;
    private List<char> typedWord;
    // think about it..
    public Vector2 dimensions;
    private int cursorIdx;
    public WordUnit(List<char> word)
    {
        this.word = word;
        typedWord = new List<char>();
        dimensions = CalcDimensions(word);
    }

    private Vector2 CalcDimensions(List<char> word)
    {
        return Raylib.MeasureTextEx(Resources.font, word.ToArray().ToString(), Resources.defFontSize, Resources.defSpacing);
    }

    public void DrawWord(int x, int y)
    {
        throw new NotImplementedException();
    }
}
