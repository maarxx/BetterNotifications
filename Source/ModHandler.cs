using UnityEngine;
using Verse;

namespace BetterNotifications
{
    internal class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            GetSettings<Settings>();
        }

        public override string SettingsCategory() => "BetterNotifications";

        public override void DoSettingsWindowContents(Rect inRect)
        {
        }
    }
}