namespace BotLibrary
{

    static class Debug
    {
        public static bool PRINT_TO_CONSOLE = true;

        public static void Log(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}