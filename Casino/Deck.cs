using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Deck // this is our class and inside of it we have an insturctor(method)
    {
        public Deck() // this is our constructor( a method being called as soon as the object has been created) 
        {
            Cards = new List<Card>(); // the constructor will instaciate its property card as an empty list of cards
            
            for (int i = 0; i < 13; i++) // this loops through numbers on the cards
            {
                for (int j = 0; j < 4; j++) // this loops through the suits
                {
                    Card card = new Card();
                    card.Face = (Face)i;
                    card.Suit = (Suit)j;
                    Cards.Add(card);
                }
            }     
        }
        public List<Card> Cards { get; set; }

        /* this is a method specific to this class, we aren't returning anything and that is why we have void, but it is accessable from anywhere
         in the program because of the public and it has a parameter of int times =1 which is an option parameter of how many times we might
        want to have the cards shuffled. We then do a for loop to go through the cards and actually get them shuffled*/
        public void Shuffle(int times = 3)
        {
            for (int i = 0; i < times; i++)
            {
                List<Card> tempList = new List<Card>(); // make a tempList to hold our new cards that will be shuffled
                Random random = new Random(); // call the shuffle class from the c# library

                while (Cards.Count > 0) // do a while loop saying while cards(52) is not greater than 0 do the following insid
                {
                    int randomIndex = random.Next(0, Cards.Count); // this will make a random index to grab a card
                    tempList.Add(Cards[randomIndex]);// then we add the random card to our tempList
                    Cards.RemoveAt(randomIndex); // this is a function of the list method and this method will add the card to our new list and
                                                      // then it will remove the card added to the tempList from our deck var until no cards are left
                }
                this.Cards = tempList; // we then assign our deck.cards to the new tempList holding the shuffled cards
                // the 'this' is refereing to itself so when you do this.Cards we are just refering to the Cards class itself.
            }
        }
    }
}
