using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BetterNotifications
{
    internal class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            ThisMod = this;
        }

        static Mod ThisMod;

        static Settings Settings;
        internal static void InitSettings()
        {
            Settings = ThisMod.GetSettings<Settings>();
        }

        public override string SettingsCategory() => "BetterNotifications";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard view = new Listing_Standard();
            view.ColumnWidth = 200f;
            view.Begin(inRect);
            foreach (KeyValuePair<string, LetterSet> let in Controller.LetterSets)
            {
                view.CheckboxLabeled(CleanDefLabel(let.Value), ref let.Value.active);
            }

            view.Gap();

            int letterSleepHours = Settings.letterTime;
            view.Label("BN_LetterSliderLabel".Translate(letterSleepHours), tooltip: "BN_LetterSliderTooltip".Translate());
            Settings.letterTime = Mathf.RoundToInt(view.Slider((float)letterSleepHours, 1f, 24f));

            view.Gap();

            int alertSleepHours = Settings.alertTime;
            view.Label("BN_AlertSliderLabel".Translate(alertSleepHours), tooltip: "BN_AlertSliderTooltip".Translate());
            Settings.alertTime = Mathf.RoundToInt(view.Slider((float)alertSleepHours, 1f, 24f));

            view.End();
        }

        internal static string CleanDefLabel(LetterSet letterSet)
        {
            string label = "BN_" + letterSet.def.defName;
            if (label.CanTranslate())
                label = label.Translate();
            else
            {
                label = letterSet.DefName.Substring(letterSet.DefName.IndexOf("_") + 1);
                label = GenText.SplitCamelCase(label);
            }
            return label;
        }
    }
}