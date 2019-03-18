using Harmony;
using UnityEngine;
using Verse;

namespace Toggles
{
    internal class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            ThisMod = this;
        }

        internal static ModHandler ThisMod;

        internal static void ReReadSettings()
        {
            AccessTools.Field(typeof(Mod), "modSettings").SetValue(ThisMod, null);
            ReadSettings();
        }

        internal static void ReadSettings()
        {
            ThisMod.GetSettings<Settings>();
        }

        public override string SettingsCategory() => "Toggles";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Dialog_Settings.DoWindowContents(inRect);
        }
    }
}