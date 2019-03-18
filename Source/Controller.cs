using Harmony;
using System;
using System.Reflection;
using Verse;

namespace BetterNotifications
{
    [StaticConstructorOnStartup]
    class Controller
    {
        static Controller()
        {
            int startTime = Environment.TickCount;

            DebugUtil.Log("Better Notifications!");

            // Do patches
            HarmonyInstance harmony = HarmonyInstance.Create("BetterNotifications");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            DebugUtil.Log("STARTUP TIME: " + (Environment.TickCount - startTime).ToString() + "ms");
        }
    }
}