using Toggles.Patches;
using Verse;

namespace BetterNotifications
{
    class Component : GameComponent
    {
        public Component(Game game)
        { }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            new Alert_Patch();
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            Alert_Patch.CheckIfSleeping();
            Letter_Patch.CheckLetters();
        }
    }
}