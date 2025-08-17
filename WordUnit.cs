using System.Numerics;
using Raylib_cs;

class WordUnit
{
    private List<Letter> targetWord;
    private List<Letter> typedPortion;
    private List<Letter> untypedPortion;
    // think about it..
    public Vector2 dimensions;
    private int cursorIdx;
    private int wordLength;
    public WordUnit(List<char> word)
    {
        this.targetWord = new List<Letter>();
        this.untypedPortion = new List<Letter>();
        this.typedPortion = new List<Letter>();
        word.ForEach(delegate (char c)
        {
            this.targetWord.Add(new Letter(c, Color.Black));
            this.untypedPortion.Add(new Letter(c, Color.Black));
        });
        dimensions = CalcDimensions(targetWord);
        this.wordLength = 0;
        this.cursorIdx = 0;
    }

    private Vector2 CalcDimensions(List<Letter> word)
    {
        float width = 0;
        float height = 0;
        foreach (Letter l in word)
        {
            width += l.dimensons.X;
            if (height < l.dimensons.Y)
            {
                height = l.dimensons.Y;
            }
        }
        return new Vector2(width, height);
    }

    public void DrawWord(int x, int y)
    {
        throw new NotImplementedException();
    }
}

internal class Letter
{
    public char letter;
    public Color color;
    public Vector2 dimensons;
    public Letter(char letter, Color color)
    {
        this.letter = letter;
        this.color = color;
        this.dimensons = CalcDimensions();
    }
    private Vector2 CalcDimensions()
    {
        return Raylib.MeasureTextEx(Resources.font, this.letter.ToString(), Resources.defFontSize, Resources.defSpacing);
    }
}