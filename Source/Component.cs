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

            new AlertSleeper();
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            AlertSleeper.CheckIfSleeping();
            LetterTimer.CheckLetters();
        }
    }
}