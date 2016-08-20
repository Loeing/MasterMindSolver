using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    //greedy min-max solver for mastermind.
    class Solver
    {
        private MasterMind game;
        private List<SolverNode> allPossible = new List<SolverNode>();
        private List<SolverNode> possibleSolutions = new List<SolverNode>();

        public Solver(MasterMind game)
        {
            this.game = game;
            //this is awful. it initializes the possible solutions.
            for (int thousands = 1; thousands <= 6; thousands++)
            {
                for (int hundreds = 1; hundreds <= 6; hundreds++)
                {
                    for (int  tens = 1; tens <= 6; tens++)
                    {
                        for (int units = 1; units <= 6; units++)
                        {
                            allPossible.Add(new SolverNode(new int[] { thousands, hundreds, tens, units }));
                            possibleSolutions.Add(new SolverNode(new int[] { thousands, hundreds, tens, units }));
                        }
                    }
                }
            }
        }

        //solves the game
        public string Solve()
        {
            //our initial try.
            int[] guess = new int[] { 1, 1, 2, 2 };
            string result = "";
            //the loop reduces the possible solutions until there is only one left, which will be the solution
            while (!result.Equals("++++") && possibleSolutions.Count > 1)
            {
                //tries the guess in the game
                result = game.Guess(String.Join("",guess));
                Console.WriteLine(String.Join("", guess) + "    " + result);
                RemoveImpossible(CreateRemovable(result, guess));
                UpdateScores();
                guess = allPossible.Max().Code;
            }
            string solution = String.Join("", possibleSolutions.First().Code);
            result = game.Guess(solution);
            Console.WriteLine(solution + "    " + result);
            return String.Join("", solution);
        }

        //updates the score of all of the possible solutions
        public void UpdateScores()
        {
            //initializing all possible results,  +++- is not possible 
            List<string> possibleResults = new List<string>() { "++++", "+++", "++--", "++-", "++", "+---", "+--", "+-", "+", "", "-", "--", "---", "----" };
            foreach (SolverNode solution in allPossible)
            {
                //1296 is the number of possible solutions, we use it as a max
                solution.Score = 1296; 
                //assigns the lowest possible number of possibilities that would be removed
                //by picking that code to each potential solution
                foreach (string result in possibleResults)
                {
                    if (CreateRemovable(result, solution.Code).Count < solution.Score)
                    solution.Score = CreateRemovable(result, solution.Code).Count;
                }
                if (solution.Score == 1296)
                {
                    solution.Score = 0;
                }
            }
        }

        //removes all the impossible solutions in removable from the list of potential solutions 
        public void RemoveImpossible(List<SolverNode> removable)
        {
            foreach(SolverNode impossibleSolution in removable)
            {
                possibleSolutions.Remove(impossibleSolution);
            }
        }

        //generates a list of the solutions that are no longer possible if this guess get's this result
        public List<SolverNode> CreateRemovable(string result, int[] guess)
        {
            List<SolverNode> removable = new List<SolverNode>();
            foreach (SolverNode potentialSolution in possibleSolutions)
            {
                if (!CheckGuess(potentialSolution.Code, guess).Equals(result))
                {
                    removable.Add(potentialSolution);
                }
            }
            return removable;
        }

        //This is basically the same method as the Guess method in Mastermind
        //(which we should have made more generalized and we would in a professional setting),
        //we should reuse code, but we will take this opportunity to show a more "clever" algorithmn. (While this is O(n) and the other is O(n^2) for n=4 they do about as much computation
        public string CheckGuess(int[] guess,int[] solution)
        {
            string result = "";
            int minuses = 0;
            int[] solutionOccurences = CountOccurrences(solution);
            int[] guessOccurences = CountOccurrences(guess);
            //count pluses
            for (int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == solution[i])
                {
                    result += "+";
                }
            }
            //counts minuses by seeing which of the guess or the solution has the most occurences of the digit
            for (int i = 1; i < guessOccurences.Length; i++)
            {
                minuses += Math.Min(solutionOccurences[i], guessOccurences[i]);
            }
            //removes the perfect matches from all the matches
            minuses = minuses-result.Length;
            for (int i = 0; i < minuses; i++) 
            {
                result += "-";
            }
            return result;
        }

        //counts number of occurences of each digit in the code.
        public int[] CountOccurrences(int[] code)
        {
            int[] occurrences = new int[7];
            for (int i = 0; i < code.Length; i++)
            {
                occurrences[code[i]]++;
            }
            return occurrences;
        }
    }
}
