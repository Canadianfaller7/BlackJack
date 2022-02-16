using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    class Program // this is our main class called Program and this is the entrance point of a console app
    {
        static void Main(string[] args) // this is our main method(function inside the class and when called will do the stuff we put inside it)
        {
            Console.WriteLine("Welcome to BlackJack! Let's start by telling me your name.\n"); // welcoming someone to black jack and asking their name
            string playerName = Console.ReadLine(); // save user input to a var
            Console.WriteLine("\nHow much money did you bring today?\n"); // ask how much money they brought with them
            int bank = Convert.ToInt32(Console.ReadLine()); // save usr input to a int var
            Console.WriteLine($"\nHello {playerName}. Would you like to join a game of 21 right now?\n"); // ask user if they want to play black jack
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "ya" || answer == "sure" || answer == "y") // acceptable answers from user
            {
                Player player = new Player(playerName, bank); // if user wants to play create a new Player with their name and how much money they have
                Game game = new BlackJackGame(); // start a new black jack game
                game += player; // add the player to the game
                player.isActivelyPlaying = true; // this is true if the player said they want to play
                while (player.isActivelyPlaying && player.Balance > 0) // this is saying while the player is wanting to play and they don't have a balance of 0 then
                {
                    game.Play(); // keep the game playing
                }
                game -= player; // else remove the player from the game
                Console.WriteLine("\nThank you for playing!"); // print this if the user isn't active anymore
            }
            Console.WriteLine("\nFeel free to look around the casino. Bye for now."); // this will print if we don't get an acceptable answer or user doesn't want to play anymore
            Console.ReadLine();
        }
    }
}
