//using Harmony;
//using System.Collections.Generic;
//using System.Linq;
//using Toggles.Source;
//using Toggles.Source.Patches;
//using UnityEngine;
//using Verse;

//namespace BetterNotifications
//{
//    // Toggles all letter on the HUD.
//    [HarmonyPatch(typeof(Letter))]
//    [HarmonyPatch("CanShowInLetterStack", MethodType.Getter)]
//    class Letter_Patch : MethodPatch
//    {
//        static bool Prepare()
//        {
//            return Prepare(typeof(Letter_Patch));
//        }
//        static void InitToggles()
//        {
//            foreach (LetterDef def in DefDatabase<LetterDef>.AllDefs)
//            {
//                ToggleManager.Add(
//                    label: StringUtil.CleanDefLabel(def),
//                    id: Format(def.defName),
//                    description: def.description,
//                    texture: def.Icon,
//                    color: def.color,
//                    root: Labels.Notifications,
//                    category: Labels.Letters,
//                    subCategory: Labels.ByCategory
//                    );
//            }
//        }

//        // Holds all letters added by player
//        internal static List<string> customLetters;

//        internal static List<string> CustomLetters
//        {
//            get => customLetters ?? (CustomLetters = new List<string>());
//            set => customLetters = value;
//        }

//        // Remove all toggles based on custom letters.
//        internal static void RemoveCustomLetters() =>
//            CustomLetters
//                .ForEach(x => ToggleManager.Remove(Format(x)));

//        // Adjusts strings to mod formats.
//        static string Format(string letter) => letter;

//        // Creates toggle from unmodified letter label.
//        internal static void AddRawLetter(string letter)
//        {
//            CustomLetters.Add(letter);
//            LetterToToggle(Format(letter));
//        }

//        // Makes sure all player added letters exist as toggles.
//        internal static void UpdateCustomLetters() =>
//            CustomLetters
//            .Where(letter => !ToggleManager.Exists(Format(letter))).ToList()
//            .ForEach(letter => LetterToToggle(letter));

//        // Creates a toggle from a sanitized letter label.
//        static void LetterToToggle(string letter) =>
//            ToggleManager.Add(
//                label: Format(letter),
//                id: letter,
//                description: "NO DESCRIPTION",
//                texture: Tex.TexEmpty,
//                color: Color.white,
//                root: Labels.Notifications,
//                category: Labels.Letters,
//                subCategory: Labels.ByLabel
//                );

//        // Replaces result if letter exists in settings. Prioritizes individual letters above letterDefs.
//        static void Postfix(ref Letter __instance, ref bool __result)
//        {
//            string label = Format(__instance.label);
//            string defLabel = Format(__instance.def.defName);

//            if (ToggleManager.Exists(label))
//                __result = ToggleManager.IsActive(label) ? __result : false;
//            else if (ToggleManager.Exists(defLabel))
//                __result = ToggleManager.IsActive(defLabel) ? __result : false;
//        }

//        // Returns list of raw letter labels from games' world history.
//        internal static List<string> LoggedLetters =>
//            Find.Archive.ArchivablesListForReading
//                        .Where(x => x is Letter)
//                        .Select(z => z.ArchivedLabel)
//                        .Where(x => !ToggleManager.Exists(Format(x))).ToList();
//    }
//}