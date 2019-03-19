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
        LetterDef def;
        bool active;

        internal string LabelCap => def.LabelCap;
        internal string defName => def.defName;

        internal void ExposeData()
        {
            Scribe_Values.Look<bool>(ref active, def.defName, false);
        }
    }
}