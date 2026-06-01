using Verse;

namespace DefensiveStances
{
    public static class DS_Log
    {
        private const string Prefix = "<color=#7CC576>[Defensive Stances]</color>";

        public static void Message(string message)
        {
            Log.Message(Prefix + " " + message);
        }

        public static void Warning(string message)
        {
            Log.Warning(Prefix + " " + message);
        }

        public static void Error(string message)
        {
            Log.Error(Prefix + " " + message);
        }

        public static void WarningOnce(string message, int key)
        {
            Log.WarningOnce(Prefix + " " + message, key);
        }

        public static void ErrorOnce(string message, int key)
        {
            Log.ErrorOnce(Prefix + " " + message, key);
        }
    }
}
