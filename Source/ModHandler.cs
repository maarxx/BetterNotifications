using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BetterNotifications
{
    internal class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        {
            //Settings.InitDatabase();
            GetSettings<Settings>();
        }

        public override string SettingsCategory() => "BetterNotifications";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard view = new Listing_Standard();
            view.Begin(inRect);
            foreach (KeyValuePair<LetterDef, bool> let in Controller.LetterSettings)
            {
                view.CheckboxLabeled(let.Key.LabelCap, ref Settings.letterSettings[let.Key])
            }

            view.End();
        }
    }
}