using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            int allowed;
            Console.WriteLine("Input the number of allowed tries");                      
            string allowedString = Console.ReadLine();    
            int.TryParse(allowedString, out allowed);
            MasterMind game = new MasterMind(allowed);
            game.CreateCode();
            bool solved = false;

            //Game loop
            while(game.Tries < allowed && !solved)
            {
                try
                {
                    Console.WriteLine("Enter a guess. You have " + (allowed - game.Tries) +" tries left");        
                    string guess = Console.ReadLine();
                    if (guess.Equals("solve"))
                    {
                        Solver solver = new Solver(game);
                        guess = solver.Solve();
                    }
                    string result = game.Guess(guess);
                    Console.WriteLine(result);
                    solved = result.Equals("++++");
                    if (solved)
                    {
                        Console.WriteLine("You solved it!");
                    }
                }
                catch(Exception e){
                    Console.WriteLine("Wrong Input. Should be 4 digits");
                }
            }

            //if we didn't solve it
            if(!solved){
                Console.WriteLine("You lose :(");
            }

            Console.WriteLine("Play again? y/n");
            string answer = Console.ReadLine();
            if (answer.Equals("y"))
            {
                Main(new string[4]);
            }
        }
    }
}

