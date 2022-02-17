using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Casino;
using Casino.BlackJack;

namespace BlackJack
{
    class Program // this is our main class called Program and this is the entrance point of a console app
    {
        static void Main(string[] args) // this is our main method(function inside the class and when called will do the stuff we put inside it)
        {
            

            Console.WriteLine("Welcome to the Grand Hotel and Casino! Let's start by telling me your name.\n"); // welcoming someone to black jack and asking their name
            string playerName = Console.ReadLine(); // save user input to a var

            // all this from bool to the if inside the while is to do an exception if the user adds string or decimal and to tell them to just add numbers
            bool validAnswer = false; // set this to false to run error messages
            int bank = 0;
            while (!validAnswer) // while validAnswer is true
            {
                Console.WriteLine("\nAnd how much money did you bring today?\n"); // print this
                validAnswer = int.TryParse(Console.ReadLine(), out bank); // try running this code which will get users answer and try running it
                if (!validAnswer) // if user put a string or decimal in the code then print statement below else continue on with code
                {
                    Console.WriteLine("\nPlease enter digits only, No decimals");
                }
            }
            Console.WriteLine($"\nHello {playerName}. Would you like to join a game of 21 right now?\n"); // ask user if they want to play black jack
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "ya" || answer == "sure" || answer == "y") // acceptable answers from user
            {
                Player player = new Player(playerName, bank); // if user wants to play create a new Player with their name and how much money they have
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"D:\Logs\log.txt", true)) // this will save the cards dealt to us in a new file
                {
                    file.WriteLine(player.Id);
                }
                Game game = new BlackJackGame(); // start a new black jack game
                game += player; // add the player to the game
                player.isActivelyPlaying = true; // this is true if the player said they want to play
                while (player.isActivelyPlaying && player.Balance > 0) // this is saying while the player is wanting to play and they don't have a balance of 0 then
                {
                    try
                    {
                        game.Play(); // keep the game playing
                    }
                    catch (FraudException)
                    {
                        Console.WriteLine("\nSecurity! This person is cheating. Kick them out!");
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nAn error occured. Please contact your System Administrator");
                        Console.ReadLine();
                        return;
                    }
                }
                game -= player; // else remove the player from the game
                Console.WriteLine("\nThank you for playing!"); // print this if the user isn't active anymore
            }
            Console.WriteLine("\nFeel free to look around the casino. Bye for now."); // this will print if we don't get an acceptable answer or user doesn't want to play anymore
            Console.ReadLine();
        }
    }
}
