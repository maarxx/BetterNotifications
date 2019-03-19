using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BetterNotifications
{
    internal class Letter_Patch
    {
        static Dictionary<Letter, int> TimedLetters { get; } = new Dictionary<Letter, int>();

        internal static void CheckLetters()
        {
            int ticks = Find.TickManager.TicksGame;

            if (ticks % GenDate.TicksPerHour == 0)
            {
                HashSet<Letter> remove = new HashSet<Letter>();

                foreach (Letter letter in Find.LetterStack.LettersListForReading)
                {
                    if (Controller.LetterSetting(letter.def))
                    {
                        if (!TimedLetters.ContainsKey(letter))
                            TimedLetters.Add(letter, ticks);

                        if (TimedLetters[letter] + (GenDate.TicksPerHour * Controller.LetterTime) > ticks)
                            remove.Add(letter);
                    }
                }
                foreach (Letter letter in remove)
                {
                    TimedLetters.Remove(letter);
                    Find.LetterStack.RemoveLetter(letter);
                }
            }
        }
    }
}