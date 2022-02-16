using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BlackJack
{
    public class BlackJackRules
    {
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>() // this is a private dictionary which means it will only be used in this class
        // face will be the key and int will be the value. for private classes in order to name something you need an underscore infront of the name. We are instanciating it with all of our object since it will never change
        
        { // here we instanciate the dictionary entry we will create all these dictionary entries one for each face and the number after the equal is the value
            // we use dictionaries because we can then assign a value to multiple keys where as if it was done as an enum it would give some errors.
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1
        };

        private static int[] GetAllPossibleHandValues(List<Card> Hand) // this is our method to check if the ace should count as 1 or 11 and will return an int array
        {
            // here we use a lambda expression which are methods we can perform on lists
            int aceCount = Hand.Count(x => x.Face == Face.Ace); // here we are asking if the cards face is equal to Ace and then we will count it and it will return a count in the form of variable ace count
            int[] result = new int[aceCount + 1]; // once we have our result we then try to find out how many possible results there are of aces + 1 (so if you have 2 aces then there are 3 results that can happen such as 1 ace equals 1 and the other equals 11, both equal 1 or both equal 11)
            int value = Hand.Sum(x => _cardValues[x.Face]); // this will map through our dictionary to find each item and look it up in our dict table and then finds its face value and then sums it with their value(so if ace, it would be 1)
            result[0] = value; // then we take the first entry in our int array and assign value to it
            if (result.Length == 1) return result; // if there are no aces then there is only possible value, ace is the only card with more than 1 value and so if we have no aces, there is only the 1 
            // so we are checking if the result length equals 1 then we just return result versus checking for if there is more than 1 value for other face cards.

            for(int i = 1; i < result.Length; i++) // here we do a for loop and if this gets hit then we know the result length value is greater than 1
            // we will iterate through putting in different values of ace
            {
   // this is saying to create the int array of possible values we will make a seperate value for each ace and then add 10 to it, so we already have 1 ace which equals 1
   // so next value will be value + 1 * 10 and that will be the second value and then if there is a 3rd ace then it will be 2 * 10 which will add 20 which is 2 aces on top of that
                value += (i * 10); // we take value and equal it to value + i * 10. value is the lowest value(all aces equalling 1)
                result[i] = value; // and then have result[i] = value.
            }
            return result;
        }

        public static bool CheckForBlackJack(List<Card> Hand) // this is our method to check if anyone got black jack
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand); // create int array of possible values and pass in the hand we are doing
            int value = possibleValues.Max(); // here we will get the largest value here, so the max value the cards could have to win is 21
            if (value == 21) return true; // we say if the max value == 21 then it is black jack
            else return false; // if not then you either are short of 21 or busted
        }
        public static bool isBusted(List<Card> Hand) // this is to check if someone busted
        {
            int value = GetAllPossibleHandValues(Hand).Min(); // check if the min value of the hand busted
            if (value > 21) return true; // if the value is greater than 21 then we busted
            else return false; // else return false
        }
        public static bool shouldDealerStay(List<Card> Hand) // method on if the dealer should stay or hit
        {
            int[] possibleHandValues = GetAllPossibleHandValues(Hand); // we check the possible values of the hand of the dealer
            foreach ( int value in possibleHandValues)
            {
                if (value > 16 && value < 22) // if value is greater than 16 but less than 22 then the dealer will always stay
                {
                    return true;
                }
            }
            return false; // else if it is below 16 then hit
        }
        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand) // this is a method to compare the hands(values) of the player and dealer
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand); // we get this int array of all possible hand values for player hand
            int[] dealerResults = GetAllPossibleHandValues(DealerHand); // same but for dealer

            int playerScore = playerResults.Where(x => x < 22).Max(); // then we check the the highest value in players hand that isn't above 22
            int dealerScore = dealerResults.Where(x => x < 22).Max(); // same as above

            if (playerScore > dealerScore) // if players value is higher than the dealers then player wins
            {
                return true;
            }
            else if (playerScore < dealerScore) // dealers score is higher than player, dealer wins
            {
                return false;
            }
            else // else it is a tie cause both scores are the same
            {
                return null;
            }
        }
    }
}
