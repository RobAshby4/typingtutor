using System.ComponentModel.DataAnnotations;
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
    [STAThread]
    public static void Main()
    {
        // init window
        Raylib.InitWindow(windowWidth, windowHeight, "Typing Tutor");

        // init fonts and font options.
        Font typingFont = Raylib.LoadFont("IBMPlexSerif-Regular.ttf");
        int fontSize = 32;
        int spacing = 0;

        while (!Raylib.WindowShouldClose())
        {
            // start rendering
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.RayWhite);

            // if you havent started, show instructions
            if (!started)
            {
                showInstructions(typingFont, fontSize, spacing);
                unstartedLoop(typingFont, fontSize, spacing);
            }
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private static void unstartedLoop(Font font, int fontSize, int spacing)
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
                    generator.updateLetters(practiceChars);
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(generator.yieldWord());
                    }
                }
            }
            keycode = Raylib.GetKeyPressed();
        }
        string practiceString = new string(practiceChars.ToArray());
        Vector2 offset = CalculateTextCenterOffset(practiceString, font, fontSize, spacing);
        Raylib.DrawTextEx(font, practiceString, offset, fontSize, spacing, Color.Black);
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
        this.wordList = Words.wordList;
        this.trainingSet = new List<string>();
        WordGenerator.idx = 0;
        updateGenerator();
    }

    public void updateLetters(List<char> letters)
    {
        trainingSet.Clear();
        this.letters = letters;
        updateGenerator();
    }
    void updateGenerator()
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
        Random rand = new Random();
        string[] trainingArray = trainingSet.ToArray();
        rand.Shuffle<string>(trainingArray);
        trainingSet.Clear();
        foreach (string s in trainingArray)
        {
            trainingSet.Add(s);
        }
    }

    public string? yieldWord()
    {
        // if there is a word to yield, yield it. otherwise return null
        if (trainingSet.Count > 0)
        {
            return trainingSet[idx++ % trainingSet.Count];
        }
        return null;
    }
}