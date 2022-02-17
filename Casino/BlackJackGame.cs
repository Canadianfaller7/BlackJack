using System;
using System.Collections.Generic;
using System.Linq;

namespace Casino.BlackJack
{
    public class BlackJackGame : Game, IWalkAway
    {
        public BlackJackDealer Dealer { get; set; } // this is our 21 dealer property
        public override void Play() // override is how you can apply the abstract method to a class that is inheriting from the abstract class
        {
            Dealer = new BlackJackDealer(); // here we instansiate a dealer
            foreach (Player player in Players) // here we will reset the players
            {
                player.Hand = new List<Card>(); // here we will reset their hand to be blank
                player.Stay = false; // we then make this false so it doesn't carry anything over
            }
            Dealer.Hand = new List<Card>(); // this also makes the dealer have a new hand and be freshly shuffled
            Dealer.Stay = false; // want this false too
            Dealer.Deck = new Deck(); // we then make a new deck again so the deck has all 52 cards always and will always be shuffled
            Dealer.Deck.Shuffle(); // here we shuffle the cards

            foreach (Player player in Players) // we will now loop through the players again
            {
                // all this from bool to the if inside the while is to do an exception if the user adds string or decimal and to tell them to just add numbers
                bool validAnswer = false; // set this to false to run error messages
                int bet = 0;
                while (!validAnswer) // while validAnswer is true
                {
                    Console.WriteLine("\nPlace your bet!\n"); // print this
                    validAnswer = int.TryParse(Console.ReadLine(), out bet); // try running this code which will get users answer and try running it
                    if (!validAnswer) // if user put a string or decimal in the code then print statement below else continue on with code
                    {
                        Console.WriteLine("\nPlease enter digits only, No decimals");
                    }
                    if (bet < 0)
                    {
                        throw new FraudException();
                    }
                }
                bool succesfullyBet = player.Bet(bet); // we will then make sure that the amount the user bet is true with our succesfullyBet and it wasn't 0
                if (!succesfullyBet) // if their bet is not sufficent then return to main page and ask the user to place their bet again
                {
                    return; // a plain return on a void just exits the if statement
                }
                Bets[player] = bet; // this will take the number the user put in for each player and then set it equal to be to help keep track of it and that is done by using our dictionary from game
            }
            for (int i = 0; i < 2; i++) // here we do a for loop starting at 0 and then this will loop through players and dealer giving each person a card and then going through again so each player now has 2 cards
            {
                Console.WriteLine("\nDealing..."); // tell user dealing is happening
                foreach (Player player in Players) // loop through players giving their name
                {
                    Console.Write("\n{0}: ", player.Name); // printing user name to screen
                    Dealer.Deal(player.Hand); // this will then call the deal method from our dealer class and we pass in argument player.hand so the card gets added to the players hand
                    if (i == 1) // this will then check to see if anyone got blackjack on the first 2 cards
                    {
                        bool blackJack = BlackJackRules.CheckForBlackJack(player.Hand); // here we are instanciating our checkforblackjack method from the blackjackrules class and passing in player.hand as the argument and this will check if the user got black jack after the first 2 cards
                        if (blackJack) // if user got black jack then do the following
                        {
                            Console.WriteLine("\nBlackJack! {0} wins {1}.", player.Name, Bets[player]); // saying who the winner is and how much they won
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]); // multiply their bet by 1.5 and that is the users winnings
                            return; // return to place another bet
                        }
                    }
                }
                Console.Write("Dealer: "); // same as above but for the dealer
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = BlackJackRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        Console.WriteLine("\nDealer has BlackJack! Everyone loses!");
                        foreach (KeyValuePair<Player, int> entry in Bets) // we iterate through dict to get the dealer to receive the balance of everything
                        {
                            Dealer.Balance += entry.Value; // dealer will keep the bets that were placed
                        }
                        return;// return and ask if you want to play again
                    }
                }
            }
            foreach (Player player in Players)
            {
                while (!player.Stay) // while the user isn't staying
                {
                    Console.WriteLine("\nYour cards are:\n"); // tell the user what their current cards are
                    foreach (Card card in player.Hand) // this will tell us the cards we got again 
                    {
                        Console.WriteLine("{0} ", card.ToString());
                    }
                    Console.WriteLine("\n\nHit or Stay?\n"); // ask us if we want to hit or stay based on our card totals
                    string answer = Console.ReadLine().ToLower(); // answer can be lowercased
                    if (answer == "stay")
                    {
                        player.Stay = true; // if you stay break out of the if statement and then wait for the dealer to choose to stay or hit
                        break;
                    }
                    else if (answer == "hit") // if hit
                    { 
                        Dealer.Deal(player.Hand); // get another card from the dealer and then ask if we want to hit or stay if we didn't bust
                    }
                    bool busted = BlackJackRules.isBusted(player.Hand); // check to see if the user busted
                    if (busted) // if busted
                    {
                        Dealer.Balance += Bets[player]; // give dealer the bets and add to his balance
                        Console.WriteLine("\n{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance); // tell us we lost and give us our new current balance after losing
                        Console.WriteLine("\nDo you want to play again?\n"); // ask if we want to play again
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "yeah" || answer == "ya" || answer == "sure" || answer == "y") // acceptable answers from the user
                        {
                            player.isActivelyPlaying = true; // because true will ask to place a bet again
                            return;
                        }
                        else
                        {
                            player.isActivelyPlaying = false; // if false then print the thanks for playing and feel free to walk around statement
                            return;
                        }
                    }
                }
            }
            Dealer.isBusted = BlackJackRules.isBusted(Dealer.Hand); // this will be the same as above but for the dealer
            Dealer.Stay = BlackJackRules.shouldDealerStay(Dealer.Hand); // this is a custom rule so we will reach into our rules class and access the method of stay
            while (!Dealer.Stay && !Dealer.isBusted) // while dealer isn't busted and isn't staying he will hit
            {
                Console.WriteLine("\nDealer is hitting...");
                Dealer.Deal(Dealer.Hand); // give dealer another card
                Dealer.isBusted = BlackJackRules.isBusted(Dealer.Hand); // check if they busted
                Dealer.Stay = BlackJackRules.shouldDealerStay(Dealer.Hand); // check again if they should stay or hit
            }
            if (Dealer.Stay)
            {
                Console.WriteLine("\nDealer is staying.");
            }
            if (Dealer.isBusted)
            {
                Console.WriteLine("\nDealer Busted!\n");
                foreach (KeyValuePair<Player, int> entry in Bets) // if dealer busted then we wiill do the following, loop through dict
                {
                    Console.WriteLine("\n{0} won {1}!.", entry.Key.Name, entry.Value); // this gets the name and value of the player who won and how much they won

                    // this goes into our player list and we apply lambda query where. get the list of players where their name == the name in the dict above and grab the first name
                    // then we grab the balance of that person and we add what they bet * 2, so user should get what they bet plus their winnings
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);
                    Dealer.Balance -= entry.Value;
                }
                return;
            }
            foreach ( Player player in Players) // this will loop through all players and compare their hand to the dealers hand to see who won or if a tie happened
            {
                bool? playerWon = BlackJackRules.CompareHands(player.Hand, Dealer.Hand); // to make a bool null, add a ?. here we are comparing the hands to see who won
                if (playerWon == null)
                {
                    Console.WriteLine("\nPush! No one wins.");
                    player.Balance += Bets[player]; // giving player back what they bet but nothing else since it was a tie
                }
                else if (playerWon == true)
                {
                    Console.WriteLine("\n{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += (Bets[player] * 2); // giving player their bet, plus the other bets * 2
                    Dealer.Balance -= Bets[player]; // taking the funds away from the dealer
                }
                else
                {
                    Console.WriteLine("\nDealer wins {0}!", Bets[player]);
                    Dealer.Balance += Bets[player]; // adding the bets to the dealers balance since they won
                }
                // asking user if they want to play again
                Console.WriteLine("\nPlay again?\n");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "yeah" || answer == "ya" || answer == "sure" || answer == "y")
                {
                    player.isActivelyPlaying = true;
                }
                else
                {
                    player.isActivelyPlaying = false;
                }
            }
        }
        public override void ListPlayers() // this is using the ListPlayers but we are overriding what it says and putting in our own stuff
        {
            Console.WriteLine("Black Jack Players:\n");
            base.ListPlayers();
        }
        public void WalkAway(Player player) // to use the interface method, it has to be put in the same way as it was made
        {
            throw new NotImplementedException(); // we will define this later
        }
    }   
}
