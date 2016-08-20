using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    //This is a game of mastermind
    public class MasterMind
    {
        // how many guesses are allowed.
        private int guesses;
        // number of tries in this game.
        private int tries = 0;
        // the secret code that we need to find.
        private int[] secretCode = new int[4];
     
        public MasterMind(int guesses)
        {
            this.guesses = guesses;
        }

        //creates the secret code with 4 digits between 1 and 6.
        public void CreateCode()
        {
            Random rand = new Random();
            for (int i = 0; i < secretCode.Length; i++)
            {
                secretCode[i] = rand.Next(1, 6);
            }
        }

        //returns a combination of "+" (for the right number in the right place) and "-" (for just the right number)
        public string Guess(string guess)
        {
            HandleOutOfRangeExceptiom(guess);
            string pluses = "";
            string minuses = "";
            HashSet<int> usedGuess = new HashSet<int>();
            HashSet<int> usedSecret = new HashSet<int>();
            int[] guessArray = new int[4]; 

            //we show a more efficient solution in the solver because I really wanted to find a O(n) solution
            //calculates the number of pluses and creates an array based on the guess
            for (int i = 0; i < guess.Length; i++)
            {
                int attempt;
                //checks that the input is a number and stores it in attempt.
                if (int.TryParse(guess[i].ToString(), out attempt))
                {
                    //checks if we should add a plus
                    if (attempt == secretCode[i])
                    {
                        usedGuess.Add(i);
                        usedSecret.Add(i);
                        pluses += "+";
                    }
                    guessArray[i] = attempt;
                }
                else
                {
                    throw new System.ArgumentException("Incorrect guess. Can only be digits");
                }
            }
            minuses = findMinuses(guessArray, usedGuess, usedSecret);
            
			tries++;
			return pluses+minuses;
		}

        //figures out how many minuses there are
        public string findMinuses(int[] guessArray, HashSet<int> usedGuess, HashSet<int> usedSecret)
        {
            string minuses = "";
            for (int i = 0; i < guessArray.Length; i++)
            {
                for (int slot = 0; slot < secretCode.Length; slot++)
                {
                    //checks if the slot has been used and if the guess is contained in the code
                    if (!usedGuess.Contains(slot) && !usedSecret.Contains(i) && guessArray[i] == secretCode[slot])
                    {
                        usedGuess.Add(slot);
                        minuses += "-";
                        //we want to break out so as to not check any of the later digits with the same digits
                        break;
                    }
                }
            }
            return minuses;
        }

        //throws an exception if the guess is the wrong length
        public void HandleOutOfRangeExceptiom(string guess)
        {
            if (guess.Length != secretCode.Length)
            {
                throw new System.IndexOutOfRangeException("Incorrect guess length");
            }
        }

        public int Guesses
        {
            get { return guesses; }
        }

        public int Tries
        {
            get { return tries; }
        }
    }
}
