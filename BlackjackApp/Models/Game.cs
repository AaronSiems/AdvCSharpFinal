using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public class Game : GameInterface
    {
        private ISession session { get; set; }
        public Deck deck { get; set; }
        public Player player { get; set; }
        public Dealer dealer { get; set; }
        public bool NeedsDeal { get; set; }
        public enum Result
        {
            Shuffling, Continue, DealerBust, DealerBlackJack, DealerWin,
            PlayerBust, PlayerBlackJack, PlayerWin, DoubleBlackJack, Push
        }


        public Game(IHttpContextAccessor accessor)
        {
            //Get our session data or create new ones if not exist. Default NeedsDeal to true
            session = accessor.HttpContext.Session;
            deck = session.GetObject<Deck>("deck") ?? new Deck();
            player = session.GetObject<Player>("player") ?? new Player();
            dealer = session.GetObject<Dealer>("dealer") ?? new Dealer();
            NeedsDeal = Convert.ToBoolean(session.GetInt32("needsdeal") ?? 1);
        }

        public Result Deal()
        {
            var result = Result.Continue;

            if(deck.cards == null) { deck.NewDeck(); } //New game - create new deck

            //If less than 4 cards left we will make new deck otherwise deal the cards
            if(deck.cards.Count < 4)
            {
                deck.NewDeck();
                result = Result.Shuffling;
            } else
            {
                player.NewHand(deck.Deal(), deck.Deal());
                dealer.NewHand(deck.Deal(), deck.Deal());
                NeedsDeal = false;
            }

            //Check for instant wins
            if(player.hand.HasBlackJack && dealer.hand.HasBlackJack)
            {
                Update();
                result = Result.DoubleBlackJack;
            } else if (player.hand.HasBlackJack)
            {
                Update();
                result = Result.PlayerBlackJack;
            } else if (dealer.hand.HasBlackJack)
            {
                Update();
                result = Result.DealerBlackJack;
            }


            Save();
            return result;
        }

        public Result Hit()
        {
            //Attempt to hit player, if deck empty return shuffle else check if hit busts player and return busted
            //Default action = continue
            var result = Result.Continue;
            bool shuffle = HitPlayer();

            if (shuffle)
            {
                deck.NewDeck();
                result = Result.Shuffling;
            } else if(player.hand.IsBusted) {
                Update();
                result = Result.PlayerBust;
            }
            Save();
            return result;
        }

        public Result Stand()
        {
            var result = Result.Continue;

            Save();
            return result;
        }

        private bool IsDealerHandHigher => player.hand.Value < dealer.hand.Value;
        private bool IsPlayerHandHigher => player.hand.Value > dealer.hand.Value;

        //Hit player and dealer check for empty deck and if not hit respected party
        //Return true if shuffle is needed
        private bool HitPlayer()
        {
            bool needsShuffle = false;
            if(deck.cards.Count == 0)
            {
                needsShuffle = true;
            } else
            {
                player.Hit(deck.Deal());
            }
            return needsShuffle;
        }
        private bool HitDealer()
        {
            bool needsShuffle = false;
            if (deck.cards.Count == 0)
            {
                needsShuffle = true;
            }
            else
            {
                dealer.Hit(deck.Deal());
            }
            return needsShuffle;
        }
        private void Update()
        {
            //Check for a win or loss
            if(dealer.hand.IsBusted || (IsPlayerHandHigher && !player.hand.IsBusted))
            {
                //Player won, add to winnings logic here TODO
            } else if(player.hand.IsBusted || (IsDealerHandHigher && !dealer.hand.IsBusted))
            {
                //Player lost, remove from winnings logic here TODO
            }
            NeedsDeal = true; //If conditions are met we will make a new game otherwise, new deal
        }

        private void Save()
        {
            session.SetObject<Deck>("deck", deck);
            session.SetObject<Player>("player", player);
            session.SetObject<Dealer>("dealer", dealer);
            session.SetInt32("needsdeal", Convert.ToInt32(NeedsDeal));
        }
    }
}
