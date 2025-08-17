using System.Numerics;
using Raylib_cs;

namespace HelloWorld;

class Program
{
    private static int windowHeight = 600;
    private static int windowWidth = 800;
    private static List<char> practiceChars = new List<char>();
    private static string[] strings = {
        "Type characters you wish to practice.",
        "Enter to start.",
        "Tab to reset."
    };
    private static bool started = false;
    private static WordGenerator generator = new WordGenerator(practiceChars);
    private static List<WordUnit> currentWords = new List<WordUnit>();

    [STAThread]
    public static void Main()
    {
        // init window
        Raylib.InitWindow(windowWidth, windowHeight, "Typing Tutor");


        while (!Raylib.WindowShouldClose())
        {
            // start rendering
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.RayWhite);

            // if you havent started, show instructions
            if (!started)
            {
                showInstructions(Resources.font, Resources.defFontSize, Resources.defSpacing);
                UnstartedLoop();
            }
            else
            {
                StartedLoop();
            }
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private static void StartedLoop()
    {
        int drawHeight = Resources.windowHeight / 2;
        int keycode = Raylib.GetKeyPressed();
        while (keycode != 0)
        {
            char? c = getCharacter(keycode);
            if (!(c is null))
            {
                
            }

        }

    }

    private static void UnstartedLoop()
    {
        int keycode = Raylib.GetKeyPressed();
        while (keycode != 0)
        {
            char? c = getCharacter(keycode);
            if (!(c is null))
            {
                practiceChars.Add(c.Value);
            }
            else
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && practiceChars.Count > 0)
                {
                    practiceChars.RemoveAt(practiceChars.Count - 1);
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Enter) && practiceChars.Count > 0)
                {
                    started = true;
                    generator.UpdateLetters(practiceChars);
                    for (int i = 0; i < 10; i++)
                    {
                        string? yieldedWord = generator.YieldWord();
                        if (yieldedWord is null)
                        {
                            throw new NullReferenceException();
                        }
                        WordUnit word = new WordUnit(yieldedWord.ToList());
                        Resources.wordList.Add(word);
                    }
                }
            }
            keycode = Raylib.GetKeyPressed();
        }
        string practiceString = new string(practiceChars.ToArray());
        Vector2 offset = CalculateTextCenterOffset(practiceString, Resources.font, Resources.defFontSize, Resources.defSpacing);
        Raylib.DrawTextEx(Resources.font, practiceString, offset, Resources.defFontSize, Resources.defSpacing, Color.Black);
    }

    private static void showInstructions(Font font, int fontSize, int spacing)
    {
        Vector2 textMiddle = Vector2.Zero;
        textMiddle = CalculateTextCenterOffset(strings[0], font, fontSize, spacing);
        Raylib.DrawTextEx(font, strings[0], new Vector2(textMiddle.X, 32), fontSize, spacing, Color.Gray);

        textMiddle = CalculateTextCenterOffset(strings[1], font, fontSize, spacing);
        Raylib.DrawTextEx(font, strings[1], new Vector2(textMiddle.X, 64), fontSize, spacing, Color.Gray);

        textMiddle = CalculateTextCenterOffset(strings[2], font, fontSize, spacing);
        Raylib.DrawTextEx(font, strings[2], new Vector2(textMiddle.X, 96), fontSize, spacing, Color.Gray);
    }

    private static char? getCharacter(int keycode)
    {
        if (keycode >= 65 && keycode <= 91)
        {
            if (!(Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift)))
            {
                keycode += 32;
            }
            return (char)keycode;
        }
        return null;
    }

  // Calculate the offset for centered text in the window.
    private static Vector2 CalculateTextCenterOffset(string text, Font font, int fontSize, int spacing)
    {
        Vector2 center = Vector2.Zero;
        if (text == null)
        {
            center.X = 0;
            center.Y = 0;
            return center;
        }
        Vector2 textMiddle = Raylib.MeasureTextEx(font, text, fontSize, spacing);
        center.X = (windowWidth / 2) - (textMiddle.X / 2);
        center.Y = (windowHeight / 2) - (textMiddle.Y / 2);
        return center;
    }
}

internal class WordGenerator
{
    private List<char> letters;
    private string[] wordList;
    private List<string> trainingSet;
    private static int idx;
    public WordGenerator(List<char> letters)
    {
        this.letters = letters;
        this.wordList = QualifiedWords.wordList;
        this.trainingSet = new List<string>();
        WordGenerator.idx = 0;
        UpdateGenerator();
    }

    public void UpdateLetters(List<char> letters)
    {
        trainingSet.Clear();
        this.letters = letters;
        UpdateGenerator();
    }
    void UpdateGenerator()
    {
        if (letters.Count == 0)
        {
            for (int i = 0; i < wordList.Length; i++)
            {
                trainingSet.Add(wordList[i]);
            }
        }
        else
        {
            for (int i = 0; i < wordList.Length; i++)
            {
                foreach (char c in letters)
                {
                    if (wordList[i].ToLower().Contains(c))
                    {
                        trainingSet.Add(wordList[i]);
                        break;
                    }
                }
            }
        }
        // We have to change to an array to use rand.Shuffle()
        Random rand = new Random();
        string[] trainingArray = trainingSet.ToArray();
        rand.Shuffle<string>(trainingArray);
        trainingSet.Clear();
        // Add back to trainingSet. not efficient. :(
        foreach (string s in trainingArray)
        {
            trainingSet.Add(s);
        }
    }

    void UpdateGenerator(string[] wordArray)
    {
        // for if you need to specifically set the list to some string
        for (int i = 0; i < wordArray.Length; i++)
        {
            trainingSet.Add(wordArray[i]);
        }
    }

    public string? YieldWord()
    {
        // if there is a word to yield, yield it. otherwise return null
        if (trainingSet.Count > 0)
        {
            return trainingSet[idx++ % trainingSet.Count];
        }
        return null;
    }
}