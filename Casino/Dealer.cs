using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Casino
{
    public class Dealer // this is our dealer class that can be used for our game or another casino game
    {
        public string Name { get; set; } //we make a method that has a name, we get the name and then set it
        public Deck Deck { get; set; } // dealer has a Deck method as well
        public int Balance { get; set; } // and a balance method

        public void Deal(List<Card> Hand) // this is a method that is called when the dealer will deal the cards from his hand
        {
            Hand.Add(Deck.Cards.First()); // will take first card from shuffled deck and distribute it to the player and dealer
            string card = string.Format(Deck.Cards.First().ToString() + "\n"); // will format the card properly to give us the Face and Suit of card dealt
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"D:\Logs\log.txt", true)) // this will save the cards dealt to us in a new file
            {
                file.WriteLine(DateTime.Now); // add time card was dealt
                file.WriteLine(card); // add card dealt to file
            }
            Deck.Cards.RemoveAt(0); // then it will remove the card from the Deck
        }
    }
}
