using System.Collections.Generic;

namespace Toggles
{
    // DEV DEBUG. Outputs identical log messages only once.
    internal static class DebugUtil
    {
        static List<string> LogTracker { get; } = new List<string>();

        internal static void Log(string str)
        {
            if (!LogTracker.Contains(str))
            {
                Verse.Log.Message(str);
                LogTracker.Add(str);
            }
        }
    }
}