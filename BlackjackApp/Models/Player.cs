using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public class Player
    {
        public Hand hand { get; set; }
        public int winnings { get; set; }

        public Player()
        {
            hand = new Hand();
        }

        public void NewHand(Card c1, Card c2)
        {
            hand = new Hand();
            hand.Add(c1);
            hand.Add(c2);
        }

        public void Hit(Card c)
        {
            hand.Add(c);
        }
        
    }
}
