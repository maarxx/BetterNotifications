using Verse;

namespace BetterNotifications
{
    internal class LetterSet
    {
        internal LetterSet(LetterDef def)
        {
            this.def = def;
            active = false;
        }
        internal LetterDef def;
        internal bool active;

        internal string LabelCap => def.LabelCap;
        internal string DefName => def.defName;

        internal void ExposeData()
        {
            Scribe_Values.Look<bool>(ref active, def.defName, false);
        }
    }
}