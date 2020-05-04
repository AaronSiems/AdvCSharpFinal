using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public class Hand
    {
        private List<Card> cards { get; set; }


        public Hand()
        {
            cards = new List<Card>();
        }

        public void Add(Card c)
        {
            cards.Add(c);
        }

        [JsonIgnore]
        public int Value
        {
            get
            {
                int total = cards.Sum(cards => cards.Value);
                int aces = cards.Where(c => c.IsAce).Count();

                while (aces > 0 && total > 21) //If aces exist and our total is 21 then we will
                {
                    total -= 10; //Remove 10 since value of ace was 11
                    aces--;
                }

                return total;
            }
        }

        [JsonIgnore]
        public bool HasBlackJack => cards.Count == 2 && Value == 21;
        [JsonIgnore]
        public bool IsBusted => Value > 21;
    }
}
