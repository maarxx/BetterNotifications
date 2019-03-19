using Verse;

namespace BetterNotifications
{
    internal class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref letterTime, "LetterTime", 2);
            Scribe_Values.Look<int>(ref alertTime, "AlertTime", 2);
            foreach (LetterSet letter in Controller.LetterSets)
                letter.ExposeData();
        }

        internal static int letterTime = 2;
        internal static int alertTime = 2;
    }
}