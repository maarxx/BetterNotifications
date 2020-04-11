using HarmonyLib;
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
            ModHandler.InitSettings();

            // Do patches
            var harmony = new Harmony("krafs.BetterNotifications");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        internal static void InitLetterSettings()
        {
            foreach (LetterDef def in DefDatabase<LetterDef>.AllDefs)
                LetterSets.Add(def.defName, new LetterSet(def));
        }

        internal static Dictionary<string, LetterSet> LetterSets = new Dictionary<string, LetterSet>();

        internal static int AlertTime => Settings.alertTime;
        internal static int LetterTime => Settings.letterTime;

        internal static bool LetterSetting(LetterDef def)
        {
            return LetterSets[def.defName].active;
        }
    }
}