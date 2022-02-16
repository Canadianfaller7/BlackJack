using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Player
    {
        public Player(string name, int begginingBalance) // this is a constructor that takes a parameter of Name and begginingBalance
        {
            Hand = new List<Card>(); // create empty list card here and assign it to hand
            Balance = begginingBalance; // this will be the balance the user input at the beginning
            Name = name; // name of the player
        }
        private List<Card> _hand = new List<Card>();
        public List<Card> Hand { get { return _hand; } set { _hand = value; } }
        public int Balance { get; set; }
        public string Name { get; set; }
        public bool isActivelyPlaying { get; set; }
        public bool Stay { get; set; }


        public bool Bet (int amount) // this is a bet method
        {
            if (Balance - amount < 0) // saying if the balance of the user - their bet amount is less than 0 then print the statement below
            {
                Console.WriteLine("\nYou do not have enough to place a bet that size.");
                return false;  
            }
            else
            {
                Balance -= amount; // else subtract their bet amount from the balance and then return true and let the player play
                return true;
            }
        }

        public static Game operator+ (Game game, Player player) // here we actually are overloading the plus operator to add a player to the game when the user puts in their name
        {
            game.Players.Add(player);
            return game;
        }
        public static Game operator- (Game game, Player player) // this one is to remove the player from the game
        {
            game.Players.Remove(player);
            return game;
        }
    }
}
