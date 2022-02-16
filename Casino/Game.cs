using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    // generic class for most card games we made this with these methods so we can use it for multiple games that can inherit the methods from this class
    public abstract class Game // abstract classes are here to lock our class because we will never make an instance of the class meaning it will never be made an object
    {
        private List<Player> _players = new List<Player>(); // to make sure list is never null we make this list Private
        private Dictionary<Player, int> _bets = new Dictionary<Player, int>(); // we will make a private dictionary the key is Player and the value is the amount the user bet

        public List<Player> Players { get { return _players; } set { _players = value; } } // this is the same as the get set, but now we are returning an empty list and not a null, this will stop it from throwing errors
        public string Name { get; set; } // metho for the name of the game we will play

        public Dictionary<Player, int> Bets { get { return _bets; } set { _bets = value; } } // we then make this here because we need to do this when making a private class

        public abstract void Play(); // abstract method contain no implementation and can only be made inside a abstract class, these state that any class
        // inheriting the class Game must also have this method
        public virtual void ListPlayers()
            // virtual means that this method gets inhertited by an inheriting class but has the ability to override it, it can be implemented but can be overwritten
        {
            foreach (Player player in Players) //loop through players and display their names on screen
            {
                Console.WriteLine(player);
            }
        }
    }
}
