using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public class Card
    {
        public string Rank { get; set; }
        public string Suit { get; set; }

        public int Value
        {
            get
            {
                if (IsAce) //Always return 11, can check if we want to use 1 in other classes
                {
                    return 11;
                } 
                else if (Rank == "J" || Rank == "Q" || Rank == "K")
                {
                    return 10;
                }
                else
                {
                    return Convert.ToInt32(Rank);
                }
            }
        }

        public bool IsAce => Rank == "A"; //Use for hand calculations in other classes
    }
}
