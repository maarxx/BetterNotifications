using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace BetterNotifications
{
    [StaticConstructorOnStartup]
    class Controller
    {
        static Controller()
        {
            InitLetterSettings();

            // Do patches
            HarmonyInstance harmony = HarmonyInstance.Create("BetterNotifications");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        static void InitLetterSettings()
        {
            foreach (LetterDef def in DefDatabase<LetterDef>.AllDefs)
                LetterSets.Add(new LetterSet(def));
        }

        internal static HashSet<LetterSet> LetterSets;

        internal static int AlertTime => Settings.alertTime;
        internal static int LetterTime => Settings.letterTime;
        internal static Dictionary<LetterDef, bool> LetterSettings => Settings.letterSettings;

        internal static bool LetterSetting(LetterDef def)
        {
            return LetterSets[def];
        }
    }
}