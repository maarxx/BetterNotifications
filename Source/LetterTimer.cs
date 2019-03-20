using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BetterNotifications
{
    internal class LetterTimer
    {
        static Dictionary<Letter, int> TimedLetters { get; } = new Dictionary<Letter, int>();

        internal static void CheckLetters()
        {
            int ticks = Find.TickManager.TicksGame;

            if (ticks % GenDate.TicksPerHour == 0)
            {
                List<Letter> letters = Find.LetterStack.LettersListForReading;

                for (int i = letters.Count - 1; i > -1; i--)
                {
                    Letter letter = letters[i];
                    if (Controller.LetterSetting(letter.def))
                    {
                        if (!TimedLetters.ContainsKey(letter))
                            TimedLetters.Add(letter, ticks);

                        if ((TimedLetters[letter] + (GenDate.TicksPerHour * Controller.LetterTime)) < ticks)
                        {
                            Find.LetterStack.RemoveLetter(letter);
                            TimedLetters.Remove(letter);
                        }
                    }
                }
            }
        }
    }
}