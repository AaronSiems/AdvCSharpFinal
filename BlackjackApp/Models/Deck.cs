using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public class Deck
    {
        private string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", };
        private string[] suits = { "C", "S", "D", "H" }; //Clubs, Spades, Diamonds, Hearts

        public List<Card> cards { get; set; }

        //Clears the deck and populates with all 52 cards
        public void NewDeck()
        {
            cards = new List<Card>();
            foreach(string r in ranks)
            {
                foreach(string s in suits)
                {
                    cards.Add(new Card { Rank = r, Suit = s });
                }
            }
        }

        //Picks a random card in the list, no need for a shuffle function
        public Card Deal()
        {
            var rand = new Random();
            int i = rand.Next(cards.Count);

            var Card = cards[i];
            cards.Remove(Card);
            return Card;
        }
    }
}
