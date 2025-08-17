using Raylib_cs;

public static class Resources
{
    public static int windowWidth = 800;
    public static int windowHeight = 600;
    public static Font font = Raylib.LoadFont("IBMPlexSerif-Regular.ttf");
    // the non ex font size is set at 32 px
    public static int defFontSize = 32;
    public static int defSpacing = 0;
    public static List<WordUnit> allWords;
}