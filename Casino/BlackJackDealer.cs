using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BlackJack
{
    public class BlackJackDealer: Dealer // here we made a specific blackjack dealer class
    {
        private List<Card> _hand = new List<Card>();  // a private method or class is used when we know it will only be used in this one class. This is a new list of card called hand
        public List<Card> Hand { get { return _hand; } set { _hand = value; } } // when doing a private method we need to make one as well of public like so that will get and set the values later on
        public bool Stay { get; set; } // method that will be used later to let us know if the user or dealer will stay with the cards they have 
        public bool isBusted { get; set; } // method that will be called later to let us know if anyone went over 21
    }
}
